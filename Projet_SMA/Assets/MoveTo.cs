using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

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
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Target != null)
        {
            MoveToTarget(Target);
        }
	}

    void MoveToTarget(GameObject target)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target.transform.position;

        transform.LookAt(target.transform.position);
    }
}
