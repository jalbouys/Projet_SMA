using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

    private GameObject target = null;

    public float wanderRadius = 100;
    public float wanderTimer = 5;

    private float timer;
    private NavMeshAgent agent;

    public GameObject Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    // Use this for initialization
    void Start () {

        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Target != null) 
        {
            MoveToTarget(Target);
        }
        else// no enemy in sight => wander around
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

    void MoveToTarget(GameObject target)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target.transform.position;

        transform.LookAt(target.transform.position);
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
