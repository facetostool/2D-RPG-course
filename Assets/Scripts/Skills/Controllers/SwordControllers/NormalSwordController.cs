using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSwordController : BaseSwordController
{
    public void Setup(Vector2 _throwForce, float _gravityScale, float _returnSpeed)
    {
        SetupBase(_throwForce, _gravityScale, _returnSpeed);
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
        
        other.GetComponent<Enemy>()?.Damage();

        StuckSword(other);
    }
}
