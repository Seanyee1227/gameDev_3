using System.Collections;
using UnityEngine;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class WeaponAssualtRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

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

    private AudioSource _audioSource;
    private PlayerAnimation _anim;
    private CasingMemoryPool _memoryPool;

    public WeaponName weaponName => _weaponSetting.weaponName; 

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponentInParent<PlayerAnimation>();
        _memoryPool = GetComponent<CasingMemoryPool>();

        // ź���� �ִ�� �ʱ�ȭ
        _weaponSetting.currentAmmo = _weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);
        _flashEffect.SetActive(false);

        onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);
    }

    public void StartWeaponAction(int _type = 0)
    {
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

    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}