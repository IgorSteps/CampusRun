using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent Agent;

    void Start()
    {
        Agent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Agent.destination = Player.position;
    }
}
