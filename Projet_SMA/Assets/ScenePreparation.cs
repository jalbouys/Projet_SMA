using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using System.Collections.Generic;

public class ScenePreparation : MonoBehaviour {


	// Use this for initialization
    public GameObject barbarian;
    public List<GameObject> barbarians;

	void Start () {
        Debug.LogWarning("Villagers: " + PlayerPrefs.GetInt("Villagers"));
        Debug.LogWarning("Guards: " + PlayerPrefs.GetInt("Guards"));
        Debug.LogWarning("Barbarians: " + PlayerPrefs.GetInt("Barbarians"));

        Random rnd = new Random();

        for(int i = 0; i < PlayerPrefs.GetInt("Barbarians");i++)
        {
            Vector3 position = new Vector3(Random.Range(-50.0f,-20.0f),-8.537f,Random.Range(50.0f,70.0f));
            GameObject tmp = (GameObject)Instantiate(barbarian, position, barbarian.transform.rotation);
            barbarians.Add(tmp);
            tmp.SetActive(true);
            
        }
	}

    // Update is called once per frame
    void Update()
    {
    }
	        
}
