using UnityEngine;
using System.Collections;

public class VillagerMoveTo : MonoBehaviour {

    public GameObject personnage;
	
	//wandering variables
    public float wanderRadius = 100;
    public float wanderTimer = 5;
 
    private Transform target;
    private NavMeshAgent agent;
    private float timer;

	// Use this for initialization
	void Start () {
		//wandering stuff
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (0==1)// CanSeePlayer())
        {
            //print("There is the player");
            
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = personnage.transform.position;
            
            transform.LookAt(personnage.transform.position);
        }
		else//no enemy in sight => wander around
		{
			timer += Time.deltaTime;
			if (timer >= wanderTimer) {
				Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
				agent.SetDestination(newPos);
				timer = 0;
			}
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
 
 //wander stuff
 public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
	
}
