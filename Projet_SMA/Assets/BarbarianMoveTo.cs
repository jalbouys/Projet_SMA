using UnityEngine;
using System.Collections;

public class BarbarianMoveTo : MonoBehaviour {

    private Vector3 previousTargetPosition;
    private Vector3 villageEntrance = new Vector3(-18, 0, 20);
    private RaycastHit villageHit = new RaycastHit();
    // Use this for initialization
    void Start()
    {
        villageHit.point = villageEntrance;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void moveToVillage()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = villageHit.point;
        transform.LookAt(villageHit.point);
    }

    //On vérifie si l'on voit une entité précise.
    public bool CanSeeEntity(GameObject personnage)
    {

        int fieldOfViewRange = 60;
        RaycastHit hit;
        Vector3 rayDirection = personnage.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        { // If the player is very close behind the player and in view the enemy will detect the player
            if ((hit.transform.tag == "Guard"))
            {
                return true;
            }
        }

        if ((Vector3.Angle(rayDirection, transform.forward)) < fieldOfViewRange)
        { // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {

                if (hit.transform.tag == "Guard")
                {
                    Debug.Log("Can see Guard");
                    return true;
                }
                else
                {
                    //Debug.Log("Can not see anything");
                    return false;
                }
            }
        }

        return false;
    }
}
