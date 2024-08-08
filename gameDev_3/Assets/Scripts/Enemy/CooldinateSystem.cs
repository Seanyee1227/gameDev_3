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
        // 모델의 로컬 축을 확인하고, 필요에 따라 회전 조정
        modelTransform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}
