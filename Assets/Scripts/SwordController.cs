using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isSpinning;
    private bool isReturning;
    private float returnSpeed;
    
    public void Setup(Vector2 _throwForce, float _gravityScale, float _returnSpeed)
    {
        rb.velocity = new Vector2(_throwForce.x, _throwForce.y);
        rb.gravityScale = _gravityScale;
        player.SetSword(gameObject);
        returnSpeed = _returnSpeed;
    }
 
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        player = PlayerManager.instance.player;
    }

    void Start()
    {
        animator.SetBool("Spin", true);
        isSpinning = true;
        isReturning = false;
    }
   
    void Update()
    {
        if (isSpinning)
            transform.right = rb.velocity;
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isReturning = true;
            transform.parent = null;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                player.transform.position,
                returnSpeed * Time.deltaTime
            );
            
            if (transform.position == player.transform.position)
            {
                player.ClearSword();
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        animator.SetBool("Spin", false);
        transform.parent = other.transform;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isSpinning = false;
    }
}
