using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    StateMachine _stateMachine;

    private EnemyController _enemyController;

    public AttackState(EnemyController enemy)
    {
        _enemyController = enemy;
    }

    public void OnEnter()
    {
        Debug.Log("ENTER STATE ATTACK");

        _enemyController._animator.CrossFadeInFixedTime("Enemy1_Idle", 0.5f);
    }

    public void OnUpdate()
    {
        Debug.Log("ONUPDATE STATE ATTACK");

        //if player is out of attack range but in the detection range, change to chase state
        float distanceToPlayer = Vector3.Distance(_enemyController.transform.position, _enemyController._player.position);
        if (distanceToPlayer <= _enemyController.detectionRange && distanceToPlayer > _enemyController.attackRange)
        {
            _enemyController._stateMachine.ChangeState(new ChaseState(_enemyController));
        }
    }

    public void OnExit()
    {
        Debug.Log("EXIT STATE ATTACK");
    }
}
