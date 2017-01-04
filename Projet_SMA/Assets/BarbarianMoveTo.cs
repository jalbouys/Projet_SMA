using System.Collections;
using UnityEngine;

/*Script related to movements of barbarian agents*/
public class BarbarianMoveTo : MonoBehaviour {


    public float wanderRadius = 100;//random wandering variables
    public float wanderTimer = 5;
    private float timer;

    private NavMeshAgent agent;
    private string debugMsg = "";
    private Vector3 previousTargetPosition;
    private Vector3 villageCenter = new Vector3(-23, 0, -20);
    private Vector3 villageEntrance = new Vector3(-18,0,18);
    private bool movingToVillage = false;

    private bool gotTheChest = false;
    private bool movingWithChest = false;
    private GameObject target = null;

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

    // Use this for initialization
    void Start () {

        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }
	
	// Update is called once per frame
	void Update () {
        
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (gotTheChest)
        {
            debugMsg = "Bringing chest outside!";
            if (!movingWithChest)
            {
                agent.SetDestination(villageEntrance);
                transform.LookAt(villageEntrance);
                movingWithChest = true;
            }
            else
            {
                GameObject chest = GameObject.FindGameObjectWithTag("Chest");
                chest.transform.position = new Vector3(transform.position.x,transform.position.y + 2,transform.position.z + 2);
                //chest.transform.LookAt(transform.position);
                if (Vector3.Distance(villageEntrance, chest.transform.position) < 5)
                    chest.GetComponent<ChestInfo>().hasBeenStolen = true;
            }
        }
        else if (Target != null)
        {
            Attack barbAttack = GetComponent<Attack>();
            if (!barbAttack.helping && !barbAttack.attacking) //test to avoid message erasing
            {
                debugMsg = "Target locked: " + target.tag;
            }
            MoveToTarget(Target); //move towards the target
            movingToVillage = false; //make sure that we are not moving towards village anymore
            if (target.tag == "Chest" && Vector3.Distance(transform.position, target.transform.position) < 5)
            {
                GameObject chest = GameObject.FindGameObjectWithTag("Chest");
                if (chest.GetComponent<ChestInfo>().carrier == null)
                {
                    gotTheChest = true;
                    chest.GetComponent<ChestInfo>().carrier = gameObject;
                }
                Target = null;
                GetComponent<Attack>().Target = null;
                GetComponent<GetInfoBarbarian>().target = null;
            }
        }
        else if (movingToVillage)
        {
            debugMsg = "Moving towards village";
            if (Vector3.Distance(transform.position, villageCenter) < 4 || transform.position.z < (villageCenter.z + 10) )
            {
                movingToVillage = false;
                agent.areaMask = 8; //village area mask
            }
        }
        else// no enemy in sight => wander around
        {
            if (agent.areaMask != 8) // In case he hasn't entered the village yet
            {
                moveToVillage();
            }
            else
            {
                debugMsg = "Wandering around...";
                Attack barbAttack = GetComponent<Attack>();

                if (barbAttack.helping) //force variables to false, because bugged otherwise
                    barbAttack.helping = false;

                if (barbAttack.attacking)
                    barbAttack.attacking = false;

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

    /*Move towards village entrance set in villageEntrance variable*/
    public void moveToVillage()
    {
        GetComponent<NavMeshAgent>().SetDestination(villageCenter);
        transform.LookAt(villageCenter);
        movingToVillage = true;
    }

    /*Function used to move to a given target*/
    void MoveToTarget(GameObject target)
    {
        agent.destination = target.transform.position;

        transform.LookAt(target.transform.position);
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
