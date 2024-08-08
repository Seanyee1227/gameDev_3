using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private float _curHealth;
    [SerializeField]
    private float _maxHealth = 1800f;

    [Header("Die")]
    [SerializeField]
    private bool _isDie;

    private void Awake()
    {
        _curHealth = _maxHealth;
        _isDie = false;
    }

    public void TakeDamage(int _damage)
    {
        _curHealth -= _damage;

        if (_curHealth <= 0)
        {
            _isDie = true;

            Debug.Log("ав╬З╫ю╢о╢ы.");
            Destroy(gameObject);
        }
    }
}
