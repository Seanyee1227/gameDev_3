using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssualtRifle : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] 
    private AudioClip audioClipTakeOutWeapon; // ���� ����
    private AudioSource audioSource; // ���� ���

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting;

    private float _lastAttackTime = 0f;

    private AudioSource _audioSource;
    private PlayerAnimation _anim;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<PlayerAnimation>();
    }

    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }


}
