using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private float _Xspeed = 3f;
    [SerializeField] 
    private float _Yspeed = 3f;

    private float _limitMinX = -80f;
    private float _limitMaxX = 50f;
    private float _eulerAngleX;
    private float _eulerAngleY;

    public void Rotate(float _mouseX, float _mouseY)
    {
        _eulerAngleX -= _mouseY * _Xspeed; 
        _eulerAngleY += _mouseX * _Yspeed;

        _eulerAngleX = ClampAngle(_eulerAngleX, _limitMinX, _limitMaxX);
        transform.rotation = Quaternion.Euler(_eulerAngleX, _eulerAngleY, 0);
    }

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