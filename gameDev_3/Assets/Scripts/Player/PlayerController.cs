using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCode")]
    [SerializeField]
    private KeyCode _keyCodeRun = KeyCode.LeftShift; // 달리기 키
    [SerializeField]
    private KeyCode _keyCodeJump = KeyCode.Space; // 점프 키

    [Header("Audio Clip")]
    [SerializeField]
    private AudioClip _audioClipWalk; //걷기 사운드
    [SerializeField]
    private AudioClip _audioClipRun; // 달리기 사운드

    private CameraRotate _camreraRotate;
    private PlayerMove _playerMove;
    private Status _status;
    private PlayerAnimation _anim;
    private AudioSource _audioSource;
    private WeaponAssualtRifle _weapon;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _camreraRotate = GetComponent<CameraRotate>();
        _playerMove = GetComponent<PlayerMove>();   
        _status = GetComponent<Status>();
        _anim = GetComponent<PlayerAnimation>();    
        _audioSource = GetComponent<AudioSource>();
        _weapon = GetComponentInChildren<WeaponAssualtRifle>();
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
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

        // 이동 (걷기 or 달리기)
        if (_vAxis != 0 || _hAxis != 0 )
        {
            bool _isRun = Input.GetKey(_keyCodeRun);
            _playerMove.MoveSpeed = _isRun == true ? _status.RunSpeed : _status.WalkSpeed;
            _anim.MoveSpeed = _isRun == true ? 1 : 0.5f;
            _audioSource.clip = _isRun == true ? _audioClipRun : _audioClipWalk;

            if (_audioSource.isPlaying == false )
            {
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }
        else
        {
            _playerMove.MoveSpeed = 0;
            _anim.MoveSpeed = 0;

            if (_audioSource.isPlaying == true)
            {
                _audioSource.Stop();
            }
        }
        _playerMove.MoveTo(new Vector3(_hAxis, 0, _vAxis));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(_keyCodeJump))
        {
            _playerMove.Jump();
        }
    }

    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _weapon.StopWeaponAction();
        }
    }
}
