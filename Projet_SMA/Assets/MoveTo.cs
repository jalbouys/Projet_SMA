using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

    public GameObject personnage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (CanSeePlayer())
        {
            //print("There is the player");
            
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = personnage.transform.position;
            
            transform.LookAt(personnage.transform.position);
        }
	}

     bool CanSeePlayer() {
 
     int fieldOfViewRange = 60;
     RaycastHit hit;
     Vector3 rayDirection = personnage.transform.position - transform.position;
     if(Physics.Raycast(transform.position,rayDirection,out hit))
     { // If the player is very close behind the player and in view the enemy will detect the player
         if((hit.transform.tag == "Player"))
         {
         return true;
         }
     }
 
     if((Vector3.Angle(rayDirection, transform.forward)) < fieldOfViewRange){ // Detect if player is within the field of view
         if (Physics.Raycast (transform.position, rayDirection, out hit)) {
 
             if (hit.transform.tag == "Player") {
                 Debug.Log("Can see player");
                 return true;
             }else{
                 Debug.Log("Can not see player");
                 return false;
             }
         }
     }

     return false;
 }
}
