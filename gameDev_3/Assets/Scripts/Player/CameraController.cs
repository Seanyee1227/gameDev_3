using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public float mouseSens = 100f;

    public float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
        float _mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= _mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Player.Rotate(Vector3.up * _mouseX);
    }
}
