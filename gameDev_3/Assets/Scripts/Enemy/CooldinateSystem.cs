using UnityEngine;

public class CooldinateSystem : MonoBehaviour
{
    [SerializeField]
    Transform _object;

    private void Update()
    {
        AlignModelToNavMeshAgent(_object);
    }

    void AlignModelToNavMeshAgent(Transform modelTransform)
    {
        // ���� ���� ���� Ȯ���ϰ�, �ʿ信 ���� ȸ�� ����
        modelTransform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}
