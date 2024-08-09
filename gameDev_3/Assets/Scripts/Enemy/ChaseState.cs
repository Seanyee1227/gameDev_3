using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    private int _damage = 25;
    private float _attackDelay = 2f;

    NavMeshAgent _agent;
    Transform _target;

    EnemyHealth _enemyHealth;
    PlayerHealth _playerHealth;
    [SerializeField]
    BoxCollider _attackRange;

    [Header("Sound")]
    [SerializeField]
    private AudioClip _chaseSound;
    private AudioSource _audioSource;
    [SerializeField]
    [Range(0f, 1f)] // 0 is silent, 1 is full volume
    private float _volume = 0.5f; // Set this to the desired volume level

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyHealth = animator.GetComponent<EnemyHealth>();
        _agent = animator.GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _audioSource = animator.GetComponent<AudioSource>();
        _agent.speed = 4.5f;

        if (_audioSource != null && _chaseSound != null)
        {
            _audioSource.clip = _chaseSound;
            _audioSource.loop = true;
            _audioSource.spatialBlend = 1.0f; // 3D sound
            _audioSource.minDistance = 5f;    // Minimum distance to hear the sound
            _audioSource.maxDistance = 15f;   // Maximum distance to hear the sound
            _audioSource.rolloffMode = AudioRolloffMode.Logarithmic; // Rolloff mode
            _audioSource.volume = _volume; // Set the volume
            _audioSource.Play();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_target.position);
        float _distance = Vector3.Distance(_target.position, animator.transform.position);

        if (_distance > 15f && !_enemyHealth.isDamaged)
        {
            animator.SetBool("Chasing", false);
            if (_audioSource != null)
            {
                _audioSource.Stop();
            }
        }
        if (_distance < 2.5f)
        {
            animator.SetBool("Attacking", true);
            if (_audioSource != null)
            {
                _audioSource.Stop();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(animator.transform.position);

        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }
}
