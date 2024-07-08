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

    public void SetUp(MemoryPool _pool)
    {
        _memoryPool = _pool;
    }

    private void Update()
    {
        // 파티클이 재생 중이 아니라면 삭제
        if (_particleSystem.isPlaying == false)
        {
            _memoryPool.DeactivePoolItem(gameObject);
        }
    }
}
