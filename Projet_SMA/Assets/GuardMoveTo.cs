using UnityEngine;
using System.Collections;

/*Script related to movements of guard agents*/
public class GuardMoveTo : MonoBehaviour {
    
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
            if (distance > 30) //target too far, keep patrolling
            {
                debugMsg = "patrolling...";
                GetComponent<Animator>().Play("Walk");
                patrolling = true;
                defending = false;
                GetComponent<Defend>().helping = false;
            }
            else if (distance > 10) //target too far to defend, keep position
            {
                debugMsg = "Alert intruders coming!";
                patrolling = false;
                agent.destination = transform.position; //set the destination to current position, to force guard to stop
                transform.LookAt(Target.transform.position); //look at the target coming
            }
            else
            {
                if (defending == false) //already defending
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
            debugMsg = "patrolling...";
            GetComponent<Animator>().Play("Walk");
            patrolling = true;
            defending = false;
            GetComponent<Defend>().helping = false;
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
