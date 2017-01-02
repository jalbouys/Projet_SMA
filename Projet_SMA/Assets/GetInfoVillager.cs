using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*This script contains all the information for guard agents :
    list of known/seen barbarians,guards,villagers,agressors
    hp, target, attacker,...
     */
public class GetInfoVillager : MonoBehaviour
{

    public List<GameObject> barbarians = new List<GameObject>();
    public List<GameObject> guards = new List<GameObject>();
    public List<GameObject> villagers = new List<GameObject>();
    public List<GameObject> agressors = new List<GameObject>();
    public int hp = 100;
    public GameObject target;//or target pos?
    public GameObject attacker = null;
    public int fieldOfViewRange = 60;

    /*Initialize the lists of barbarians, guards and villagers
        set a target if any barbarian on sight...*/
    void Start()
    {
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        {
            if (CanSeeTarget(barbarian))//If barbarian on sight
            {
                barbarians.Add(barbarian);//add him to the list
                if (attacker == null || IsCloser(barbarian,attacker)) //should not be done like that, needs to be changed
                    attacker = barbarian;
            }
        }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        { guards.Add(guard); }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            villagers.Add(villager);
        }
        GetComponent<VillagerMoveTo>().Attacker = attacker;
    }

    // Update is called once per frame
    void Update()
    {
        barbarians.Clear();
        guards.Clear();
        villagers.Clear();
        agressors.RemoveAll(item => item == null);
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        {
            if (CanSeeTarget(barbarian)) //If barbarian on sight
            {
                barbarians.Add(barbarian); //add him to the list
                if (attacker == null || IsCloser(barbarian, attacker))
                    attacker = barbarian;
            }
        }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        { guards.Add(guard); }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        { villagers.Add(villager); }

        if(barbarians.Count > 3) //If more than 3 barbarians seen, understand that village is being invaded
        {
            GetComponent<VillagerMoveTo>().villageInvaded = true;
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

        GetComponent<VillagerMoveTo>().Attacker = attacker;
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

    bool IsCloser(GameObject current, GameObject prev)
    {
        return (Vector3.Distance(current.transform.position, transform.position) < Vector3.Distance(prev.transform.position, transform.position));
    }

}
