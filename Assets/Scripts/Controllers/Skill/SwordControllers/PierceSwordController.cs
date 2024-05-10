using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceSwordController : BaseSwordController

{
    private int pierceNumber;
    
    public void Setup(Vector2 _throwForce, float _gravityScale, float _returnSpeed, float _freezeTime, int _pierceNumber)
    {
        SetupBase(_throwForce, _gravityScale, _returnSpeed, _freezeTime);
        
        pierceNumber = (int)_pierceNumber;
        animator.SetBool("Spin", false);
    }
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled) return;
        
        base.OnTriggerEnter2D(other);
        
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            StuckSword(other);
            return;
        }
        
        RegisterDamage(enemy);
        if (pierceNumber <= 0)
        {
            StuckSword(other);
            return;
        }

        pierceNumber--;
    }
}
