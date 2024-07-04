using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private float _Xspeed = 3f; // x축 속도
    [SerializeField] 
    private float _Yspeed = 3f; // y축 속도

    private float _limitMinX = -80f; // 최소 x값
    private float _limitMaxX = 50f; // 최대 x값
    private float _eulerAngleX;
    private float _eulerAngleY;

    // 카메라 회전
    public void Rotate(float _mouseX, float _mouseY)
    {
        _eulerAngleX -= _mouseY * _Xspeed; 
        _eulerAngleY += _mouseX * _Yspeed;

        _eulerAngleX = ClampAngle(_eulerAngleX, _limitMinX, _limitMaxX);
        transform.rotation = Quaternion.Euler(_eulerAngleX, _eulerAngleY, 0);
    }

    // 카메라 각도 제한
    private float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < -360)
        {
            _angle += 360;
        }
        if (_angle > 360)
        {
            _angle -= 360;
        }

        return Mathf.Clamp(_angle, _min, _max);
    }
}