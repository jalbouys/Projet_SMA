using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

    public float nextAttackTime;
    public float attackTime = 1;//time it takes to do 1 attack
    BarbarianMoveTo moveToBar;
    MoveTo move;
    private GameObject target = null;
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
            Debug.Log(distance);
            if (distance < 2)//if close enough, attack
            {
                if (Time.time > nextAttackTime)
                    attack(target);//whoop-ass
            }
        }
    }

    void attack(GameObject target)
    {
        GetComponent<Animation>().Play("Lumbering");//start attacking
        nextAttackTime = Time.time + attackTime;//time when the next attack can happen
        GetInfo getInfo = target.GetComponent<GetInfo>();
        getInfo.hp -= 10;//remove 10HP from victim
        getInfo.attacker = transform.gameObject;
        Debug.Log("Whack! ");
        Debug.Log(getInfo.hp);
        Debug.Log(" HP left\n");
    }
}
