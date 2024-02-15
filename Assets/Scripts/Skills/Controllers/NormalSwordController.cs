using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSwordController : BaseSwordController
{
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
        base.OnTriggerEnter2D(other);
        
        other.GetComponent<Enemy>()?.Damage();

        animator.SetBool("Spin", false);
        transform.parent = other.transform;
        collider2D.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isSpinning = false;
    }

    public void Setup(Vector2 _throwForce, float _gravityScale, float _returnSpeed)
    {
        SetupBase(_throwForce, _gravityScale, _returnSpeed);
    }
}
