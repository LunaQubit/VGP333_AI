using UnityEngine;
using System.Collections;

public class Bot_AI : MonoBehaviour
{
    NavMeshAgent navAgent;

    // Use this for initialization
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }


    public void NavigateToTransform(Transform target)
    {
        navAgent.SetDestination(target.position);
    }
}
