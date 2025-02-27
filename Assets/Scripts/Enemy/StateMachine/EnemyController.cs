
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour

{
    [Header("--------------------------------------------------")]
    [Header("--State Settings--")]
    //public State currentState;
    public float detectionRange = 10f;
    public float attackRange = 3f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("--------------------------------------------------")]
    [Header("--Waypoints--")]
    public Transform[] _waypoints;

    [Header("--------------------------------------------------")]
    [Header("--HealthBar--")]
    public HealthBar healthBar;

    [Header("--------------------------------------------------")]
    [Header("--Nav--")]
    //find player
    public Transform _player;
    public NavMeshAgent _agent;

    [Header("--------------------------------------------------")]
    [Header("--Information--")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] public Animator _animator;

    [Header("--------------------------------------------------")]
    [Header("--Drop--")]
    private DropSystem _dropSystem;

    //FSM
    public StateMachine _stateMachine = new StateMachine();


    private void Awake()
    {
        _player = GameObject.Find("Player").transform;

        //drop
        _dropSystem = GetComponent<DropSystem>();
    }

    private void Start()
    {

        //FSM
        _stateMachine.Initialize(new PatrolState(this));

        //health
        _currentHealth = _maxHealth;

        //health bar
        healthBar.SetMaxHealth(_maxHealth);

        //animation
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        //FSM
        _stateMachine.Update();

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }


        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    _animator.CrossFadeInFixedTime("Enemy1_Run", 0.5f);
        //}
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    _animator.CrossFadeInFixedTime("Enemy1_Attack1_Left", 0.5f);
        //}
        //if (Input.GetKeyUp(KeyCode.E))
        //{
        //    _animator.CrossFadeInFixedTime("Enemy1_Attack2_Right", 0.5f);
        //}
        //if (Input.GetKeyUp(KeyCode.R))
        //{
        //    _animator.CrossFadeInFixedTime("Enemy1_Attack3_Tail", 0.5f);
        //}
        //if (Input.GetKeyUp(KeyCode.T))
        //{
        //    _animator.CrossFadeInFixedTime("Enemy1_Walk", 0.5f);
        //}
        //if (Input.GetKeyUp(KeyCode.Y))
        //{
        //    _animator.CrossFadeInFixedTime("Enemy1_Attack4_Roar", 0.5f);
        //}
        //if (Input.GetKeyUp(KeyCode.U))
        //{
        //    _animator.CrossFadeInFixedTime("Enemy1_Attack5_", 0.5f);
        //}
        //if (Input.GetKeyUp(KeyCode.I))
        //{
        //    _animator.CrossFadeInFixedTime("Enemy1_Idle", 0.5f);
        //}
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _dropSystem.SpawnDropItem();
        Destroy(gameObject);

       
    }
}
