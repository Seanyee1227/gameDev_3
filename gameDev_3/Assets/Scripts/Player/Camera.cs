using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Player;

    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public float aimOffsetX;
    public float aimOffsetY;
    public float aimOffsetZ;
    public float DelayTime;

    public float MouseSens;
    private float _MouseX;
    private float _MouseY;

    private bool _isAiming = false;

    Vector3 _playerPos;
    Vector3 _aimPos;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        Follow();
        Rotate();
        Aiming();

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Rotate()
    {
        _MouseX += Input.GetAxisRaw("Mouse X") * MouseSens * Time.deltaTime;
        _MouseY -= Input.GetAxisRaw("Mouse Y") * MouseSens * Time.deltaTime;

        _MouseY = Mathf.Clamp(_MouseY, -90f, 90f); // 상하 최대값, 최소값
        _MouseX = Mathf.Clamp(_MouseX, -90f, 90f); // 상하 최대값, 최소값
        transform.localRotation = Quaternion.Euler(_MouseY, _MouseX, 0f);
    }

    private void Follow()
    {
        if (_isAiming)
        {
            _aimPos = new Vector3(Player.transform.position.x + aimOffsetX, Player.transform.position.y + aimOffsetY, Player.transform.position.z + aimOffsetZ);
            transform.position = Vector3.Lerp(transform.position, _aimPos, Time.deltaTime * DelayTime);
        }
        else
        {
            _playerPos = new Vector3(Player.transform.position.x + offsetX, Player.transform.position.y + offsetY, Player.transform.position.z + offsetZ);
        }
        transform.position = Vector3.Lerp(transform.position, _playerPos, Time.deltaTime * DelayTime);
    }

    private void Aiming()
    {
        if (Input.GetMouseButton(1))
        {
            _isAiming = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isAiming = false;
        }
    }
}
