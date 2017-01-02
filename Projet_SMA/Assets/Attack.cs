using UnityEngine;
using System.Collections;

/*This is the script dealing with attack behaviors for Barbarians*/
public class Attack : MonoBehaviour {

    public float nextAttackTime;
    public float attackTime = 1; //time it takes to do 1 attack
    public bool attackingAlone = true;
    BarbarianMoveTo moveToBar;
    private GameObject target = null;
    public bool helping = false;
    public bool attacking = false;
    public GameObject Target //current target of the agent
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    // Use this for initialization
    void Start () {
        nextAttackTime = Time.time;
        moveToBar = GetComponent<BarbarianMoveTo>();
        moveToBar.moveToVillage(); //At first, Barbarian will move towards the village

    }
	
	// Update is called once per frame
	void Update () {

        if (target != null)
        {
            var distance = Vector3.Distance(target.transform.position, transform.position);
            if (helping)
                moveToBar.DebugMsg = "Coming to help!"; // debug message to let us know if he is attacking to help or just for fun

            if (distance < 2) //if close enough, attack
            {
                attacking = true;
                moveToBar.DebugMsg = "Attacking: " + target.tag;
                if (Time.time > nextAttackTime)
                    attack(target);//whoop-ass
            }
            else
                attacking = false;
        }
        else if(target == null || attacking == false)
        {
            GetComponent<Animation>().Stop("Lumbering");//stop attacking
            GetComponent<Animation>().Play("Walk");//start walking again
            attacking = false;
            helping = false; //not helping nor attacking anymore
        }
    }

    /*attack function, can be either for a villager target or a guard*/
    void attack(GameObject target)
    {
        
        GetComponent<Animation>().Play("Lumbering");//start attacking
        nextAttackTime = Time.time + attackTime;//time when the next attack can happen
        GetInfoVillager targetInfoV;
        GetInfoGuard targetInfoG;
        if (target.tag == "Villager") /*case of villager, call GetInfoVillager script */
        {
            targetInfoV = target.GetComponent<GetInfoVillager>();
            targetInfoV.agressors.Add(gameObject);
            attackingAlone = (targetInfoV.agressors.Count == 1);
            targetInfoV.hp -= 10;//remove 10HP from victim
            targetInfoV.attacker = transform.gameObject;
        }
        else /*case of guard, call GetInfoGuard script, same action as above */
        {
            targetInfoG = target.GetComponent<GetInfoGuard>();
            targetInfoG.agressors.Add(gameObject);
            attackingAlone = (targetInfoG.agressors.Count == 1);
            targetInfoG.hp -= 10;//remove 10HP from victim
            targetInfoG.attacker = transform.gameObject;
        }
    }

}
