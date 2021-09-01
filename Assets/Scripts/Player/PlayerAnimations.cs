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

    private void Update()
    {
        _animator.SetInteger("AnimState", _input.Direction != 0 ? 1 : 0);
        _animator.SetFloat("AirSpeedY", _movement.RB2D.velocity.y);
        _animator.SetBool("Grounded", _movement.IsGrounded);
    }
}
