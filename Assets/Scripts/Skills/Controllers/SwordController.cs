using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwordController : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D collider2D;
    
    private bool isSpinning;
    private bool isReturning;
    private float returnSpeed;

    private bool isNormal;

    #region bounce
    
    private bool isBouncing;
    private int bounceNumber;
    private float bounceRadius;
    private float bounceSpeed;
    private int currentBounceTargetNumber;
    private List<Transform> bouncesEnemies = new List<Transform>();
    
    #endregion
    
    private bool isPierce;
    private int pierceNumber;
    
    public void Setup(Vector2 _throwForce, float _gravityScale, float _returnSpeed)
    {
        rb.velocity = new Vector2(_throwForce.x, _throwForce.y);
        rb.gravityScale = _gravityScale;
        player.SetSword(gameObject);
        returnSpeed = _returnSpeed;
        isNormal = true;
        
        animator.SetBool("Spin", true);
        isSpinning = true;
    }
    
    public void SetupBounce(int _bounceNumber, float _bounceRadius, float _bounceSpeed)
    {
        isNormal = false;
        isBouncing = true;
        bounceNumber = _bounceNumber;
        bounceRadius = _bounceRadius;
        bounceSpeed = _bounceSpeed;
    }
    
    public void SetupPierce(int _pierceNumber)
    {
        isNormal = false;
        isPierce = true;
        pierceNumber = _pierceNumber;
        
        animator.SetBool("Spin", false);
    }
 
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CircleCollider2D>();
        
        player = PlayerManager.instance.player;
    }

    void Start()
    {
        isReturning = false;
    }
   
    void Update()
    {
        if (isSpinning && !isReturning)
            transform.right = rb.velocity;
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isReturning = true;
            transform.parent = null;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        if (isBouncing)
        {
            if (bouncesEnemies.Count > 0)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position, 
                    bouncesEnemies[currentBounceTargetNumber].position,
                    bounceSpeed * Time.deltaTime
                );
                
                if (Vector2.Distance(transform.position, bouncesEnemies[currentBounceTargetNumber].position) < 0.1f)
                {
                    currentBounceTargetNumber++;
                    bounceNumber--;
                    if (currentBounceTargetNumber >= bouncesEnemies.Count)
                    {
                        currentBounceTargetNumber = 0;
                    }
                    if (bounceNumber <= 0)
                    {
                        isBouncing = false;
                        isReturning = true;
                    }
                }
            }
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
        if (isPierce)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
                if (pierceNumber <= 0)
                {
                    StopSword(other);
                }
                else
                {
                    pierceNumber--;
                    return;
                }
            }
        }

        if (isBouncing)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (bouncesEnemies.Count <= 0)
                {
                    bouncesEnemies = new List<Transform>();
                    Collider2D[] hits = Physics2D.OverlapCircleAll(enemy.transform.position, bounceRadius);
                    foreach (var hit in hits)
                    {
                        Enemy enemyInRadius = hit.GetComponent<Enemy>();
                        if (enemyInRadius)
                        {
                            bouncesEnemies.Add(enemyInRadius.transform);
                        }
                    }
                }
                enemy.Damage();
            }
            else
            {
                StopSword(other);
            }
        }
        
        if (isNormal)
        {
            StopSword(other);
            other.GetComponent<Enemy>()?.Damage();
        }

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isSpinning = false;
    }

    private void StopSword(Collider2D other)
    {
        animator.SetBool("Spin", false);
        transform.parent = other.transform;
        collider2D.enabled = false;
    }
}
