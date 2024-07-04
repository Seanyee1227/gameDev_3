using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _casingPrefab; // 탄피 오브젝트
    private MemoryPool _memoryPool; // 탄피 메모리풀

    private void Awake()
    {
        _memoryPool = new MemoryPool(_casingPrefab);
    }

    public void SpawnCasing(Vector3 _pos, Vector3 _dir)
    {
        GameObject _item = _memoryPool.ActivePoolItem();
        _item.transform.position = _pos;
        _item.transform.rotation = Random.rotation;
        _item.GetComponent<Casing>().Setup(_memoryPool, _dir);
    }
}
