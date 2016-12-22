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
        Debug.Log("MessageReceived\n");
        if (message == "jump")
        {
            transform.position.Set(transform.position.x, transform.position.y, 50.0F);
            Debug.Log("Jump\n");
        }
        else if (message == "move")
        {
            //transform.position = Vector3.MoveTowards(transform.position, personnage.transform.position, Time.deltaTime * 5);
        }
        else if (message == "help")
        {
            //Si un villageois ou un gard appelle à l'aide et que l'on est pas attaqué on ira peut être l'aider
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
        foreach (GameObject guard in otherGuards)
        {
            var distance = Vector3.Distance(guard.transform.position, transform.position);
            if (distance < 10 && guard.transform.position != transform.position)//if other barbarian is less than 10m away...
            {
                if (hp < 50 && attacked == true)
                    guard.GetComponent<CommunicationGuards>().MessageReceived("help");
            }
        }
    }
}
