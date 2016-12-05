using UnityEngine;
using System.Collections;

public class Defend : MonoBehaviour {

    float nextAttackTime;
    float attackTime = 1;//time it takes to do 1 attack
    GuardMoveTo guardMoveTo;
    GetInfo getInfoGuard;
    // Use this for initialization
    void Start () {
        nextAttackTime = Time.time;
        guardMoveTo = GetComponent<GuardMoveTo>();
        getInfoGuard = GetComponent<GetInfo>();
    }
	
	// Update is called once per frame
	void Update () {

        foreach(GameObject barbarian in getInfoGuard.barbarians)
        if (guardMoveTo.CanSeeEntity(barbarian))//if sees someone, stop patrolling
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
