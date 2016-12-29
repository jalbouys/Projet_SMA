using UnityEngine;
using System.Collections;


public class Defend : MonoBehaviour
{

    public float nextAttackTime;
    public float attackTime = 1;//time it takes to do 1 attack
    public bool helping = false;
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
    void Start()
    {
        nextAttackTime = Time.time;


    }

    // Update is called once per frame
    void Update()
    {

        if (Target != null)
        {
            var distance = Vector3.Distance(target.transform.position, transform.position);
            //Debug.Log(distance);
            if (distance < 2)//if close enough, attack
            {
                if (Time.time > nextAttackTime)
                    attack(target);//whoop-ass
            }
        }
        else if (Target == null)
        {
            //GetComponent<Animation>().Stop("Jump");//stop attacking
        }
    }

    void attack(GameObject target)
    {

        GetComponent<Animator>().Play("Jump");//start attacking
        nextAttackTime = Time.time + attackTime;//time when the next attack can happen
        GetInfoBarbarian targetInfo;
        targetInfo = target.GetComponent<GetInfoBarbarian>();
        targetInfo.agressors.Add(gameObject);
        targetInfo.hp -= 10;//remove 10HP from victim
        targetInfo.attacker = transform.gameObject;
        //Debug.Log("Whack! ");
        //Debug.Log(targetInfo.hp);
        //Debug.Log(" HP left\n");
    }

}
