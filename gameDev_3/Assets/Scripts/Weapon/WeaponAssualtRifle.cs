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
    private GameObject _flashEffect; // ��� ����Ʈ

    [Header("Spawn Points")]
    [SerializeField]
    private Transform _casingSpawnPoint; // ź�� ���� ��ġ
    [SerializeField]
    private Transform _bulletSpawnPoint; // �Ѿ� ���� ��ġ

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _audioClipTakeOutWeapon; // ���� ����
    [SerializeField]
    private AudioClip _audioClipFire; // ��� ����
    [SerializeField]
    private AudioClip _audioClipReload; // ������ ����


    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting _weaponSetting; // ���� ����

    [Header("Aim UI")]
    [SerializeField]
    private Image _aimImage;

    private float _lastAttackTime = 0f; // ������ �߻� �ð� Ȯ��
    private bool _isReload = false; // ������ Ȯ��
    private bool _isAttack = false; // ���� ����
    private bool _isModeChange = false; // ��� ��ȯ
    private float _defaultFov = 60f; // �⺻ ī�޶�
    private float _AimFov = 30f; // Aim ����� ī�޶�

    private AudioSource _audioSource;
    private PlayerAnimation _anim;
    private CasingMemoryPool _memoryPool;
    private ImpactMemoryPool _impactMemoryPool;
    private Camera _mainCamera;

    // �ܺο��� �ʿ��� ������ �����ϱ� ���� ������ ������Ƽ
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

        // ź���� �ִ�� �ʱ�ȭ
        _weaponSetting.currentAmmo = _weaponSetting.maxAmmo;

        // źâ ���� �ִ�� �ʱ�ȭ
        _weaponSetting.currentMagazin = _weaponSetting.maxMagazin;
    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);
        _flashEffect.SetActive(false);

        // ���Ⱑ Ȱ��ȭ �� �� ź �� ����
        onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);

        // ���Ⱑ Ȱ��ȭ �� �� źâ �� ����
        onMagazinEvent.Invoke(_weaponSetting.currentMagazin);

        ResetVarialbes();
    }

    public void StartWeaponAction(int _type = 0)
    {
        // ������ �߿� ���� �Ұ�
        if (_isReload == true) return;

        // ��� ��ȯ �߿� ���� �Ұ�
        if (_isModeChange == true) return;
       
        // ���
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
            // ���� �� ��� ��ȯ �Ұ�
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

                // ���� źâ ���� ���� ��Ű��, UI �����̵�
                _weaponSetting.currentMagazin--;
                onMagazinEvent.Invoke(_weaponSetting.currentMagazin);

                // �������� ���� ź���� �ִ�� �����ϰ�, UI ������Ʈ
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
            // �޸� �� ���� X
            if (_anim.MoveSpeed > 0.5f)
            {
                return;
            }

            // ���� �ֱⰡ �Ǿ�� ������ �� �ֵ��� ���� �ð� ����
            _lastAttackTime = Time.time;

            // ź���� ������ ��� �Ұ�
            if (_weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            // ź�� ����, UI ������Ʈ
            _weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);

            // �ѱ� �ִϸ��̼� ��� ( Fire or AimFire)
            //_anim.Play("Fire", -1, 0);
            string _animation = _anim.AimModes == true ? "AimFire" : "Fire";
            _anim.Play(_animation, -1, 0);

            // ��� ����Ʈ ��� (default ����� ����)
            if (_anim.AimModes == false) StartCoroutine("FlashEffect");

            // ��� ���� ���
            PlaySound(_audioClipFire);

            // ź�� ����
            _memoryPool.SpawnCasing(_casingSpawnPoint.position, transform.right);

            // ������ �߻��� ���ϴ� ��ġ ����
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

        // ȭ���� �߾� ��ǥ
        _ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        //���� ��Ÿ� �ȿ� ������ _target�� ������ �΋H�� ��ġ
        if (Physics.Raycast(_ray, out _hit, _weaponSetting.attackDistane))
        {
            _target = _hit.point;
        }
        // ���� ��Ÿ� �ȿ� ������ _target�� �ִ� ��Ÿ�
        else
        {
            _target = _ray.origin + _ray.direction * _weaponSetting.attackDistane;
        }
        Debug.DrawRay(_ray.origin, _ray.direction * _weaponSetting.attackDistane, Color.red);

        // _target�� ��ǥ�� �����ϰ�, �ѱ��� ���������� �Ͽ� Ratcast  ����
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

            // ��忡 ���� �þ� ����
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