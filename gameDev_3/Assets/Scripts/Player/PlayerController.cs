using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCode")]
    [SerializeField]
    private KeyCode _keyCodeRun = KeyCode.LeftShift;

    private CameraRotate _camreraRotate;
    private PlayerMove _playerMove;
    private Status _status;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _camreraRotate = GetComponent<CameraRotate>();
        _playerMove = GetComponent<PlayerMove>();   
        _status = GetComponent<Status>();
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

        if (_vAxis != 0 || _hAxis != 0 )
        {
            bool _isRun = Input.GetKey(_keyCodeRun);
            _playerMove.MoveSpeed = _isRun == true ? _status.RunSpeed : _status.WalkSpeed;
        }
        _playerMove.MoveTo(new Vector3(_hAxis, 0, _vAxis));
    }
}
