using UnityEngine;
using System.Collections;

public class GuardMoveTo : MonoBehaviour {

    public GameObject personnage;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    public bool attacked = false;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (attacked == true)
        {
            Debug.Log("Attacked!");
        }
        else if (agent.remainingDistance < 1f)
            GotoNextPoint();
    }

    public bool CanSeeEntity() {
 
     int fieldOfViewRange = 60;
     RaycastHit hit;
     Vector3 rayDirection = personnage.transform.position - transform.position;

     if (Physics.Raycast(transform.position,rayDirection,out hit))
     { // If the player is very close behind the player and in view the enemy will detect the player

            string tagName = hit.transform.tag;
            if ((tagName == "Barbarian"))
         {
             Debug.Log("Alert! Attacker here!!");
             return true;
         }
     }
 
     if((Vector3.Angle(rayDirection, transform.forward)) < fieldOfViewRange){ // Detect if player is within the field of view
         if (Physics.Raycast (transform.position, rayDirection, out hit)) {

             string tagName = hit.transform.tag;
 
             if (tagName == "Barbarian") {
                 Debug.Log("Can see Barbarian");
                 return true;
             }else{
                 Debug.Log("Can not see any Barbarian");
                 return false;
             }
         }
     }

     return false;
 }


}
