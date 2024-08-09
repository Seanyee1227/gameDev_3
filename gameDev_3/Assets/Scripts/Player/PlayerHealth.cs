using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float curHealth;
    public float maxHealth = 120f;
    [SerializeField]
    private Slider _healthSlider;

    private void Awake()
    {
        curHealth = maxHealth;

        if (_healthSlider != null)
        {
            _healthSlider.maxValue = maxHealth; 
            _healthSlider.value = curHealth;
        }
    }

    public void TakeDamaged(int _damage)
    {
        if (curHealth > 0)
        {
            Debug.Log("공격받음!");
            curHealth -= _damage;

            if (_healthSlider != null)
            {
                _healthSlider.value = curHealth;
            }
        }
        else
        {
            Debug.Log("죽음!");
            Time.timeScale = 0f;
        }
    }
}
