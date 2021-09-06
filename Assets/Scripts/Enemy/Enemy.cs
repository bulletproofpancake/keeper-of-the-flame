using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    
    [Header("Attack Detection")]
    [SerializeField] private Transform eyes;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float viewRange;
    [SerializeField] private float chaseViewRange;
    [SerializeField] private float stanceViewRange;
    [SerializeField] private float attackViewRange;

    [Header("Attack Execution")]
    [SerializeField] private float damage;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private float startTimeBtwAttack;
    private float _timeBtwAttack;

    [Header("Audio")] 
    [SerializeField] private Sound hurtSFX;
    [SerializeField] private Sound attackSFX;
    
    private float _currentHealth;
    private Animator _animator;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody2D;
    private bool _isFlipped;
    private bool _isDead;
    private bool _isPlayerWithinView;
    private bool _isPlayerWithinChase;
    private bool _isPlayerWithinStance;
    private bool _isPlayerWithinAttack;
    private bool _canAttack;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        _timeBtwAttack = startTimeBtwAttack;
        _collider.enabled = true;
        _rigidbody2D.isKinematic = false;
        StartCoroutine(DeathRoutine());
    }

    private void Update()
    {
        if (_isDead) return;
        
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

        _isPlayerWithinView = Physics2D.OverlapCircle(transform.position, viewRange, playerLayer);
        _isPlayerWithinChase = Physics2D.Raycast(eyes.position, -transform.right, chaseViewRange, playerLayer);
        _isPlayerWithinStance = Physics2D.Raycast(eyes.position, -transform.right, stanceViewRange, playerLayer);
        _isPlayerWithinAttack = Physics2D.Raycast(eyes.position, -transform.right, attackViewRange, playerLayer);

        transform.rotation = _isFlipped ? Quaternion.Euler(0, 180f, 0) : Quaternion.identity;
        
        if(_isPlayerWithinView)
        {

            var collision = Physics2D.OverlapCircle(transform.position, viewRange, playerLayer);

            _isFlipped = !(collision.transform.position.x < transform.position.x);
            
            if (_isPlayerWithinChase)
            {
                // Move towards player

                _rigidbody2D.velocity = new Vector2(-transform.right.x * movementSpeed, _rigidbody2D.velocity.y);

                _animator.SetInteger("AnimState", 2);

                if (_isPlayerWithinStance)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    _animator.SetInteger("AnimState", 1);
                }

                if (_isPlayerWithinAttack && _canAttack)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    var player = Physics2D.Raycast(eyes.position, -transform.right, attackViewRange, playerLayer).collider;
                    if (!player.GetComponent<PlayerCombat>().IsDead)
                        Attack();
                }

            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
                _animator.SetInteger("AnimState", 0);
            }
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
        AudioManager.Instance.Play(attackSFX);
        foreach (var player in playerToDamage)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(damage);
        }
    }
    
    public void TakeDamage(float damageAmount)
    {
        AudioManager.Instance.Play(hurtSFX);
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
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
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
        Gizmos.DrawWireSphere(transform.position, viewRange);
        Gizmos.DrawRay(eyes.position, -transform.right * chaseViewRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(eyes.position - Vector3.up * 0.25f, -transform.right * stanceViewRange);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(eyes.position - Vector3.up * 0.5f, -transform.right * attackViewRange);
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
