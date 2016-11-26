using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetInfo : MonoBehaviour {

    public List<GameObject> barbarians;
    public List<GameObject> guards;
    public List<GameObject> villagers;
    public int hp = 100;

    // Use this for initialization
    void Start ()
    {

        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        {barbarians.Add(barbarian);}
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        {guards.Add(guard);}
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {villagers.Add(villager);}
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
