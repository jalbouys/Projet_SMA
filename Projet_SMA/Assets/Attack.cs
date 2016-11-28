using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

    public float nextAttackTime;
    public float attackTime = 1;//time it takes to do 1 attack

    // Use this for initialization
    void Start () {
        nextAttackTime = Time.time;

    }
	
	// Update is called once per frame
	void Update () {
        GetInfo getInfo = GetComponent<GetInfo>();
        var distance = Vector3.Distance(getInfo.target.transform.position, transform.position);
        if (distance < 2)//if close enough, attack
        {
            if(Time.time > nextAttackTime)
                attack(getInfo.target);//whoop-ass
        }
    }

    void attack(GameObject target)
    {
        nextAttackTime = Time.time + attackTime;//time when the next attack can happen
        GetInfo getInfo = target.GetComponent<GetInfo>();
        getInfo.hp -= 10;//remove 10HP from victim
        getInfo.attacker = transform.gameObject;
        Debug.Log("Whack! ");
        Debug.Log(getInfo.hp);
        Debug.Log(" HP left\n");
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
