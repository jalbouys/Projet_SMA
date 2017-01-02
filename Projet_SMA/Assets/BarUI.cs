using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int cpt = 0;
        foreach (GameObject barbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        { cpt++; }
        GetComponent<Text>().text = "Barbarians : " + cpt;
    }
}
