using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public float MoveSpeed
    {
        set => _anim.SetFloat("MoveSpeed", value);
        get => _anim.GetFloat("MoveSpeed");
    }

    public void Play(string _stateName, int _layer, float _normalizedTime)
    {
        _anim.Play(_stateName, _layer, _normalizedTime);
    }
}
