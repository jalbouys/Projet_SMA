using UnityEngine;
using System.Collections;

public class BarbarianMoveTo : MonoBehaviour {

    private Vector3 previousTargetPosition;
    private Vector3 villageEntrance = new Vector3(-18, 0, 20);
    private RaycastHit villageHit = new RaycastHit();
    // Use this for initialization
    void Start()
    {
        villageHit.point = villageEntrance;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void moveToVillage()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = villageHit.point;
        transform.LookAt(villageHit.point);
    }
}
