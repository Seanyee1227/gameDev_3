using System.Collections;
using UnityEngine;

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

    private float _lastAttackTime = 0f; // ������ �߻� �ð� Ȯ��
    private bool _isReload = false; // ������ Ȯ��

    private AudioSource _audioSource;
    private PlayerAnimation _anim;
    private CasingMemoryPool _memoryPool;

    // �ܺο��� �ʿ��� ������ �����ϱ� ���� ������ ������Ƽ
    public WeaponName WeaponName => _weaponSetting.weaponName;
    public int CurrentMagazine => _weaponSetting.currentMagazin;
    public int MaxMagazine => _weaponSetting.maxMagazin;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponentInParent<PlayerAnimation>();
        _memoryPool = GetComponent<CasingMemoryPool>();

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
        //onMagazinEvent.Invoke(_weaponSetting.currentMagazin);  
    }

    public void StartWeaponAction(int _type = 0)
    {
        if (_isReload == true)
        {
            return;
        }

        if (_type == 0)
        {
            if (_weaponSetting.isAutoAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int _type = 0)
    {
        if (_type == 0)
        {
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

            // �ѱ� �ִϸ��̼� ���
            _anim.Play("Fire", -1, 0);

            // ��� ����Ʈ ���
            StartCoroutine("FlashEffect");

            // ��� ���� ���
            PlaySound(_audioClipFire);

            // ź�� ����
            _memoryPool.SpawnCasing(_casingSpawnPoint.position, transform.right);
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

    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}