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
    private bool _isReload = false; // 재장전 확인

    private AudioSource _audioSource;
    private PlayerAnimation _anim;
    private CasingMemoryPool _memoryPool;

    // 외부에서 필요한 정보를 열람하기 위해 정의한 프로퍼티
    public WeaponName WeaponName => _weaponSetting.weaponName;
    public int CurrentMagazine => _weaponSetting.currentMagazin;
    public int MaxMagazine => _weaponSetting.maxMagazin;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponentInParent<PlayerAnimation>();
        _memoryPool = GetComponent<CasingMemoryPool>();

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