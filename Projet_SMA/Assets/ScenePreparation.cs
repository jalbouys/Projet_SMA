using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using System.Collections.Generic;

/*Script called to prepare the scene, i.e instanciate agents*/
public class ScenePreparation : MonoBehaviour {


	// Use this for initialization
    public GameObject barbarian;
    public List<GameObject> barbarians;
    public GameObject villager;
    public List<GameObject> villagers;
    public GameObject guard;
    public List<GameObject> guards;

    void Start () {
        Debug.LogWarning("Villagers: " + PlayerPrefs.GetInt("Villagers"));
        Debug.LogWarning("Guards: " + PlayerPrefs.GetInt("Guards"));
        Debug.LogWarning("Barbarians: " + PlayerPrefs.GetInt("Barbarians"));

        Random rnd = new Random();

        /*instanciation of the Barbarians*/
        for(int i = 0; i < PlayerPrefs.GetInt("Barbarians");i++)
        {
            Vector3 position = new Vector3(Random.Range(-70.0f,-50.0f),0.0f,Random.Range(50.0f,70.0f));
            GameObject tmp = (GameObject)Instantiate(barbarian, position, barbarian.transform.rotation);
            tmp.name = "Barbarian " + i;
            barbarians.Add(tmp);
            tmp.SetActive(true);
            
        }

        /*instanciation of the Villagers*/
        for (int i = 0; i < PlayerPrefs.GetInt("Villagers"); i++)
        {
            Vector3 position = new Vector3(Random.Range(-28.0f, -14.0f), 0.0f, Random.Range(-50.0f, -38.0f));
            GameObject tmp = (GameObject)Instantiate(villager, position, villager.transform.rotation);
            tmp.name = "Villager " + i;
            villagers.Add(tmp);
            tmp.SetActive(true);

        }

        /*instanciation of the Guards*/
        for (int i = 0; i < PlayerPrefs.GetInt("Guards"); i++)
        {
            Vector3 position = new Vector3(Random.Range(-24.0f, -12.0f), 0.0f, 17.0f);
            GameObject tmp = (GameObject)Instantiate(guard, position, guard.transform.rotation);
            tmp.name = "Guard " + i;
            guards.Add(tmp);
            tmp.SetActive(true);

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
	        
}
