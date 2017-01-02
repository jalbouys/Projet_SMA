using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/*This script contains all the information for barbarian agents :
    list of known/seen barbarians,guards,villagers,agressors
    hp, target, attacker,...
     */
public class GetInfoBarbarian : MonoBehaviour
{

    public List<GameObject> barbarians = new List<GameObject>();
    public List<GameObject> guards = new List<GameObject>();
    public List<GameObject> villagers = new List<GameObject>();
    public List<GameObject> agressors = new List<GameObject>();
    public int hp = 100;
    public bool hitVillagersOverGuards = false;
    public bool findChestMode = true;
    public GameObject target = null;
    public GameObject attacker;
    public int agressorsLimit = 3;

    //senses parameters
    public int fieldOfViewRange = 60; 
    public int sightRange = 40;
    public int smellRange = 5;


    /*Initialize the lists of barbarians, guards and villagers
        set a target if anybody in sight...*/
    void Start()
    {
        hitVillagersOverGuards = false;
        findChestMode = true;
        GameObject chest = GameObject.FindGameObjectWithTag("Chest");

        //adding barbarians
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        { barbarians.Add(barbarian); }

        //adding guards
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            if (CanSeeTarget(guard) || CanSmellTarget(guard)) //looking for on-sight guards
            {
                guards.Add(guard);
                if (target == null && guard.GetComponent<GetInfoGuard>().agressors.Count < agressorsLimit) // Only choose as target if less than 2 attacking him
                {
                    target = guard;
                    if(!guard.GetComponent<GetInfoGuard>().agressors.Contains(gameObject))
                        guard.GetComponent<GetInfoGuard>().agressors.Add(gameObject);
                }
            }
        }

        //adding villagers
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            if (CanSeeTarget(villager) || CanSmellTarget(villager)) //looking for on-sight villagers
            {
                villagers.Add(villager);
                if ((target == null) || (hitVillagersOverGuards && target.tag == "Guard")) // In case target is not null and Villager 
                {                                                                // has priority, we change for the new target
                    if (villager.GetComponent<GetInfoVillager>().agressors.Count < agressorsLimit)// Only choose as target if less than 2 attacking him
                    {
                        target = villager;
                        if (!villager.GetComponent<GetInfoVillager>().agressors.Contains(gameObject))
                            villager.GetComponent<GetInfoVillager>().agressors.Add(gameObject);
                    }
                }
            }
        }
        //look for the chest, depending on the objective
        if (CanSeeTarget(chest) && findChestMode)
            target = chest;
        
        //update the target
        GetComponent<BarbarianMoveTo>().Target = target;
        GetComponent<Attack>().Target = target;
    }

    /*Called once a frame: Gets the new lists of barbarians, guards and villagers
        (in case someone died) set a target if anybody in sight...*/
    void Update()
    {
        //check hp and target from communication script in case it has changed...
        GetComponent<BarbarianCommunication>().hp = hp;
        GetComponent<BarbarianCommunication>().target = target;

        GameObject chest = GameObject.FindGameObjectWithTag("Chest");

        barbarians.Clear();
        guards.Clear();
        villagers.Clear();

        //remove all the agressors who died since last update
        agressors.RemoveAll(item => item == null);
        
        //adding barbarians
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        { barbarians.Add(barbarian); }

        //adding guards
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            if (CanSeeTarget(guard) || CanSmellTarget(guard)) //looking for on-sight guards
            {
                guards.Add(guard);
                if (target == null && guard.GetComponent<GetInfoGuard>().agressors.Count < agressorsLimit) // Only choose as target if less than 2 attacking him
                {
                    target = guard;
                    if (!guard.GetComponent<GetInfoGuard>().agressors.Contains(gameObject))
                        guard.GetComponent<GetInfoGuard>().agressors.Add(gameObject);
                }
            }
        }

        //adding villagers
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            if (CanSeeTarget(villager) || CanSmellTarget(villager)) //looking for on-sight villagers
            {
                villagers.Add(villager);
                if ((target == null) || (hitVillagersOverGuards && target.tag == "Guard")) // In case target is not null and Villager 
                {                                                                // has priority, we change for the new target
                    if (villager.GetComponent<GetInfoVillager>().agressors.Count < agressorsLimit)// Only choose as target if less than 2 attacking him
                    {
                        target = villager;
                        if (!villager.GetComponent<GetInfoVillager>().agressors.Contains(gameObject))
                            villager.GetComponent<GetInfoVillager>().agressors.Add(gameObject);
                    }
                }
            }
        }
        //look for the chest, depending on the objective
        if (CanSeeTarget(chest) && findChestMode)
            target = chest;

        if ((villagers.Count == 0) && (guards.Count == 0))//Did not see anybody
        { 
            GetComponent<BarbarianMoveTo>().Target = null;
            GetComponent<Attack>().Target = null;
            GetComponent<BarbarianCommunication>().target = null;
        }
        else
        {
            GetComponent<BarbarianMoveTo>().Target = target;
            GetComponent<Attack>().Target = target;
            GetComponent<BarbarianCommunication>().target = target;
        }

        // In case of no hp left, we destroy the object and all the pointers to it...
        if (hp <= 0)
        {
            foreach (GameObject agressor in agressors)
            {
                if (agressor != null)
                    agressor.GetComponent<GetInfoGuard>().target = null;
            }
            Destroy(gameObject);
        }
    }

    /*Useful function to tell if a target is on sight or not*/
    bool CanSeeTarget(GameObject target)
    {
        
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

            if(Vector3.Angle(rayDirection,transform.forward) < fieldOfViewRange * 0.5f)
            {
                
                if (Physics.Raycast(transform.position,rayDirection,out hit))
                {
                    if (hit.transform.tag == target.tag)
                    {
                    if (Vector3.Distance(transform.position, target.transform.position) < sightRange)
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

    /*Tells if a target is within "smell" range*/
    bool CanSmellTarget(GameObject target)
    {
        return (Vector3.Distance(transform.position, target.transform.position) < smellRange);

    }


}

