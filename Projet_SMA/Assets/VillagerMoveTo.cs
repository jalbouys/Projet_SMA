using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillagerMoveTo : MonoBehaviour
{

    public GameObject personnage;
    //wandering variable
    public float wanderRadius = 100;
    public float wanderTimer = 5;
    public bool villageInvaded = false;
    private List<GameObject> houses = new List<GameObject>();
    private GameObject attacker = null;
    private NavMeshAgent agent;
    private float timer;
    private string debugMsg = "";
    private bool movingToSafePlace = false;

    public GameObject Attacker
    {
        get
        {
            return attacker;
        }

        set
        {
            attacker = value;
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
    void Start()
    {
        //wandering stuff
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        foreach (GameObject house in GameObject.FindGameObjectsWithTag("House"))
        {
            houses.Add(house);
        }
    }

    // Update is called once per frame
    void Update()
    {

        Animator charAnim = GetComponent<Animator>();
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (villageInvaded)
        {
            debugMsg = "Running Home, Village invaded!";
            charAnim.Play("Walk");
            charAnim.speed = 1;
            if (!movingToSafePlace)
            {
                agent.SetDestination(FindClosestHouse(transform.position));
                movingToSafePlace = true;
            }
            else if (Vector3.Distance(transform.position, FindClosestHouse(transform.position)) < 5)
            {
                villageInvaded = false;
                movingToSafePlace = false;
            }

        }
        else if (attacker != null && Vector3.Distance(attacker.transform.position, transform.position)<10)
        {
            debugMsg = "Running away from ennemy!";
            charAnim.Play("Walk");
            charAnim.speed = 1;
            if (!movingToSafePlace)
            {
                agent.SetDestination(FindClosestHouse(transform.position));
                movingToSafePlace = true;
            }
            else if (Vector3.Distance(transform.position, FindClosestHouse(transform.position)) < 5)
            {
                villageInvaded = false;
                movingToSafePlace = false;
            }
        }
        else//no enemy in sight => wander around
        {
            debugMsg = "Wandering peacefully around...";
            charAnim.Play("Walk");
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

    private Vector3 FindClosestHouse(Vector3 position) {
        float dist = 10000;
        Vector3 dest = position;
        foreach(GameObject house in houses)
        {
            if (Vector3.Distance(position, house.transform.position) < dist)
            {
                dist = Vector3.Distance(position, house.transform.position);
                dest = house.transform.position;
            }
        }
        return dest;
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