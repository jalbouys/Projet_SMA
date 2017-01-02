using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*Communication script for guards and villagers*/
public class CommunicationGuards : MonoBehaviour {

    public List<GameObject> otherGuards;
    public List<GameObject> otherVillagers;
    public GameObject target;
    public int hp = 100;
    public bool attacked = false;
    public int shoutingDistance = 20;

    void MessageReceived(string message)
    {
        if (message == "help guard") /* received "help" from a guard */
        {
            // We received an help message, we try to find who sent it
            foreach (GameObject guard in otherGuards)
            {
                if (guard == null)
                    continue;

                var distance = Vector3.Distance(guard.transform.position, transform.position);
                if (distance < shoutingDistance && guard.transform.position != transform.position)//looking for a guard near us
                {
                    GameObject otherGuardTarget = guard.GetComponent<Defend>().Target;
                    if (guard.GetComponent<CommunicationGuards>().hp < 50 &&  otherGuardTarget != null)
                    {
                        GetComponent<Defend>().Target = otherGuardTarget; //We found our friend who needs help, we change our target for his...
                        GetComponent<GuardMoveTo>().Target = otherGuardTarget;
                        GetComponent<Defend>().helping = true;
                        GetComponent<GuardMoveTo>().DebugMsg = "Coming to help!";
                    }
                }
            }
        }
        else if ((message == "help villager") && (transform.tag == "Guard")) /* received "help" from a villager and we are a guard */
        {
            foreach (GameObject villager in otherVillagers)
            {
                if (villager == null)
                    continue;
                var distance = Vector3.Distance(villager.transform.position, transform.position);
                if (distance < shoutingDistance && villager.transform.position != transform.position)//looking for a villager near us
                {
                    GameObject villagerAttacker = villager.GetComponent<VillagerMoveTo>().Attacker;
                    if (villager.GetComponent<CommunicationGuards>().hp < 40 && villagerAttacker != null)
                    {
                        GetComponent<Defend>().Target = villagerAttacker; //We found our friend who needs help, we change our target for his...
                        GetComponent<GuardMoveTo>().Target = villagerAttacker;
                        GetComponent<Defend>().helping = true;
                        GetComponent<GuardMoveTo>().DebugMsg = "Coming to help!";
                    }
                }
            }
        }
        else if ((transform.tag == "Villager") && message.StartsWith("ennemy:")) /*We are a villager, received new ennemy info*/
        {
            GameObject ennemy = GameObject.Find(message.Substring(7));
            GetComponent<GetInfoVillager>().barbarians.Add(ennemy); // add to our list the barbarian seen by our friend
        }
    }
    // Use this for initialization of guards and villagers lists
    void Start()
    {
        foreach (GameObject otherGuard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            otherGuards.Add(otherGuard);
        }

        foreach (GameObject otherVillager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            otherVillagers.Add(otherVillager);
        }

    }

    // Update is called once per frame
    void Update()
    {
        otherGuards.RemoveAll(item => item == null);
        otherVillagers.RemoveAll(item => item == null); //remove null elements to prevent from an error below
        if (transform.tag == "Guard") //If we are a guard
        {
            hp = GetComponent<GetInfoGuard>().hp;
            foreach (GameObject guard in otherGuards)
            {
                var distance = Vector3.Distance(guard.transform.position, transform.position);
                if (distance < shoutingDistance && guard.transform.position != transform.position)//if other guard is less than shoutingDistance away...
                {
                    if (hp < 50 && GetComponent<Defend>().Target != null)
                        guard.GetComponent<CommunicationGuards>().MessageReceived("help guard"); //ask for help from a guard
                }
            }
        }
        else //If we are a villager
        {
            hp = GetComponent<GetInfoVillager>().hp;
            foreach (GameObject guard in otherGuards)
            {
                var distance = Vector3.Distance(guard.transform.position, transform.position);
                if (distance < shoutingDistance && guard.transform.position != transform.position)//if guard is less than shoutingDistance away...
                {
                    if (hp < 20 && GetComponent<VillagerMoveTo>().Attacker != null) // if low life + being attacked: ask for help from guard
                        guard.GetComponent<CommunicationGuards>().MessageReceived("help villager");
                }
            }

            foreach (GameObject villager in otherVillagers)
            {
                var distance = Vector3.Distance(villager.transform.position, transform.position);
                if (distance < shoutingDistance && villager.transform.position != transform.position)//if villager is less than shoutingDistance away...
                {
                    if (GetComponent<VillagerMoveTo>().Attacker != null) // if there is an attacker, send it to our friends around
                        villager.GetComponent<CommunicationGuards>().MessageReceived("ennemy:" + GetComponent<VillagerMoveTo>().Attacker.name);
                }
            }
        }
    }
}
