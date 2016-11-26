using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Communication : MonoBehaviour {

    public List<GameObject> otherBarbarians;
    public GameObject personnage;
    public int hp = 100;

    void MessageReceived(string message)
    {
        Debug.Log("MessageReceived\n");
        if (message == "jump")
        {
            transform.position.Set(transform.position.x, transform.position.y, 50.0F);
            Debug.Log("Jump\n");
        }
        else if (message == "move")
        {
            transform.position = Vector3.MoveTowards(transform.position, personnage.transform.position, Time.deltaTime * 5);
        }
    }
    // Use this for initialization
    void Start ()
    {
        foreach (GameObject otherBarbarian in GameObject.FindGameObjectsWithTag("Barbarian"))
        {
            otherBarbarians.Add(otherBarbarian);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach(GameObject barbarian in otherBarbarians)
        {
            var distance = Vector3.Distance(barbarian.transform.position, transform.position);
            if(distance < 10)//if other barbarian is less than 10m away...
            {
                Debug.Log("nearby\n");
                barbarian.SendMessage("MessageReceived", "move");
                //  transform.position = Vector3.MoveTowards(transform.position, barbarian.transform.position, Time.deltaTime * 5);
               // transform.position.Set(transform.position.x, transform.position.y, zheight);

            }
        }
    }
    
}
