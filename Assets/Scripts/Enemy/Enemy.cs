using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    
    private float _currentHealth;
    private Animator _animator;
    private BoxCollider2D _collider;
    private bool _isDead;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        _collider.enabled = true;
        StartCoroutine(DeathRoutine());
    }

    private void Update()
    {
        if (_currentHealth <= 0) { _isDead = true; }
    }

    public void TakeDamage(float damageAmount)
    {
        _animator.SetTrigger("Hurt");
        _currentHealth -= damageAmount; 
    }

    private IEnumerator DeathRoutine()
    {
        while (!_isDead)
        {
            yield return null;
        }
        _collider.enabled = false;
        _animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.25f);
        Die();
    }
    
    private void Die()
    {
        _animator.SetTrigger("Death");
        Destroy(gameObject,1f);
    }
}
