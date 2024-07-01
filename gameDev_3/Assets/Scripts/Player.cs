using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float _hAxis;
    private float _vAxis;
    private bool _isJump = false;

    Vector3 _moveVec;
    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        _hAxis = Input.GetAxisRaw("Horizontal"); 
        _vAxis = Input.GetAxisRaw("Vertical");

        _moveVec = new Vector3(_hAxis, 0, _vAxis).normalized;

        transform.position += _moveVec * speed * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !_isJump)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJump = false;
        }
    }
}
