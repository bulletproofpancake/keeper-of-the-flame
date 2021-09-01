using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _input;
    private PlayerMovement _movement;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _input.Jump += Jump;
        _input.Attack += Attack;
    }

    private void OnDisable()
    {
        _input.Jump -= Jump;
        _input.Attack -= Attack;
    }

    private void Update()
    {
        _animator.SetInteger("AnimState", _input.Direction != 0 ? 1 : 0);
        _animator.SetFloat("AirSpeedY", _movement.RB2D.velocity.y);
        _animator.SetBool("Grounded", _movement.IsGrounded);
    }

    private void Jump()
    {
        _animator.SetTrigger("Jump");
    }

    private void Attack()
    {
        _animator.SetTrigger(_movement.IsGrounded ? "Attack3" : "Attack1");
    }
    
}
