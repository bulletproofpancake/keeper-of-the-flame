using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _direction;
    private bool _isFlipped;
    
    public float Direction => _direction;
    public bool IsFlipped => _isFlipped;
    public event Action Jump;
    public event Action Attack;

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
        
        if(Input.GetKeyDown(KeyCode.Z)){Jump?.Invoke();}
        if(Input.GetKeyDown(KeyCode.X)){Attack?.Invoke();}
    }
}
