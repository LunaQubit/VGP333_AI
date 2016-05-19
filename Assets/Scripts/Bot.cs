using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bot : MonoBehaviour
{
    #region Enums
    public enum AIState
    {
        Patrol,
        Idle,
        Chase,
        Die,

        SIZE
    }

    public enum PathFollowStyle
    {
        Loop,
        Once,
        PingPong,
    }
    #endregion

    #region fields
    [Header("Navigation")]
    public Transform[] patrolPath;
    [Header("Locomotion")]
    public float speed;
    [Header("Logic")]
    public PathFollowStyle pathFollowStyle;

    private int currentPatrolPoint = 0;
    [SerializeField]
    private float approachThreshold = 0.5f;
    private int pathDirection = 1;
    #endregion

    #region STate machine fields
    EventHandler[] StateCallbacks = new EventHandler[(int)AIState.SIZE];
    private AIState currentState = AIState.Idle;
    private float chaseDistance = 10f;
    #endregion


    public void Start()
    {
        StateCallbacks[(int)AIState.Idle] += OnIdle;
        StateCallbacks[(int)AIState.Patrol] += OnMoveToTarget;
        StateCallbacks[(int)AIState.Patrol] += CheckForPlayer;
        StateCallbacks[(int)AIState.Chase] += ChasePlayer;

    }

    public void Update()
    {
        UpdateStateMachine();

    }

    public void UpdateStateMachine()
    {
        int cState = (int)currentState;
        if (StateCallbacks[cState] != null)
        {
            StateCallbacks[(int)currentState].Invoke(this, null);
        }
    }


    private void MoveToTarget(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        //Debug.DrawRay(transform.position, dir, Color.red);

        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
    }

    #region State Callbacks
    private void OnIdle(object sender, EventArgs e)
    {
        Debug.LogFormat("I'm Idlin'");
        currentState = AIState.Patrol;
    }

    private void OnMoveToTarget(object sender, EventArgs e)
    {
        UpdateLocomotion();
    }

    private void CheckForPlayer(object sender, EventArgs e)
    {
        float distance = Vector3.Distance(EnemyManager.Instance.Player.transform.position, transform.position);
        if (distance < chaseDistance)
        {
            currentState = AIState.Chase;
        }
    }

    private void ChasePlayer(object sender, EventArgs e)
    {
        MoveToTarget(EnemyManager.Instance.Player.transform);
    }

    #endregion

    #region Private Methods
    public void UpdateLocomotion()
    {
        int index = currentPatrolPoint;
        if (pathFollowStyle == PathFollowStyle.Loop)
        {
            index = index % patrolPath.Length;
        }
        else if (pathFollowStyle == PathFollowStyle.Once)
        {
            if (index >= patrolPath.Length)
            {
                index = -1;
                return;
            }
        }
        else if (pathFollowStyle == PathFollowStyle.PingPong)
        {
            if (index >= patrolPath.Length)
            {
                index = patrolPath.Length - 1;
                pathDirection = -1;
            }
            else if (index < 0)
            {
                index = 0;
                pathDirection = 1;
            }
        }
        Transform target = patrolPath[index];
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < approachThreshold)
        {
            // Arrived
            OnArrived();
        }
        else
        {
            MoveToTarget(target);
        }

    }
    private void OnArrived()
    {
        currentPatrolPoint += pathDirection;
    }
    #endregion

    #region DebugDrawing

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if( currentState == AIState.Chase)
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
    #endregion

}
