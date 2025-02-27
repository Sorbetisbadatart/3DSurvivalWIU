using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState
{
    StateMachine _stateMachine;

    private EnemyController _enemyController;

    public ChaseState(EnemyController enemy)
    {
        _enemyController = enemy;
    }

    public void OnEnter()
    {
       // Debug.Log("ENTER STATE CHASE");

        //_enemyController._animator.CrossFadeInFixedTime("Enemy1_Walk", 0.5f);
        _enemyController._animator.CrossFadeInFixedTime("Enemy1_Run", 0.5f);

        _enemyController._agent.speed = _enemyController.patrolSpeed;
        _enemyController._agent.isStopped = false;
    }

    public void OnUpdate()
    {
       // Debug.Log("ONUPDATE STATE CHASE");

        _enemyController._agent.isStopped = false;
        _enemyController._agent.speed = _enemyController.chaseSpeed;
        _enemyController._agent.SetDestination(_enemyController._player.position);

        //if player is out of detection range, change to patrol state
        float distanceToPlayer = Vector3.Distance(_enemyController.transform.position, _enemyController._player.position);
        if (distanceToPlayer > _enemyController.detectionRange)
        {
            _enemyController._stateMachine.ChangeState(new PatrolState(_enemyController));
        }
        else if (distanceToPlayer <= _enemyController.attackRange)//If player is in the attack range, change to attack state
        {
            _enemyController._stateMachine.ChangeState(new AttackState(_enemyController));
        }
    }

    public void OnExit()
    {
       // Debug.Log("EXIT STATE CHASE");

        _enemyController._agent.isStopped = true;
    }
}
