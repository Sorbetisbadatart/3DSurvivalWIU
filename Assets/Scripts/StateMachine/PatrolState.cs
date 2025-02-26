using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PatrolState : EnemyState
{
    StateMachine _stateMachine;

    private EnemyController _enemyController;

    private int _currentWaypointIndex = 0;
    private float _waypointThreshold = 0.5f;

    public PatrolState(EnemyController enemy)
    {
        _enemyController = enemy;
    }
    public void OnEnter()
    {
        Debug.Log("ENTER STATE PATROL");

        _enemyController._animator.CrossFadeInFixedTime("Enemy1_Walk", 0.5f);

        _enemyController._agent.speed = _enemyController.patrolSpeed;
        _enemyController._agent.isStopped = false;

        //random start waypoint
        _currentWaypointIndex = Random.Range(0, _enemyController._waypoints.Length);
    }

    public void OnUpdate()
    {
        Debug.Log("ONUPDATE STATE PATROL");

        if (_enemyController._waypoints == null ||
            _enemyController._waypoints.Length == 0)
            return;

        //way point
        Transform targetWaypoint = _enemyController._waypoints[_currentWaypointIndex];

        _enemyController._agent.SetDestination(targetWaypoint.position);

        Vector3 toTarget = targetWaypoint.position - _enemyController.transform.position;
        toTarget.y = 0;
        float distance = toTarget.magnitude;

        if (distance <= _enemyController._agent.stoppingDistance)
        {
            Debug.Log("reached");

            //set next waypoint randomly
            _currentWaypointIndex = Random.Range(0, _enemyController._waypoints.Length);
            //_currentWaypointIndex = (_currentWaypointIndex + 1) % _enemyController._waypoints.Length;
        }

        //if player is in detection range, change to chase state
        float distanceToPlayer = Vector3.Distance(_enemyController.transform.position, _enemyController._player.position);
        if (distanceToPlayer <= _enemyController.detectionRange && distanceToPlayer > _enemyController.attackRange)
        {
            _enemyController._stateMachine.ChangeState(new ChaseState(_enemyController));
        }
    }

    public void OnExit()
    {
        Debug.Log("EXIT STATE PATROL");

        _enemyController._agent.isStopped = true;
    }
}
