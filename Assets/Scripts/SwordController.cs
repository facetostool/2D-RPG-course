using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    // private Vector2 throwForce;

    private Animator animator;
    private Rigidbody2D rb;
    
    public void Setup(Vector2 _throwForce, float _gravityScale)
    {
        rb.velocity = new Vector2(_throwForce.x, _throwForce.y);
        rb.gravityScale = _gravityScale;
    }
 
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

   
    void Update()
    {
        
    }
}
