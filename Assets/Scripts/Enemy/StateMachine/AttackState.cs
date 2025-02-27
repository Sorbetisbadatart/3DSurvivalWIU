
using UnityEngine;

public class AttackState : EnemyState
{
    private EnemyController _enemyController;

    public float _attackCooldown;
    private float _cooldownTimer = 0f;
    private bool _attackFinished = false;

    private string[] _attackAnimations = new string[]
    {
        "Enemy1_Attack1_Left",
        "Enemy1_Attack2_Right",
        "Enemy1_Attack3_Tail",
        "Enemy1_Attack4_Roar",
        "Enemy1_Attack5_"
    };

    public AttackState(EnemyController enemy)
    {
        _enemyController = enemy;
    }

    public void OnEnter()
    {
       // Debug.Log("ENTER STATE ATTACK");
        _cooldownTimer = 0f;
        _attackFinished = false;
        _attackCooldown = Random.Range(0.2f, 3.5f);

        PerformAttack();

        
    }

    public void OnUpdate()
    {

       // Debug.Log("ONUPDATE STATE ATTACK");


        AnimatorStateInfo stateInfo = _enemyController._animator.GetCurrentAnimatorStateInfo(0);    

        if (!_attackFinished && (stateInfo.IsName("Enemy1_Attack1_Left") ||
                                  stateInfo.IsName("Enemy1_Attack2_Right") ||
                                  stateInfo.IsName("Enemy1_Attack3_Tail") ||
                                  stateInfo.IsName("Enemy1_Attack4_Roar") ||
                                  stateInfo.IsName("Enemy1_Attack5_")))
        {
            if (stateInfo.IsName("Enemy1_Attack1_Left"))
            {

            }
            else if (stateInfo.IsName("Enemy1_Attack2_Right"))
            {

            }


            else { }
            if (stateInfo.normalizedTime >= 1.0f)
            {
                _attackFinished = true;

                _enemyController._animator.CrossFadeInFixedTime("Enemy1_Idle", 0.2f);
            }

            return;
        }

        if (_attackFinished)
        {
            _cooldownTimer += Time.deltaTime;
            FacePlayer();

            float distanceToPlayer = Vector3.Distance(_enemyController.transform.position, _enemyController._player.position);

            if (distanceToPlayer > _enemyController.attackRange)
            {
                _enemyController._stateMachine.ChangeState(new ChaseState(_enemyController));
            }

            if (_cooldownTimer >= _attackCooldown)
            {
                //float distanceToPlayer = Vector3.Distance(_enemyController.transform.position, _enemyController._player.position);

                //if (distanceToPlayer > _enemyController.attackRange)
                //{
                //    _enemyController._stateMachine.ChangeState(new ChaseState(_enemyController));
                //}
                //else
                //{
                //    _cooldownTimer = 0f;
                //    _attackFinished = false;
                //    _attackCooldown = Random.Range(0.2f, 3.5f);
                //    PerformAttack();
                //}

                _cooldownTimer = 0f;
                _attackFinished = false;
                _attackCooldown = Random.Range(0.2f, 3.5f);
                PerformAttack();
            }
        }
    }

    public void OnExit()
    {
      //  Debug.Log("EXIT STATE ATTACK");
    }

    private void PerformAttack()
    {
        int index = Random.Range(0, _attackAnimations.Length);
        _enemyController._animator.CrossFadeInFixedTime(_attackAnimations[index], 0.2f);
    }

    private void FacePlayer()
    {
        Vector3 direction = (_enemyController._player.position - _enemyController.transform.position);

        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float turnSpeed = 2f;
            _enemyController.transform.rotation = Quaternion.Slerp(_enemyController.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }
}
