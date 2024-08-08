using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float _idleSpeed;
    [SerializeField]
    private float _chaseSpeed;

    [Header("Health")]
    [SerializeField]
    private float _curHp;
    [SerializeField]
    private float _maxHp;


    public float IdleSpeed => _idleSpeed;
    public float ChaseSpeed => _chaseSpeed;
    public float CurHp => _curHp;
    public float MaxHp => _maxHp;
}
