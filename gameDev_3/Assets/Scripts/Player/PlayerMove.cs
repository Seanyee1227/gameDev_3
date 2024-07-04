using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    private Vector3 _moveForce;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _characterController.Move(_moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 _dir)
    {
        _dir = transform.rotation * new Vector3(_dir.x, 0, _dir.z);

        _moveForce = new Vector3(_dir.x * _moveSpeed, _moveForce.y, _dir.z * _moveSpeed);
    }
}
