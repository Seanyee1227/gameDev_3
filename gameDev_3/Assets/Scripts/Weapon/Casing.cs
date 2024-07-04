using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float _deactiveTime = 5.0f;
    [SerializeField]
    private float _casingSpin = 1.0f;
    [SerializeField]
    private AudioClip[] _audioClips;

    private Rigidbody _rb;
    private AudioSource _audioSource;
    private MemoryPool _memoryPool;

    public void Setup(MemoryPool _pool, Vector3 _dir)
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _memoryPool = _pool;

        // ź���� �̵� �ӷ�, ȸ�� �ӷ�
        _rb.velocity = new Vector3(_dir.x, 1.0f, _dir.z);
        _rb.angularVelocity = new Vector3(Random.Range(-_casingSpin, _casingSpin),
                                          Random.Range(-_casingSpin, _casingSpin),
                                          Random.Range(-_casingSpin, _casingSpin));

        StartCoroutine("DeactiveAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ź�� ���� ����
        int _index = Random.Range(0, _audioClips.Length);
        _audioSource.clip = _audioClips[_index];
        _audioSource.Play();
    }

    private IEnumerator DeactiveAfterTime()
    {
        yield return new WaitForSeconds(_deactiveTime);

        _memoryPool.DeactivePoolItem(this.gameObject);
    }
}
