using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum State { Idle, Patrol, Chase, }

public class EnemyFSM : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField]
    private Transform _points;
    [SerializeField]
    private float _waitPoint = 2f;
    private int _curPoint;
    private float _waitCounter;

    [Header("Components")]
    NavMeshAgent _nav;

    [Header("States")]
    [SerializeField]
    private State _curState;

    [Header("Chase")]
    [SerializeField]
    private float _chaseRange;

    [Header("Suspicious")]
    [SerializeField]
    private float _suspiciousTime;
    private float _lastSawPlayer;

    private GameObject _player;

    private void Start()
    {
        _nav = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");

        _waitCounter = _waitPoint;
        _lastSawPlayer = _suspiciousTime;
    }

    private void Update()
    {
        float _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        switch (_curState)
        {
            case State.Idle:

                if (_waitCounter > 0)
                {
                    _waitCounter -= Time.deltaTime;
                }
                else
                {
                    _curState = State.Patrol;
                    _nav.SetDestination(_points.GetChild(_curPoint).position);
                }

                if (_distanceToPlayer <= _chaseRange)
                {
                    _curState = State.Chase;
                }

                break;

            case State.Patrol:

                if (_nav.remainingDistance <= 0.2f)
                {
                    _curPoint++;
                    if (_curPoint >= _points.childCount)
                    {
                        _curPoint = 0;
                    }
                    _curState = State.Idle;
                    _waitCounter = _waitPoint;
                }

                if (_distanceToPlayer <= _chaseRange)
                {
                    _curState = State.Chase;
                }

                break;

            case State.Chase:

                _nav.SetDestination(_player.transform.position);
                if (_distanceToPlayer > _chaseRange)
                {
                    _nav.isStopped = true;
                    _nav.velocity = Vector3.zero;
                    _lastSawPlayer -= Time.deltaTime;

                    if (_lastSawPlayer <= 0)
                    {
                        _curState = State.Idle;
                        _lastSawPlayer = _suspiciousTime;
                        _nav.isStopped = false;
                    }
                }

                break;
        }
    }
}
