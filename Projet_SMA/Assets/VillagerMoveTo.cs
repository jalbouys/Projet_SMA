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
    private string debugMsg = "";

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
            debugMsg = "Wandering around...";
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