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
    private GameObject _flashEffect; // 사격 이펙트

    [Header("Spawn Points")]
    [SerializeField]
    private Transform _casingSpawnPoint; // 탄피 생성 위치

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

    private float _lastAttackTime = 0f; // 마지막 발사 시간 확인

    private AudioSource _audioSource;
    private PlayerAnimation _anim;
    private CasingMemoryPool _memoryPool;

    public WeaponName weaponName => _weaponSetting.weaponName; 

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponentInParent<PlayerAnimation>();
        _memoryPool = GetComponent<CasingMemoryPool>();

        // 탄약을 최대로 초기화
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

            // 총기 애니매이션 재생
            _anim.Play("Fire", -1, 0);

            // 사격 이펙트 재생
            StartCoroutine("FlashEffect");

            // 사격 사운드 재생
            PlaySound(_audioClipFire);

            // 탄피 생성
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