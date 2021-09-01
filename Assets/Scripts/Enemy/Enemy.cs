using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    
    [Header("Attack Detection")]
    [SerializeField] private Transform eyes;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float chaseViewRange;
    [SerializeField] private float stanceViewRange;
    [SerializeField] private float attackViewRange;

    [Header("Attack Execution")]
    [SerializeField] private float damage;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private float startTimeBtwAttack;
    private float _timeBtwAttack;
    
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
        _timeBtwAttack = startTimeBtwAttack;
        _collider.enabled = true;
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
        
        _isPlayerWithinChase = Physics2D.Raycast(eyes.position, Vector2.left, chaseViewRange, playerLayer);
        _isPlayerWithinStance = Physics2D.Raycast(eyes.position, Vector2.left, stanceViewRange, playerLayer);
        _isPlayerWithinAttack = Physics2D.Raycast(eyes.position, Vector2.left, attackViewRange, playerLayer);
        
        if (_isPlayerWithinChase)
        {
            // Move towards player
            _animator.SetInteger("AnimState",2);

            if (_isPlayerWithinStance)
            {
                _animator.SetInteger("AnimState", 1);
            }

            if (_isPlayerWithinAttack && _canAttack)
            {
                var player = Physics2D.Raycast(eyes.position, Vector2.left, attackViewRange, playerLayer).collider; 
                if(!player.GetComponent<PlayerCombat>().IsDead)
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
        // GiveDamage is invoked so that the player takes damage during the animation
        Invoke("GiveDamage", 0.4f);
        _timeBtwAttack = startTimeBtwAttack;
    }

    public void GiveDamage()
    {
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerLayer);

        foreach (var player in playerToDamage)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(damage);
        }
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
        Gizmos.DrawRay(eyes.position, Vector2.left * chaseViewRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(eyes.position - Vector3.up * 0.25f, Vector2.left * stanceViewRange);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(eyes.position - Vector3.up * 0.5f, Vector2.left * attackViewRange);
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
