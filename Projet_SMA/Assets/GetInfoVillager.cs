using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// gets list of all other agents on the field
/// </summary>

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

    // Use this for initialization
    void Start()
    {
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        {
            if (CanSeeTarget(barbarian))
            {
                barbarians.Add(barbarian);
                if (attacker == null || IsClosest(barbarian,attacker))
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
            if (CanSeeTarget(barbarian))
            {
                barbarians.Add(barbarian);
                if (attacker == null || IsClosest(barbarian, attacker))
                    attacker = barbarian;
            }
        }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        { guards.Add(guard); }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        { villagers.Add(villager); }

        //Debug.Log(hp);
        if(hp <= 0)
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


    bool CanSeeTarget(GameObject target)
    {

        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        if (Vector3.Angle(rayDirection, transform.forward) < fieldOfViewRange * 0.5f)
        {

            if (Physics.Raycast(transform.position, rayDirection, out hit))
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

    bool IsClosest(GameObject current, GameObject prev)
    {
        return (Vector3.Distance(current.transform.position, transform.position) < Vector3.Distance(prev.transform.position, transform.position));
    }

}
