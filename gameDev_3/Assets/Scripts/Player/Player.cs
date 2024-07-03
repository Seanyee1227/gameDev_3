using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private CameraRotate _cameraRotate;

    // 체력
    private float _maxHealth = 100;
    public float currentHealth;
   

    // 움직임
    public float speed;
    public float currentSpeed;
    public float runSpeed;
    public float jumpForce;
    private float _hAxis;
    private float _vAxis;
    private bool _isJump = false;
    private bool _isRun = false;
    
    Vector3 _moveVec;
    Rigidbody _rb;
    Animator _anim;

    // UI
    public Slider healthBar;

    private void Awake()
    {
        currentSpeed = speed;
        currentHealth = _maxHealth;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();    
        _cameraRotate = GetComponent<CameraRotate>();   
        _anim = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar.maxValue = _maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void Update()
    {
        Jump();
        Rotate();
        if (Input.GetKeyDown(KeyCode.G))
        {
            Dameged(10);
        }
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

        Vector3 _movePosition = transform.position + _moveVec * currentSpeed * Time.deltaTime;
        _rb.MovePosition(_movePosition);

        if (Input.GetKey(KeyCode.LeftShift) && !_isRun)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
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

    public void Dameged(float _damage)
    {
        if (currentHealth > 0)
        {
            Debug.Log(_damage);
            currentHealth -= _damage;
            if (healthBar != null )
            {
                healthBar.value = currentHealth;
            }
            if (currentHealth <= 0)
            {
                Debug.Log("Die");
                Die();
            }
        }
    }

    private void Die()
    {
        Time.timeScale = 0f;
    }
}
