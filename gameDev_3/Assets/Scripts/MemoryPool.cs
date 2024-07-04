using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
   // 메모리 풀로 관리되는 오브젝트의 정보
   public class PoolItem
    {
        public bool isActive;
        public GameObject gameObject;
    }

    private int _increaseCount; // 추가 생성되는 오브젝트의 개수
    private int _maxCount;
    private int _activeCount;

    private GameObject _poolObject;
    private List<PoolItem> _poolList;

    public int MaxCount => _maxCount;
    public int ActiveCount => _activeCount;

    public MemoryPool(GameObject poolObject)
    {
        _maxCount = 0;
        _activeCount = 0;
        this._poolObject = poolObject;
    }
}
