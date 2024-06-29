using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float _hAxis;
    float _vAxis;

    Vector3 _moveVec;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _hAxis = Input.GetAxisRaw("Horizontal");
        _vAxis = Input.GetAxisRaw("Vertical");

        _moveVec = new Vector3(_hAxis, 0, _vAxis).normalized;

        transform.position += _moveVec * speed * Time.deltaTime;
    }
}
