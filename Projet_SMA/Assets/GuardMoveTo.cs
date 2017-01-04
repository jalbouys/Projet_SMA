using UnityEngine;
using System.Collections;

/*Script related to movements of guard agents*/
public class GuardMoveTo : MonoBehaviour {

    public float wanderRadius = 100;//random wandering variables
    public float wanderTimer = 5;
    private float timer;
    private Vector3 villageCenter = new Vector3(-23, 0, -20);
    private bool movingToVillage = false;

    public Transform[] points; //patrolling parameters
    private int destPoint = 0;

    private NavMeshAgent agent;
    public bool attacked = false;
    private string debugMsg = "";
    private GameObject target;
    public bool patrolling = true;
    private bool defending = false;

    public string DebugMsg //debugging string, for on-screen debug
    {
        get
        {
            return debugMsg;
        }

        set
        {
            debugMsg = value;
        }
    }
    public GameObject Target //agent's target
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
        agent.autoBraking = false;
        patrolling = true;
        timer = wanderTimer;//for wandering

       GotoNextPoint();
    }


    //patrolling function, to move from one point to another one
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

    //random wander stuff
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void Update()
    {

        if (attacked == true)
        {
            debugMsg = "Alert!";
        }
       else if ((agent.remainingDistance < 1.5f) && patrolling) //patrolling routine
            GotoNextPoint();

       if(Target != null)
        {
            var distance = Vector3.Distance(Target.transform.position, transform.position);
            if (distance > 30 && !patrolling) //target too far, wander
            {
                debugMsg = "giving up...";
                GetComponent<Animator>().Play("Walk");
                defending = false;
                GetComponent<Defend>().helping = false;
            }
            else if ((distance > 10) && (patrolling == true)) //target too far to defend, keep position
            {
                debugMsg = "Alert intruders coming!";
                agent.destination = transform.position; //set the destination to current position, to force guard to stop
                transform.LookAt(Target.transform.position); //look at the target coming
            }
            else
            {
                if (defending == false) //not already defending
                {
                    debugMsg = "defending the village!";
                    patrolling = false;
                    defending = true;
                }
                MoveToTarget(Target);
            }
        }
        if(Target == null)
        {
            if (patrolling == true)
            {
                debugMsg = "patrolling...";
                GetComponent<Animator>().Play("Walk");
                //patrolling = true;
                defending = false;
                GetComponent<Defend>().helping = false;
            }
            else//wandering
            {
                defending = false;
                GetComponent<Defend>().helping = false;

                if (agent.areaMask != 8)//not in village yet
                {
                    debugMsg = "Going inside the village...";
                    if (Vector3.Distance(transform.position, villageCenter) < 10)
                    {
                        agent.areaMask = 8; //village area mask
                    }

                    GetComponent<NavMeshAgent>().SetDestination(villageCenter);
                    transform.LookAt(villageCenter);
                }
                else
                {
                    debugMsg = "Wandering...";
                    //agent.areaMask = 8; //village area mask
                    timer += Time.deltaTime;
                    if (timer >= wanderTimer)
                    {
                        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                        agent.SetDestination(newPos);
                        timer = 0;
                    }
                }
            }
        }
    }

    /*Function used to move to a given target*/
    void MoveToTarget(GameObject target)
    {
        agent.destination = target.transform.position;

        transform.LookAt(target.transform.position);
    }

    //Smart debugger
    void OnGUI()
    {
        Vector3 characterPos = Camera.main.WorldToScreenPoint(transform.position);
        characterPos = new Vector3(Mathf.Clamp(characterPos.x, 15 + (100 / 2), Screen.width - (100 / 2)),
                                           Mathf.Clamp(characterPos.y, 50, Screen.height),
                                           characterPos.z);
        GUILayout.BeginArea(new Rect((characterPos.x + 15) - (100 / 2), (Screen.height - characterPos.y) - 15 , 100, 100));

        GUI.Label(new Rect(0, 0, 100, 100), debugMsg);
        GUILayout.EndArea();
    }


}
