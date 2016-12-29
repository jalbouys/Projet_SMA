using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// gets list of all other agents on the field
/// </summary>

public class GetInfoBarbarian : MonoBehaviour
{

    public List<GameObject> barbarians = new List<GameObject>();
    public List<GameObject> guards = new List<GameObject>();
    public List<GameObject> villagers = new List<GameObject>();
    public List<GameObject> agressors = new List<GameObject>();
    public int hp = 100;
    public bool hitVillagersOverGuards = false;
    public bool findChestMode = true;
    public GameObject target = null;//or target pos?
    public GameObject attacker;
    public int fieldOfViewRange = 60;
    

    // Use this for initialization
    void Start()
    {
        hitVillagersOverGuards = false;
        findChestMode = true;
        GameObject chest = GameObject.FindGameObjectWithTag("Chest");
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        { barbarians.Add(barbarian); }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            if (CanSeeTarget(guard))
            {
                guards.Add(guard);
                if (target == null && guard.GetComponent<GetInfoGuard>().agressors.Count < 2) // Only choose as target if less than 2 attacking him
                    target = guard;
            }
        }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            if (CanSeeTarget(villager))
            {
                villagers.Add(villager);
                if ((target == null) || (hitVillagersOverGuards && target.tag == "Guard"))
                {
                    if (villager.GetComponent<GetInfoVillager>().agressors.Count < 2)
                        target = villager;
                }
            }
        }
        if (CanSeeTarget(chest) && findChestMode)
            target = chest;
        GetComponent<BarbarianMoveTo>().Target = target;
        GetComponent<Attack>().Target = target;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BarbarianCommunication>().hp = hp;
        GetComponent<BarbarianCommunication>().target = target;
        GameObject chest = GameObject.FindGameObjectWithTag("Chest");
        barbarians.Clear();
        guards.Clear();
        villagers.Clear();
        agressors.RemoveAll(item => item == null);
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        { barbarians.Add(barbarian); }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            if (CanSeeTarget(guard))
            {
                guards.Add(guard);
                if (target == null && guard.GetComponent<GetInfoGuard>().agressors.Count < 2)
                    target = guard;
            }

        }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            if (CanSeeTarget(villager))
            {
                villagers.Add(villager);
                if ((target == null) || (hitVillagersOverGuards && target.tag == "Guard"))
                {
                    if (villager.GetComponent<GetInfoVillager>().agressors.Count < 2)
                        target = villager;
                }
            }
        }
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

    bool CanSeeTarget(GameObject target)
    {
        
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

            if(Vector3.Angle(rayDirection,transform.forward) < fieldOfViewRange * 0.5f)
            {
                
                if (Physics.Raycast(transform.position,rayDirection,out hit))
                {
                    //Debug.Log("Tag : " + hit.transform.tag);
                    if (hit.transform.tag == target.tag)
                    {
                        //Debug.Log("Saw: " + target.tag);
                        return true;
                    }
                    else
                    {
                        //Debug.Log("Saw Nothing");
                        return false;
                    }
                }

            }
            return false;
    }


}

