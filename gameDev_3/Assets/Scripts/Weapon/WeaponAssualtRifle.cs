using System.Collections;
using UnityEngine;

public class WeaponAssualtRifle : MonoBehaviour
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject _flashEffect; // ��� ����Ʈ

    [Header("Audio Clips")]
    [SerializeField] 
    private AudioClip audioClipTakeOutWeapon; // ���� ����
    [SerializeField]
    private AudioClip audioClipFire; // ��� ����
    

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting _weaponSetting; // ���� ����

    private float _lastAttackTime = 0f; // ������ �߻� �ð� Ȯ��

    private AudioSource _audioSource;
    private PlayerAnimation _anim;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponentInParent<PlayerAnimation>();
    }

    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);
        _flashEffect.SetActive(false);
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

            _anim.Play("Fire", -1, 0);

            StartCoroutine("FlashEffect");

            PlaySound(audioClipFire);
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
