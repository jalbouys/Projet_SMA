using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*Communication script for barbarians*/
public class BarbarianCommunication : MonoBehaviour {

    public List<GameObject> otherBarbarians;
    public GameObject target;
    public int hp = 100;
    public bool attacked = false;

    /*message handler function*/
    void MessageReceived(string message)
    {
            // We received an help message, we try to find who sent it
       foreach (GameObject barbarian in otherBarbarians)
       {
            if (barbarian == null)
                continue;
        var distance = Vector3.Distance(barbarian.transform.position, transform.position);
        if (distance < 10 && barbarian.transform.position != transform.position)//looking for a barbarian near us
        {
            if (message == "help") // case of "help" message received
            {
                GameObject barbarianTarget = barbarian.GetComponent<Attack>().Target;
                if (barbarian.GetComponent<BarbarianCommunication>().hp < 50 && barbarianTarget != null)
                {
                    GetComponent<Attack>().Target = barbarianTarget; //We found our friend who needs help, we change our target for his...
                    GetComponent<BarbarianMoveTo>().Target = barbarianTarget;
                    GetComponent<Attack>().helping = true;
                    GetComponent<BarbarianMoveTo>().DebugMsg = "Coming to help!";
                }
            }
            else if (message == "attacking") // case of "attacking" message received
                {
                    GameObject barbarianTarget = barbarian.GetComponent<BarbarianMoveTo>().Target;
                    Attack myAttack = GetComponent<Attack>();
                    if (myAttack.Target == null && barbarianTarget != null) // Case of ally attacking + and no target
                    {
                        if ((barbarianTarget.tag == "Guard" && barbarianTarget.GetComponent<GetInfoGuard>().agressors.Count < 2) ||
                            (barbarianTarget.tag == "Villager" && barbarianTarget.GetComponent<GetInfoVillager>().agressors.Count < 2))
                            myAttack.Target = barbarianTarget;
                    }

                }    
        }
       }
    }

    /*initialize the list of barbarians at the beginning*/
    void Start()
    {
        foreach (GameObject otherBarbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        {
            otherBarbarians.Add(otherBarbarian);
        }
    }

    // Update is called once per frame
    void Update()
    {
        otherBarbarians.RemoveAll(item => item == null); //remove null items to avoid errors below
        hp = GetComponent<GetInfoBarbarian>().hp;
        foreach (GameObject barbarian in otherBarbarians) //check all the barbarians
        {
            var distance = Vector3.Distance(barbarian.transform.position, transform.position);
            if (distance < 10 && barbarian.transform.position != transform.position)//if other barbarian is less than 10m away...
            {
                if (hp < 50 && GetComponent<GetInfoBarbarian>().agressors.Count != 0 && GetComponent<Attack>().attackingAlone)
                    barbarian.GetComponent<BarbarianCommunication>().MessageReceived("help"); //ask for help
                else if (GetComponent<BarbarianMoveTo>().Target != null)
                    barbarian.GetComponent<BarbarianCommunication>().MessageReceived("attacking");//send the target to our friends around
                
            }
        }
    }
}
