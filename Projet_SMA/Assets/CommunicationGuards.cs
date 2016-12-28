using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommunicationGuards : MonoBehaviour {

    public List<GameObject> otherGuards;
    public List<GameObject> otherVillagers;
    public GameObject target;
    public int hp = 100;
    public bool attacked = false;

    void MessageReceived(string message)
    {
        //Debug.Log("MessageReceived: ");
        //Debug.Log(message + "\n");
        if (message == "help guard")
        {
            // We received an help message, we try to find who sent it
            foreach (GameObject guard in otherGuards)
            {
                var distance = Vector3.Distance(guard.transform.position, transform.position);
                if (distance < 10 && guard.transform.position != transform.position)//looking for a guard near us
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
        else if ((message == "help villager") && (transform.tag == "Guard"))
        {
            foreach (GameObject villager in otherVillagers)
            {
                var distance = Vector3.Distance(villager.transform.position, transform.position);
                if (distance < 10 && villager.transform.position != transform.position)//looking for a guard near us
                {
                    GameObject villagerAttacker = villager.GetComponent<VillagerMoveTo>().Attacker;
                    if (villager.GetComponent<CommunicationGuards>().hp < 20 && villagerAttacker != null)
                    {
                        GetComponent<Defend>().Target = villagerAttacker; //We found our friend who needs help, we change our target for his...
                        GetComponent<GuardMoveTo>().Target = villagerAttacker;
                        GetComponent<Defend>().helping = true;
                        GetComponent<GuardMoveTo>().DebugMsg = "Coming to help!";
                    }
                }
            }
        }
        else if ((transform.tag == "Villager") && message.StartsWith("ennemy:"))
        {
            GameObject ennemy = GameObject.Find(message.Substring(7));
            GetComponent<GetInfoVillager>().barbarians.Add(ennemy); // add to our list the barbarian seen by our friend
        }
    }
    // Use this for initialization
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
        otherVillagers.RemoveAll(item => item == null);
        if (transform.tag == "Guard")
        {
            hp = GetComponent<GetInfoGuard>().hp;
            foreach (GameObject guard in otherGuards)
            {
                var distance = Vector3.Distance(guard.transform.position, transform.position);
                if (distance < 10 && guard.transform.position != transform.position)//if other guard is less than 10m away...
                {
                    if (hp < 50 && GetComponent<Defend>().Target != null)
                        guard.GetComponent<CommunicationGuards>().MessageReceived("help guard");
                }
            }
        }
        else
        {
            hp = GetComponent<GetInfoVillager>().hp;
            foreach (GameObject guard in otherGuards)
            {
                var distance = Vector3.Distance(guard.transform.position, transform.position);
                if (distance < 10 && guard.transform.position != transform.position)//if guard is less than 10m away...
                {
                    if (hp < 20 && GetComponent<VillagerMoveTo>().Attacker != null) // if low life + being attacked: ask for help from guard
                        guard.GetComponent<CommunicationGuards>().MessageReceived("help villager");
                }
            }

            foreach (GameObject villager in otherVillagers)
            {
                var distance = Vector3.Distance(villager.transform.position, transform.position);
                if (distance < 10 && villager.transform.position != transform.position)//if villager is less than 10m away...
                {
                    if (GetComponent<VillagerMoveTo>().Attacker != null) // if low life + being attacked: ask for help from guard
                        villager.GetComponent<CommunicationGuards>().MessageReceived("ennemy:" + GetComponent<VillagerMoveTo>().Attacker.name);
                }
            }
        }
    }
}
