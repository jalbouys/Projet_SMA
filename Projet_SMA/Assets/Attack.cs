using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

    public float nextAttackTime;
    public float attackTime = 1;//time it takes to do 1 attack
    BarbarianMoveTo moveToBar;
    MoveTo move;
    private GameObject target = null;
    public bool helping = false;
    public bool attacking = false;
    public GameObject Target
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
        moveToBar.moveToVillage();
        move = GetComponent<MoveTo>();

    }
	
	// Update is called once per frame
	void Update () {

        if (target != null)
        {
            var distance = Vector3.Distance(target.transform.position, transform.position);
            if (helping)
                GetComponent<MoveTo>().DebugMsg = "Coming to help!";
            //Debug.Log(distance);
            if (distance < 2)//if close enough, attack
            {
                attacking = true;
                GetComponent<MoveTo>().DebugMsg = "Attacking: " + target.tag;
                if (Time.time > nextAttackTime)
                    attack(target);//whoop-ass
            }
            else
                attacking = false;
        }
        else if(target == null || attacking == false)
        {
            GetComponent<Animation>().Stop("Lumbering");//stop attacking
            GetComponent<Animation>().Play("Walk");//stop attacking
            attacking = false;
            helping = false;
        }
    }

    void attack(GameObject target)
    {
        
        GetComponent<Animation>().Play("Lumbering");//start attacking
        nextAttackTime = Time.time + attackTime;//time when the next attack can happen
        GetInfoVillager targetInfoV;
        GetInfoGuard targetInfoG;
        if (target.tag == "Villager")
        {
            targetInfoV = target.GetComponent<GetInfoVillager>();
            targetInfoV.agressors.Add(gameObject);
            targetInfoV.hp -= 10;//remove 10HP from victim
            targetInfoV.attacker = transform.gameObject;
            //Debug.Log("Whack! ");
            //Debug.Log(targetInfoV.hp);
            //Debug.Log(" HP left\n");
        }
        else
        {
            targetInfoG = target.GetComponent<GetInfoGuard>();
            targetInfoG.agressors.Add(gameObject);
            targetInfoG.hp -= 10;//remove 10HP from victim
            targetInfoG.attacker = transform.gameObject;
            //Debug.Log("Whack! ");
            //Debug.Log(targetInfoG.hp);
            //Debug.Log(" HP left\n");
        }
    }

}
