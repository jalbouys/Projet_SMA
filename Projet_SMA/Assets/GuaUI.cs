using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuaUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int cpt = 0;
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        { cpt++; }
        GetComponent<Text>().text = "Guards : " + cpt;
    }
}
