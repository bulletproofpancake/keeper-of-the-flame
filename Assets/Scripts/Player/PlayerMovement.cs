using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _input;
    private bool _isGrounded;
    private Rigidbody2D _rb2D;

    public bool IsGrounded => _isGrounded;
    public Rigidbody2D RB2D => _rb2D;
    
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.rotation = _input.IsFlipped ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            _isGrounded = true;
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
