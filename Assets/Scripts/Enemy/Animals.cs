
using UnityEngine;
using UnityEngine.AI;

public class Animals : MonoBehaviour
{
    [Header("--------------------------------------------------")]
    [Header("--wandering--")]
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private float _areaRadius = 10f;
    [SerializeField] private float _runAreaRadius = 50f;
    private float _timeToNextMove;
    private float _moveTimer = 0f;

    [Header("--------------------------------------------------")]
    [Header("--Nav--")]
    [SerializeField] private  NavMeshAgent _agent;

    [Header("--------------------------------------------------")]
    [Header("--Information--")]
    [SerializeField] private Animator _animator;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    [Header("--------------------------------------------------")]
    [Header("--HealthBar--")]
    public HealthBar healthBar;

    [Header("--------------------------------------------------")]
    [Header("--Drop--")]
    private DropSystem _dropSystem;

    private bool isDamaged = false;

    AudioSource _audioSource;
    [SerializeField] private AudioClip _a;
    private bool isAudioPlaying = false;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _currentHealth = _maxHealth;
        healthBar.SetMaxHealth(_maxHealth);

        _initialPosition = transform.position;
        
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        //drop
        _dropSystem = GetComponent<DropSystem>();
    }

    private void Update()
    {
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
        }

        //start timer
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _animator.SetBool("IsWalking", false);
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                _moveTimer += Time.deltaTime;

                //new position
                if (_moveTimer >= _timeToNextMove)
                {
                    _timeToNextMove = Random.Range(1.0f, 10.0f);
                    _moveTimer = 0f;
                    MoveToRandomPosition();
                }
            }
        }

        if (isDamaged == true)
        {
            if (!isAudioPlaying)
            {
                _audioSource.PlayOneShot(_a);
                isAudioPlaying = true;
            }
            RunAway();
        }

        //_initialPosition.y = transform.position.y;
        _initialPosition = transform.position;
    }

    private void MoveToRandomPosition()
    {
        _animator.SetBool("IsWalking", true);
        _agent.speed = _walkSpeed;
        Vector3 randomDirection = Random.insideUnitSphere * _areaRadius;
        randomDirection += _initialPosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, _areaRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }

    private void RunAway()
    {
        _areaRadius = _runAreaRadius;

        _animator.SetBool("IsRunning", true);
        _agent.speed = _runSpeed;

        _timeToNextMove = 0;

          }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(_initialPosition, _areaRadius);
    }

    public void TakeDamage(float damage)
    {
        isDamaged = true;

        AudioManager.Instance.PlaySFX("ChickenHurt");
        _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);

        if(_currentHealth <= 0)
        {
            Die();
        }
        
    }

    void Die()
    {
        

        _dropSystem.SpawnDropItem();
        Destroy(gameObject);

    }

}
