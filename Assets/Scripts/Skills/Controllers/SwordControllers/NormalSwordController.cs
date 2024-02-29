using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSwordController : BaseSwordController
{
    public void Setup(Vector2 _throwForce, float _gravityScale, float _returnSpeed, float _freezeTime)
    {
        SetupBase(_throwForce, _gravityScale, _returnSpeed, _freezeTime);
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
        if (enemy != null)
        {
            RegisterDamage(enemy);
        }
        
        StuckSword(other);
    }
}
