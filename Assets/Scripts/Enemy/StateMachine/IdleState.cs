using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    StateMachine _stateMachine;

    //private Enemies _enemy;
    private EnemyController _enemyController;

    public IdleState(EnemyController enemy)
    {
        //this._enemy = enemy;
        _enemyController = enemy;

        //this._stateMachine = new StateMachine();
    }

    public void OnEnter()
    {
        Debug.Log("ENTER STATE IDLE");
        //_enemy._animator.Play("Enemy1_Idle");
        _enemyController._animator.CrossFadeInFixedTime("Enemy1_Idle", 0.5f);
    }


    public void OnUpdate()
    {
        Debug.Log("ONUPDATE STATE IDLE");

        //if player is near, change to chase state
        float distanceToPlayer = Vector3.Distance(_enemyController.transform.position, _enemyController._player.position);
        if (distanceToPlayer <= _enemyController.detectionRange && distanceToPlayer > _enemyController.attackRange)
        {
            _enemyController._stateMachine.ChangeState(new ChaseState(_enemyController));
        }
    }

    public void OnExit()
    {
        Debug.Log("EXIT STATE IDLE");
    }
}
