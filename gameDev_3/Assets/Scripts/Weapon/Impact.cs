using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private MemoryPool _memoryPool;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void SetUp(MemoryPool _pool)
    {
        _memoryPool = _pool;
    }

    private void Update()
    {
        if (_particleSystem.isPlaying == false)
        {
            _memoryPool.DeactivePoolItem(gameObject);
        }
    }
}
