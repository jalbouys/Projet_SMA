using System.Collections;
using UnityEngine;

public class BarbarianMoveTo : MonoBehaviour {

    private GameObject target = null;

    public float wanderRadius = 100;
    public float wanderTimer = 5;

    private float timer;
    private NavMeshAgent agent;
    private string debugMsg = "";
    private Vector3 previousTargetPosition;
    private Vector3 villageEntrance = new Vector3(-18, 0, 20);
    private bool movingToVillage = false;

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

    public string DebugMsg
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
            Attack barbAttack = GetComponent<Attack>();
            if (!barbAttack.helping && !barbAttack.attacking)
            {
                debugMsg = "Target locked: " + target.tag;
            }
            MoveToTarget(Target);
            movingToVillage = false;
        }
        else if (movingToVillage)
        {
            debugMsg = "Moving towards village";
        }
        else// no enemy in sight => wander around
        {
            debugMsg = "Wandering around...";
            timer += Time.deltaTime;
            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
    }


    public void moveToVillage()
    {
        GetComponent<NavMeshAgent>().SetDestination(villageEntrance);
        transform.LookAt(villageEntrance);
        movingToVillage = true;
    }

    void MoveToTarget(GameObject target)
    {
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

    //Smart debugger
    void OnGUI()
    {
        Vector3 characterPos = Camera.main.WorldToScreenPoint(transform.position);
        characterPos = new Vector3(Mathf.Clamp(characterPos.x, 0 + (100 / 2), Screen.width - (100 / 2)),
                                           Mathf.Clamp(characterPos.y, 50, Screen.height),
                                           characterPos.z);
        GUILayout.BeginArea(new Rect((characterPos.x + 0) - (100 / 2), (Screen.height - characterPos.y) + 0, 100, 100));

        GUI.Label(new Rect(0, 0, 100, 100), debugMsg);
        GUILayout.EndArea();
    }
}
