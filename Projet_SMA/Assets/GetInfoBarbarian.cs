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
    public int hp = 100;
    public GameObject target;//or target pos?
    public GameObject attacker;
    public int fieldOfViewRange = 60;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        { barbarians.Add(barbarian); }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        { guards.Add(guard); }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            villagers.Add(villager);
            target = villager;
        }
        GetComponent<MoveTo>().Target = target;
        GetComponent<Attack>().Target = target;
    }

    // Update is called once per frame
    void Update()
    {
        barbarians.Clear();
        guards.Clear();
        villagers.Clear();
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        { barbarians.Add(barbarian); }
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        { guards.Add(guard); }
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            if(CanSeeTarget(villager))
                villagers.Add(villager);
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
                    Debug.Log("Tag : " + hit.transform.tag);
                    if (hit.transform.tag == target.tag)
                    {
                        Debug.Log(target.tag);
                        return true;
                    }
                    else
                    {
                        Debug.Log("See Nothing");
                        return false;
                    }
                }

            }
            return false;
    }

}
