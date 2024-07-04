using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssualtRifle : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] 
    private AudioClip audioClipTakeOutWeapon; // 장착 사운드
    private AudioSource audioSource; // 사운드 재생

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
