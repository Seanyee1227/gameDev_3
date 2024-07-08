using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class MagazinEvent : UnityEngine.Events.UnityEvent<int> { }

public class WeaponAssualtRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazinEvent onMagazinEvent = new MagazinEvent();

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject _flashEffect; // 사격 이펙트

    [Header("Spawn Points")]
    [SerializeField]
    private Transform _casingSpawnPoint; // 탄피 생성 위치
    [SerializeField]
    private Transform _bulletSpawnPoint; // 총알 생성 위치

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _audioClipTakeOutWeapon; // 장착 사운드
    [SerializeField]
    private AudioClip _audioClipFire; // 사격 사운드
    [SerializeField]
    private AudioClip _audioClipReload; // 재장전 사운드


    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting _weaponSetting; // 무기 설정

    [Header("Aim UI")]
    [SerializeField]
    private Image _aimImage;

    private float _lastAttackTime = 0f; // 마지막 발사 시간 확인
    private bool _isReload = false; // 재장전 확인
    private bool _isAttack = false; // 공격 여부
    private bool _isModeChange = false; // 모드 변환
    private float _defaultFov = 60f; // 기본 카메라
    private float _AimFov = 30f; // Aim 모드의 카메라

    private AudioSource _audioSource;
    private PlayerAnimation _anim;
    private CasingMemoryPool _memoryPool;
    private ImpactMemoryPool _impactMemoryPool;
    private Camera _mainCamera;

    // 외부에서 필요한 정보를 열람하기 위해 정의한 프로퍼티
    public WeaponName WeaponName => _weaponSetting.weaponName;
    public int CurrentMagazine => _weaponSetting.currentMagazin;
    public int MaxMagazine => _weaponSetting.maxMagazin;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponentInParent<PlayerAnimation>();
        _memoryPool = GetComponent<CasingMemoryPool>();
        _impactMemoryPool = GetComponent<ImpactMemoryPool>();
        _mainCamera = Camera.main;

        // 탄약을 최대로 초기화
        _weaponSetting.currentAmmo = _weaponSetting.maxAmmo;

        // 탄창 수를 최대로 초기화
        _weaponSetting.currentMagazin = _weaponSetting.maxMagazin;
    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);
        _flashEffect.SetActive(false);

        // 무기가 활성화 될 때 탄 수 갱신
        onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);

        // 무기가 활성화 될 때 탄창 수 갱신
        onMagazinEvent.Invoke(_weaponSetting.currentMagazin);

        ResetVarialbes();
    }

    public void StartWeaponAction(int _type = 0)
    {
        // 재장전 중에 공격 불가
        if (_isReload == true) return;

        // 모드 전환 중에 공격 불가
        if (_isModeChange == true) return;
       
        // 사격
        if (_type == 0)
        {
            if (_weaponSetting.isAutoAttack == true)
            {
                _isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            else
            {
                OnAttack();
            }
        }
        else
        {
            // 공격 시 모드 전환 불가
            if (_isAttack == true) return;

            StartCoroutine("ModeChange");
        }
    }

    public void StopWeaponAction(int _type = 0)
    {
        if (_type == 0)
        {
            _isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();
            yield return null;
        }
    }

    private IEnumerator OnReload()
    {
        _isReload = true;

        _anim.OnReload();
        PlaySound(_audioClipReload);

        while (true)
        {
            if (_audioSource.isPlaying == false && _anim.CurrentAnimation("Movement"))
            {
                _isReload = false;

                // 현재 탄창 수를 감소 시키고, UI 업데이드
                _weaponSetting.currentMagazin--;
                onMagazinEvent.Invoke(_weaponSetting.currentMagazin);

                // 재장전시 현재 탄수를 최대로 변경하고, UI 업데이트
                _weaponSetting.currentAmmo = _weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);

                yield break;
            }
            yield return null;
        }
    }

    public void OnAttack()
    {
        if (Time.time - _lastAttackTime > _weaponSetting.attackRate)
        {
            // 달릴 때 공격 X
            if (_anim.MoveSpeed > 0.5f)
            {
                return;
            }

            // 공격 주기가 되어야 공격할 수 있도록 지금 시간 저장
            _lastAttackTime = Time.time;

            // 탄약이 없으면 사격 불가
            if (_weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            // 탄약 감소, UI 업데이트
            _weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);

            // 총기 애니매이션 재생 ( Fire or AimFire)
            //_anim.Play("Fire", -1, 0);
            string _animation = _anim.AimModes == true ? "AimFire" : "Fire";
            _anim.Play(_animation, -1, 0);

            // 사격 이펙트 재생 (default 모드일 때만)
            if (_anim.AimModes == false) StartCoroutine("FlashEffect");

            // 사격 사운드 재생
            PlaySound(_audioClipFire);

            // 탄피 생성
            _memoryPool.SpawnCasing(_casingSpawnPoint.position, transform.right);

            // 광선을 발사해 원하는 위치 공격
            Ray();
        }
    }

    private IEnumerator FlashEffect()
    {
        _flashEffect.SetActive(true);
        yield return new WaitForSeconds(_weaponSetting.attackRate * 0.3f);
        _flashEffect.SetActive(false);
    }

    public void StartReload()
    {
        if (_isReload == true || _weaponSetting.currentMagazin <= 0)
        {
            return;
        }

        StopWeaponAction();

        StartCoroutine("OnReload");
    }

    private void Ray()
    {
        Ray _ray;
        RaycastHit _hit;
        Vector3 _target = Vector3.zero;

        // 화면의 중앙 좌표
        _ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        //공격 사거리 안에 있으면 _target이 광선이 부딫힌 위치
        if (Physics.Raycast(_ray, out _hit, _weaponSetting.attackDistane))
        {
            _target = _hit.point;
        }
        // 공격 사거리 안에 없으면 _target은 최대 사거리
        else
        {
            _target = _ray.origin + _ray.direction * _weaponSetting.attackDistane;
        }
        Debug.DrawRay(_ray.origin, _ray.direction * _weaponSetting.attackDistane, Color.red);

        // _target을 목표로 설정하고, 총구를 시작점으로 하여 Ratcast  연산
        Vector3 _attackDir = (_target - _bulletSpawnPoint.position).normalized;

        if (Physics.Raycast(_bulletSpawnPoint.position, _attackDir, out _hit, _weaponSetting.attackDistane))
        {
            _impactMemoryPool.SpawnImpact(_hit);
        }
        Debug.DrawRay(_bulletSpawnPoint.position, _attackDir * _weaponSetting.attackDistane, Color.blue);
    }

    private IEnumerator ModeChange()
    {
        float _current = 0f;
        float _percent = 0f;
        float _time = 0.35f;

        _anim.AimModes = !_anim.AimModes;
        _aimImage.enabled = !_aimImage.enabled;

        float _start = _mainCamera.fieldOfView;
        float _end = _anim.AimModes == true ? _AimFov : _defaultFov;

        _isModeChange = true;

        while (_percent < 1)
        {
            _current += Time.deltaTime;
            _percent = _current / _time;

            // 모드에 따라 시야 변경
            _mainCamera.fieldOfView = Mathf.Lerp(_start, _end, _percent);

            yield return null;
        }

        _isModeChange = false;
    }

    private void ResetVarialbes()
    {
        _isReload = false;
        _isAttack = false;
        _isModeChange = false;
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}