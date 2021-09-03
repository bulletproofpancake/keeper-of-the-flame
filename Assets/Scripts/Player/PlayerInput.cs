using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerCombat _combat;

    private float _direction;
    private bool _isFlipped;
   
    public float Direction => _direction;
    public bool IsFlipped => _isFlipped;
    
    public event Action Jump;
    public event Action Attack;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _combat = GetComponent<PlayerCombat>();
    }
    
    private void Update()
    {
        if (_combat.IsDead) return;

        _direction = GameManager.Instance.IsGameEnd ? 0 : Input.GetAxisRaw("Horizontal");

        if (_direction < 0)
        {
            _isFlipped = true;
        }
        // used else if to retain player orientation
        else if (_direction > 0)
        {
            _isFlipped = false;
        }
        
        if(Input.GetKeyDown(KeyCode.Z) && _combat.CanAttack){Attack?.Invoke();}
        if(Input.GetKeyDown(KeyCode.X) && _movement.IsGrounded){Jump?.Invoke();}

    }
}
