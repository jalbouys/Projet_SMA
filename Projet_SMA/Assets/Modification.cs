using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.


public class Modification : MonoBehaviour {

	// Use this for initialization
    public Slider mainSlider;
    public Text text;
    public enum typeSlider {Villagers, Guards, Barbarians};
    public int sliderModifie;
	void Start () {
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        PlayerPrefs.SetInt("Villagers", 5);
        PlayerPrefs.SetInt("Guards", 5);
        PlayerPrefs.SetInt("Barbarians", 5);

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void ValueChangeCheck()
    {
        if (sliderModifie == (int)typeSlider.Villagers)
        {
            text.text = "Villagers : " + mainSlider.value;
            PlayerPrefs.SetInt("Villagers", (int)mainSlider.value);
        }
        else if (sliderModifie == (int)typeSlider.Guards)
        {
            text.text = "Guards : " + mainSlider.value;
            PlayerPrefs.SetInt("Guards", (int)mainSlider.value);
        }
        else if (sliderModifie == (int)typeSlider.Barbarians)
        {
            text.text = "Barbarians : " + mainSlider.value;
            PlayerPrefs.SetInt("Barbarians", (int)mainSlider.value);
        }

        
    }

}
