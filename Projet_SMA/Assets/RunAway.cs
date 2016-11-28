using UnityEngine;
using System.Collections;

public class RunAway : MonoBehaviour {

    public int currentHp;
	
    // Use this for initialization
	void Start () {
        currentHp = GetComponent<GetInfo>().hp;
    }
	
	// Update is called once per frame
	void Update () {

        GetInfo getInfo = GetComponent<GetInfo>();
        if (currentHp != getInfo.hp)
        {
            currentHp = getInfo.hp;
            
            Debug.Log(getInfo.attacker);
            runAwayFrom(getInfo.attacker);
        }

        if(getInfo.attacker)
        {
            Vector3 direction = ((getInfo.attacker.transform.position) - (transform.position)).normalized;
            transform.rotation = Quaternion.LookRotation(transform.position - getInfo.attacker.transform.position);
            transform.position += -direction * 1 * Time.deltaTime;
        }

	}

    public void runAwayFrom(GameObject attacker)
    {
        Vector3 direction = ((attacker.transform.position) - (transform.position)).normalized;
        transform.rotation = Quaternion.LookRotation(transform.position - attacker.transform.position);
        transform.position += -direction * 50 * Time.deltaTime;
    }
}
