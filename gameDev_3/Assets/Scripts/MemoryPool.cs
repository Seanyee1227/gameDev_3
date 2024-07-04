using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
   // �޸� Ǯ�� �����Ǵ� ������Ʈ�� ����
   public class PoolItem
    {
        public bool isActive;
        public GameObject gameObject;
    }

    private int _increaseCount; // �߰� �����Ǵ� ������Ʈ�� ����
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
