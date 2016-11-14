using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.


public class Modification : MonoBehaviour {

	// Use this for initialization
    public Slider mainSlider;
    public Text text;

	void Start () {
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void ValueChangeCheck()
    {
        text.text = "Barbarians : " + mainSlider.value;
    }

}
