using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _input;
    private PlayerMovement _movement;
    private PlayerCombat _combat;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        _combat = GetComponent<PlayerCombat>();
    }

    private void OnEnable()
    {
        _input.Jump += Jump;
        _input.Attack += Attack;
        _combat.GetDamaged += GetDamaged;
        _combat.OnDeath += DeathAnimation;
    }

    private void OnDisable()
    {
        _input.Jump -= Jump;
        _input.Attack -= Attack;
        _combat.GetDamaged -= GetDamaged;
        _combat.OnDeath -= DeathAnimation;
    }

    private void Update()
    {
        _animator.SetInteger("AnimState", _input.Direction != 0 ? 1 : 0);
        _animator.SetFloat("AirSpeedY", _movement.RB2D.velocity.y);
        _animator.SetBool("Grounded", _movement.IsGrounded);
    }

    private void Jump()
    {
        AudioManager.Instance.Play("Jump");
        _animator.SetTrigger("Jump");
    }

    private void Attack()
    {
        AudioManager.Instance.Play(_movement.IsGrounded ? "atkGround" : "atkAir");
        _animator.SetTrigger(_movement.IsGrounded ? "Attack3" : "Attack1");
    }

    private void GetDamaged()
    {
        AudioManager.Instance.Play("Hurt");
        _animator.SetTrigger("Hurt");
    }

    private void DeathAnimation()
    {
        StartCoroutine(DeathRoutine());
    }
    private IEnumerator DeathRoutine()
    {
        AudioManager.Instance.Play("Death");
        _animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.273f);
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        GameManager.Instance.GameEnd(isPlayerWin:false);
    }
    
}
