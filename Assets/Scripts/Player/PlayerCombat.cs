using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    private float _currentHealth;
    private bool _isDead;
    
    [Header("Attack Info")]
    [SerializeField] private float startTimeBtwAttack;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float damage;

    private PlayerInput _input;
    private float _timeBtwAttack;
    private bool _canAttack;

    public float Health => _currentHealth <= 0 ? 0 : _currentHealth;
    public bool CanAttack => _canAttack;
    public bool IsDead => _isDead;

    public event Action GetDamaged;
    public event Action OnDeath;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _input.Attack += Attack;
    }

    private void OnDisable()
    {
        _input.Attack -= Attack;
    }

    private void Start()
    {
        _timeBtwAttack = startTimeBtwAttack;
        _currentHealth = maxHealth;
        StartCoroutine(DeathRoutine());
    }
    
    private void Update()
    {
        if (_currentHealth <= 0) { _isDead = true; }
        
        if (_timeBtwAttack <= 0)
        {
            _canAttack = true;
        }
        else
        {
            _canAttack = false;
            _timeBtwAttack -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayer);
        foreach (var enemy in enemiesToDamage)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
        _timeBtwAttack = startTimeBtwAttack;
    }

    public void TakeDamage(float damageAmount)
    {
        if (_isDead) return;
        GetDamaged?.Invoke();
        ReduceHealth(damageAmount);
    }

    private void ReduceHealth(float damageAmount)
    {
        _currentHealth -= damageAmount;
    }

    private IEnumerator DeathRoutine()
    {
        while (!_isDead)
        {
            yield return null;
        }
        OnDeath?.Invoke();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position,attackRange);
    }
    
}
