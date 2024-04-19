using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    private NavMeshAgent _agent;

    void Start()
    {
        _agent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _agent.destination = _player.position;
    }
}
