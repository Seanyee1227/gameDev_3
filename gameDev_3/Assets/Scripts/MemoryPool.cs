using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private int _maxCount; // 현재 리스트에 들어있는 오브젝트 개수
    private int _activeCount; // 현재 게임에 활성화되어 있는 오브젝트 개수

    private GameObject _poolObject; // 오브젝트 풀링에서 관리하는 게임 오브젝트 프리팹
    private List<PoolItem> _poolList; // 관리되는 모든 오브젝트를 저장하는 리스트

    public int MaxCount => _maxCount; // 외부에서 현재 리스트에 등록되어 있는 오브젝트 개수 확인 프로퍼티
    public int ActiveCount => _activeCount; // 외부에서 현재 활성화 되어 있는 오브젝트 개수 확인 프로퍼티

    public MemoryPool(GameObject poolObject)
    {
        _maxCount = 0;
        _activeCount = 0;
        this._poolObject = poolObject;

        _poolList = new List<PoolItem>();

        InstantiateObjects();
    }

    // _increaseCount 단위로 오브젝트 생성한다.
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

    // 씬, 게임이 종료될 때 모든 게임 오브젝트 삭제한다.
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

    // 현재 비활성화 상태의 오브젝트 중 하나를 활성화하여 사용한다.
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

    // 현재 활성화 상태인 오브젝트 _removeObject를 비활성화 상태로 만든다.
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

    // 현재 비활성하 상태인 모든 게임 오브젝트를 보이지 않게 한다.
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
