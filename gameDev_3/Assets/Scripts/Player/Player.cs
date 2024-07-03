using UnityEngine;

public class Player : MonoBehaviour
{
    private CameraRotate _cameraRotate;

    public float speed;
    public float jumpForce;
    private float _hAxis;
    private float _vAxis;
    private bool _isJump = false;
    
    Vector3 _moveVec;
    Rigidbody _rb;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();    
        _cameraRotate = GetComponent<CameraRotate>();   
    }

    private void Update()
    {
        Jump();
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {   
        _hAxis = Input.GetAxisRaw("Horizontal"); 
        _vAxis = Input.GetAxisRaw("Vertical");

        _moveVec = (transform.forward * _vAxis + transform.right * _hAxis).normalized;

        Vector3 _movePosition = transform.position + _moveVec * speed * Time.deltaTime;
        _rb.MovePosition(_movePosition);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !_isJump)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJump = false;
        }
    }

    private void Rotate()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X");
        float _mouseY = Input.GetAxisRaw("Mouse Y");

        _cameraRotate.Rotate(_mouseX, _mouseY);
    }
}
