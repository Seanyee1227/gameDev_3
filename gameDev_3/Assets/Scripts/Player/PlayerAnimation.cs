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

    public bool AimModes
    {
        set => _anim.SetBool("isAimMode", value);
        get => _anim.GetBool("isAimMode");
    }

    public void Play(string _stateName, int _layer, float _normalizedTime)
    {
        _anim.Play(_stateName, _layer, _normalizedTime);
    }

    public void OnReload()
    {
        _anim.SetTrigger("onReload");
    }


    public bool CurrentAnimation(string _name)
    {
        return _anim.GetCurrentAnimatorStateInfo(0).IsName(_name);
    }
}
