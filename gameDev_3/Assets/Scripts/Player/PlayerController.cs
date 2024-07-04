using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CameraRotate _camreraRotate;
    private PlayerMove _playerMove;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _camreraRotate = GetComponent<CameraRotate>();
        _playerMove = GetComponent<PlayerMove>();   
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
    }

    private void UpdateRotate()
    {
        float _mouseX = Input.GetAxis("Mouse X");
        float _mouseY = Input.GetAxis("Mouse Y");

        _camreraRotate.Rotate(_mouseX, _mouseY);
    }

    private void UpdateMove()
    {
        float _hAxis = Input.GetAxisRaw("Horizontal");
        float _vAxis = Input.GetAxisRaw("Vertical");

        _playerMove.MoveTo(new Vector3(_hAxis, 0, _vAxis));
    }
}
