using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float startTimeBtwAttack;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float damage;

    private PlayerInput _input;
    private float _timeBtwAttack;
    private bool _canAttack;
    
    public bool CanAttack => _canAttack;

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
    }
    
    private void Update()
    {
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
        print("Attacking");
        
        // TODO: INSERT ENEMY DAMAGE METHOD
        
        _timeBtwAttack = startTimeBtwAttack;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position,attackRange);
    }
    
}
