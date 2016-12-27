using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarbarianCommunication : MonoBehaviour {

    public List<GameObject> otherBarbarians;
    public GameObject target;
    public int hp = 100;
    public bool attacked = false;

    void MessageReceived(string message)
    {
            // We received an help message, we try to find who sent it
       foreach (GameObject barbarian in otherBarbarians)
       {
        if (barbarian == null)
            continue;
        var distance = Vector3.Distance(barbarian.transform.position, transform.position);
        if (distance < 10 && barbarian.transform.position != transform.position)//looking for a guard near us
        {
            if (message == "help")
            {
                   // Debug.Log("received help");
                GameObject barbarianTarget = barbarian.GetComponent<Attack>().Target;
                if (barbarian.GetComponent<BarbarianCommunication>().hp < 50 && barbarianTarget != null)
                {
                    GetComponent<Attack>().Target = barbarianTarget; //We found our friend who needs help, we change our target for his...
                    GetComponent<MoveTo>().Target = barbarianTarget;
                    GetComponent<Attack>().helping = true;
                    GetComponent<MoveTo>().DebugMsg = "Coming to help!";
                }
            }
            else if (message == "attacking")
                {
                    GameObject barbarianTarget = barbarian.GetComponent<MoveTo>().Target;
                    Attack myAttack = GetComponent<Attack>();
                    if (myAttack.Target == null && barbarianTarget != null) // Case of ally attacking + and no target
                    {
                        myAttack.Target = barbarianTarget;
                        //Debug.Log("New target acquired");
                    }

                }    
        }
       }
    }

    // Use this for initialization
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

        hp = GetComponent<GetInfoBarbarian>().hp;
        foreach (GameObject barbarian in otherBarbarians)
        {
            if (barbarian == null)
                continue;
            var distance = Vector3.Distance(barbarian.transform.position, transform.position);
            if (distance < 10 && barbarian.transform.position != transform.position)//if other barbarian is less than 10m away...
            {
                if (hp < 50 && GetComponent<GetInfoBarbarian>().agressors.Count != 0)
                    barbarian.GetComponent<BarbarianCommunication>().MessageReceived("help");
                else if (GetComponent<MoveTo>().Target != null)
                {
                    barbarian.GetComponent<BarbarianCommunication>().MessageReceived("attacking");
                }
            }
        }
    }
}
