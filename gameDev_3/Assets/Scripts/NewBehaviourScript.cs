using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssualtRifle : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] 
    private AudioClip audioClipTakeOutWeapon; // 장착 사운드
    private AudioSource audioSource; // 사운드 재생

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
