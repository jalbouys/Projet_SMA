using UnityEngine;
using System.Collections;


/*This is the script dealing with defense behaviors for Guards*/
public class Defend : MonoBehaviour
{

    public float nextAttackTime;
    public float attackTime = 1; //time it takes to do 1 attack
    public bool helping = false; //Boolean to tell if guard is helping someone or not
    private GameObject target = null;
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
    void Start()
    {
        nextAttackTime = Time.time; //init the attack time


    }

    // Update is called once per frame
    void Update()
    {

        if (Target != null)
        {
            var distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance < 2)//if close enough, attack
            {
                if (Time.time > nextAttackTime)
                    attack(target);//whoop-ass
            }
        }
        else if (Target == null)
        {
            //no need to defend
        }
    }

    /*attack function, no need for more comment...*/
    void attack(GameObject target)
    {

        GetComponent<Animator>().Play("Jump");//start attacking
        nextAttackTime = Time.time + attackTime;//time when the next attack can happen
        GetInfoBarbarian targetInfo;
        targetInfo = target.GetComponent<GetInfoBarbarian>();
        targetInfo.agressors.Add(gameObject);
        targetInfo.hp -= 10;//remove 10HP from victim
        targetInfo.attacker = transform.gameObject;
    }

}
