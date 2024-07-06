using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _slowMoveSpeed;

    private Vector3 _moveForce;

    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _gravity;

    private CharacterController _characterController;

    public float MoveSpeed
    {
        set => _moveSpeed = Mathf.Max(0, value);
        get => _moveSpeed;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!_characterController.isGrounded)
        {
            _moveForce.y += _gravity * Time.deltaTime;
        }

        _characterController.Move(_moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 _dir)
    {
        _dir = transform.rotation * new Vector3(_dir.x, 0, _dir.z);

        _moveForce = new Vector3(_dir.x * _moveSpeed, _moveForce.y, _dir.z * _moveSpeed);
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            _moveForce.y = _jumpForce;
        }
    }
}
