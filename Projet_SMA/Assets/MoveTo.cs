using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

    public GameObject personnage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, personnage.transform.position, Time.deltaTime * 5);
        transform.LookAt(personnage.transform.position);
	}
}
