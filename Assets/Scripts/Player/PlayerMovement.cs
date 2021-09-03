using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Values")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    
    private PlayerInput _input;
    private PlayerCombat _combat;
    private Rigidbody2D _rb2D;
    private bool _isGrounded;
    private bool _hasGem;

    public Rigidbody2D RB2D => _rb2D;
    public bool IsGrounded => _isGrounded;

    public event Action OnGemObtain;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _combat = GetComponent<PlayerCombat>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _input.Jump += Jump;
    }

    private void OnDisable()
    {
        _input.Jump -= Jump;
    }
    
    private void Update()
    {
        transform.rotation = _input.IsFlipped ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
    }

    private void FixedUpdate()
    {
        _rb2D.velocity = new Vector2(_input.Direction * movementSpeed, _rb2D.velocity.y);
    }

    private void Jump()
    {
        _isGrounded = false;
        _rb2D.velocity = Vector2.up * jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            _isGrounded = true;
        }

        if (other.collider.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
            _hasGem = true;
            OnGemObtain?.Invoke();
        }

        if (other.collider.CompareTag("Exit"))
        {
            GameManager.Instance.GameEnd(isPlayerWin:true);
        }
        
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
