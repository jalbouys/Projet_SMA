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
                if (guard == null)
                    continue;
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
        hp = GetComponent<GetInfoGuard>().hp;
        otherGuards.RemoveAll(item => item == null);
        otherVillagers.RemoveAll(item => item == null);
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
}
