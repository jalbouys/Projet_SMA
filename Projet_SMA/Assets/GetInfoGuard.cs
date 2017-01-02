using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*This script contains all the information for guard agents :
    list of known/seen barbarians,guards,villagers,agressors
    hp, target, attacker,...
     */
public class GetInfoGuard : MonoBehaviour
{

    public List<GameObject> barbarians = new List<GameObject>();
    public List<GameObject> guards = new List<GameObject>();
    public List<GameObject> villagers = new List<GameObject>();
    public List<GameObject> agressors = new List<GameObject>();
    public int hp = 100;
    public GameObject target = null;//or target pos?
    public GameObject attacker;
    public int fieldOfViewRange = 60;

    /*Initialize the lists of barbarians, guards and villagers
        set a target if any barbarian on sight...*/
    void Start()
    {
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        {
            if (CanSeeTarget(barbarian)) //If barbarian on sight
            {
                barbarians.Add(barbarian); //add him to the list
                if (target == null || IsCloser(barbarian, target)) //In case closer than previous target, change to new target
                    target = barbarian;
            }
        }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            guards.Add(guard);
        }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            villagers.Add(villager);
        }
        GetComponent<GuardMoveTo>().Target = target;
        GetComponent<Defend>().Target = target;

    }

    // Update is called once per frame
    void Update()
    {
        barbarians.Clear();
        guards.Clear();
        villagers.Clear();
        villagers.Clear();
        agressors.RemoveAll(item => item == null);
        if (agressors.Count != 0 && target == null) // Defend himself when attacked
        {
            target = agressors[agressors.Count - 1];
        }

        GameObject agr = null;
        for(int i=0; i<agressors.Count;i++)
        {
            agr = agressors[i];
            if (agr.GetComponent<Attack>().Target != gameObject) //Test if all agressors are still targeting him or not
                agressors[i] = null;
        }
        agressors.RemoveAll(item => item == null); //remove all null agressors

        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        {
            if (CanSeeTarget(barbarian)) //If barbarian on sight
            {
                barbarians.Add(barbarian); //add him to the list
                if (target == null || IsCloser(barbarian, target)) //In case closer than previous target, change to new target
                    target = barbarian;
            }
        }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        { guards.Add(guard); }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        { villagers.Add(villager); }

        if (target != null && !GetComponent<Defend>().helping) //Don't update the target while helping someone else
        {
            GetComponent<GuardMoveTo>().Target = target;
            GetComponent<Defend>().Target = target;
        }
        
        //Deal with death of agent
        if (hp <= 0)
        {
            foreach (GameObject agressor in agressors)
            {
                if (agressor != null)
                  agressor.GetComponent<GetInfoBarbarian>().target = null;
            }
            Destroy(gameObject);
        }
    }

    /*Useful function to tell if a target is on sight or not*/
    bool CanSeeTarget(GameObject target)
    {

        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        if (Vector3.Angle(rayDirection, transform.forward) < fieldOfViewRange * 0.5f)
        {

            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.tag == target.tag)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < 20) //Only see targets from less than 20m
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }

        }
        return false;
    }

    /* tells whether a given object is closer than another one or not*/
    bool IsCloser(GameObject current, GameObject prev)
    {
        return (Vector3.Distance(current.transform.position, transform.position) < Vector3.Distance(prev.transform.position, transform.position));
    }
}
