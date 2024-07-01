using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Player;

    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public float DelayTime;

    Vector3 _playerPos;

    private void Update()
    {
        _playerPos = new Vector3(Player.transform.position.x + offsetX, Player.transform.position.y + offsetY, Player.transform.position.z + offsetZ);

        transform.position = Vector3.Lerp(transform.position, _playerPos, Time.deltaTime * DelayTime);
    }
}
