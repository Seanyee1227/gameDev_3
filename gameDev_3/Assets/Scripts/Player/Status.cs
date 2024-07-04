using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Walk, Run Speed")]
    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _runSpeed;

    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
}
