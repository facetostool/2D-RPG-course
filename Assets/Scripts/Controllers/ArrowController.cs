using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private int damage;

    [SerializeField] private bool isMove;
    
    private string whatIsTarget = "Player";
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider2D;
    private bool isFlipped;
    private ParticleSystem effectSystem;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        effectSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed * transform.localScale.x, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask layer = LayerMask.NameToLayer(whatIsTarget);
        if (other.gameObject.layer == layer)
        {
            other.GetComponent<EntityStats>().TakeArrowDamage(damage, transform);
            Stuck(other);
            return;
        } 
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Stuck(other);
        }
    }
    
    private void Stuck(Collider2D other)
    {
        effectSystem.Stop();
        isMove = false;
        capsuleCollider2D.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.SetParent(other.transform);
        Destroy(gameObject, lifeTime);
    }
    
    public void Flip()
    {
        if (isFlipped) return;
        
        transform.localScale = new Vector3(-1*transform.localScale.x, transform.localScale.y, transform.localScale.z);;
        isFlipped = true;
        whatIsTarget = "Enemy";
    }

    public void Setup(int _damage, float _speed, float faceDir)
    {
        damage = _damage;
        speed = _speed;
        transform.localScale = new Vector3(faceDir*transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
