using UnityEngine;
using System.Collections;

public class VillagerMoveTo : MonoBehaviour
{

    public GameObject personnage;
    //wandering variable
    public float wanderRadius = 100;
    public float wanderTimer = 5;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    // Use this for initialization
    void Start()
    {
        //wandering stuff
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (0 == 1)// CanSeePlayer())
        {
            //print("There is the player");

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = personnage.transform.position;

            transform.LookAt(personnage.transform.position);
        }
        else//no enemy in sight => wander around
        {
            Animator charAnim = GetComponent<Animator>();
            charAnim.speed = 1;
            timer += Time.deltaTime;
            if (timer >= wanderTimer)
            {

                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
    }


    //wander stuff
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

}