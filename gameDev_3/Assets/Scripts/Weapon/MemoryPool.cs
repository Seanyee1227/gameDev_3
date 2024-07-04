using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private int _maxCount; // ���� ����Ʈ�� ����ִ� ������Ʈ ����
    private int _activeCount; // ���� ���ӿ� Ȱ��ȭ�Ǿ� �ִ� ������Ʈ ����

    private GameObject _poolObject; // ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ ������
    private List<PoolItem> _poolList; // �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ

    public int MaxCount => _maxCount; // �ܺο��� ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ���� Ȯ�� ������Ƽ
    public int ActiveCount => _activeCount; // �ܺο��� ���� Ȱ��ȭ �Ǿ� �ִ� ������Ʈ ���� Ȯ�� ������Ƽ

    public MemoryPool(GameObject poolObject)
    {
        _maxCount = 0;
        _activeCount = 0;
        this._poolObject = poolObject;

        _poolList = new List<PoolItem>();

        InstantiateObjects();
    }

    // _increaseCount ������ ������Ʈ �����Ѵ�.
    public void InstantiateObjects()
    {
        _maxCount += _increaseCount;

        for (int i = 0; i < _increaseCount; ++i)
        {
            PoolItem _poolItem = new PoolItem();

            _poolItem.isActive = false;
            _poolItem.gameObject = GameObject.Instantiate(_poolObject);
            _poolItem.gameObject.SetActive(false);

            _poolList.Add(_poolItem);
        }
    }

    // ��, ������ ����� �� ��� ���� ������Ʈ �����Ѵ�.
    public void DestroyObjects()
    {
        if (_poolList == null)
        {
            return;
        }

        int _count = _poolList.Count;
        for (int i = 0; i < _count; ++i)
        {
            GameObject.Destroy(_poolList[i].gameObject);
        }
        _poolList.Clear();
    }

    // ���� ��Ȱ��ȭ ������ ������Ʈ �� �ϳ��� Ȱ��ȭ�Ͽ� ����Ѵ�.
    public GameObject ActivePoolItem()
    {
        if (_poolList == null)
        {
            return null;
        }

        if (_maxCount == _activeCount)
        {
            InstantiateObjects();
        }

        int _count = _poolList.Count;
        for (int i = 0; i < _count; ++i)
        {
            PoolItem _poolItem = _poolList[i];

            if (_poolItem.isActive == false)
            {
                _poolItem.isActive = true;
                _poolItem.gameObject.SetActive(true);

                return _poolItem.gameObject;
            }
        }
        return null;
    }

    // ���� Ȱ��ȭ ������ ������Ʈ _removeObject�� ��Ȱ��ȭ ���·� �����.
    public void DeactivePoolItem(GameObject _removeObject)
    {
        if (_poolList == null || _removeObject == null)
        {
            return;
        }

        int _count = _poolList.Count;
        for (int i = 0; i < _count; ++i)
        {
            PoolItem _poolItem = _poolList[i];

            if (_poolItem.gameObject == _removeObject)
            {
                _activeCount--;

                _poolItem.isActive = false;
                _poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    // ���� ��Ȱ���� ������ ��� ���� ������Ʈ�� ������ �ʰ� �Ѵ�.
    public void DeactiveAllPoolIteams()
    {
        if (_poolList == null)
        {
            return;
        }

        int _count = _poolList.Count;
        for (int i =0; i< _count; ++i)
        {
            PoolItem _poolItem = _poolList[i];

            if (_poolItem.gameObject != null && _poolItem.isActive == true)
            {
                _poolItem.isActive = false;
                _poolItem.gameObject.SetActive(false);
            }
        }
        _activeCount = 0;
    }
}
