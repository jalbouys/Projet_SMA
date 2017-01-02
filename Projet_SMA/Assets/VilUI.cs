using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VilUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int cpt = 0;
        foreach (GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        { cpt++; }
        GetComponent<Text>().text = "Vilagers : " + cpt;
    }
}
