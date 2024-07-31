using UnityEngine;

public enum ImpactType { Normal = 0, Enemy, }

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    GameObject[] _impactPrefab; // �ǰ� ����Ʈ
    MemoryPool[] _memoryPool; // ����Ʈ �޸�Ǯ

    private void Awake()
    {
        // �ǰ� ����Ʈ�� ���� ������ �������� �޸�Ǯ ����
        _memoryPool = new MemoryPool[_impactPrefab.Length];
        for (int i = 0; i < _impactPrefab.Length; ++i)
        {
            _memoryPool[i] = new MemoryPool(_impactPrefab[i]);  
        }
    }

    public void SpawnImpact(RaycastHit _hit)
    {
        // �ε��� ������Ʈ�� tag�� ���� �ٸ��� ó��
        if (_hit.transform.CompareTag("Normal"))
        {
            Debug.Log("Normal");
            OnSpawnImpact(ImpactType.Normal, _hit.point, Quaternion.LookRotation(_hit.normal));
        }
        else if (_hit.transform.CompareTag("Enemy")) 
        {
            Debug.Log("Enemy");
            OnSpawnImpact(ImpactType.Enemy, _hit.point, Quaternion.LookRotation(_hit.normal));
        }

    }

    public void OnSpawnImpact(ImpactType _type, Vector3 _pos, Quaternion _rotation)
    {
        GameObject _item = _memoryPool[(int)_type].ActivePoolItem();
        _item.transform.position = _pos;
        _item.transform.rotation = _rotation;
        _item.GetComponent<Impact>().SetUp(_memoryPool[(int)_type]);
    }
}
