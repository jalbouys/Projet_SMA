using UnityEngine;
using System.Collections;

public class Defend : MonoBehaviour {

    public float nextAttackTime;
    public float attackTime = 1;//time it takes to do 1 attack
    GuardMoveTo guardMoveTo;
    // Use this for initialization
    void Start () {
        nextAttackTime = Time.time;
        guardMoveTo = GetComponent<GuardMoveTo>();
    }
	
	// Update is called once per frame
	void Update () {

        if (guardMoveTo.CanSeeEntity())//if sees someone, stop patrolling
        {
            guardMoveTo.attacked = true;
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
    }
}
