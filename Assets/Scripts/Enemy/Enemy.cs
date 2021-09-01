using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    
    [Header("Attacks")]
    [SerializeField] private Transform eyes;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float chaseRange;
    [SerializeField] private float stanceRange;
    [SerializeField] private float attackRange;
    
    private float _currentHealth;
    private Animator _animator;
    private BoxCollider2D _collider;
    private bool _isDead;
    private bool _isPlayerWithinChase;
    private bool _isPlayerWithinStance;
    private bool _isPlayerWithinAttack;
    private bool _canAttack;

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

        _isPlayerWithinChase = Physics2D.Raycast(eyes.position, Vector2.left, chaseRange, playerLayer);
        _isPlayerWithinStance = Physics2D.Raycast(eyes.position, Vector2.left, stanceRange, playerLayer);
        _isPlayerWithinAttack = Physics2D.Raycast(eyes.position, Vector2.left, attackRange, playerLayer);

        
        if (_isPlayerWithinChase)
        {
            // Move towards player
            _animator.SetInteger("AnimState",2);

            if (_isPlayerWithinStance)
            {
                _animator.SetInteger("AnimState", 1);
            }

            if (_isPlayerWithinAttack)
            {
                Attack();
            }
            
        }
        else
        {
            _animator.SetInteger("AnimState",0);
        }        
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(eyes.position, Vector2.left * chaseRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(eyes.position - Vector3.up * 0.25f, Vector2.left * stanceRange);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(eyes.position - Vector3.up * 0.5f, Vector2.left * attackRange);
    }
}
