using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _direction;
    private bool _isFlipped;

    private PlayerMovement _movement;
    private PlayerCombat _combat;
    
    public float Direction => _direction;
    public bool IsFlipped => _isFlipped;
    public event Action Jump;
    public event Action Attack;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _combat = GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");

        if (_direction < 0)
        {
            _isFlipped = true;
        }
        // uses else if instead of else
        // so that when direction == 0
        // which is when the player doesn't press a key
        // we retain the orientation
        else if (_direction > 0)
        {
            _isFlipped = false;
        }
        
        if(Input.GetKeyDown(KeyCode.Z) && _combat.CanAttack){Attack?.Invoke();}
        if(Input.GetKeyDown(KeyCode.X) && _movement.IsGrounded){Jump?.Invoke();}

    }
}
