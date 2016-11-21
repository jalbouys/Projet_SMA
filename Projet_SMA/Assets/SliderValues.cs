using UnityEngine;
using System.Collections;

public class SliderValues : MonoBehaviour {

    public int Villagers = 5;
    public int Guards = 5;
    public int Barbarians = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        // Do not destroy this game object:
        DontDestroyOnLoad(this);
    } 
}
