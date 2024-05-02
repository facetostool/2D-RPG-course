using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    private bool isHit;
    private Animator anim;
    private static readonly int Hit = Animator.StringToHash("Hit");

    public void Setup(GameObject _target, float _speed, int _damage)
    {
        target = _target;
        speed = _speed;
        damage = _damage;
    }
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        if (isHit) return;
        
        if (!target)
        {
            Destroy(gameObject);
            return;
        }
        
        if (Vector3.Distance(transform.position, target.transform.position) > 0.5f)
        {
            transform.right = transform.position - target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            return;
        }
        
        isHit = true;
        
        anim.transform.localRotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(3, 3);
        transform.position = new Vector3(transform.position.x, transform.position.y + 2);
        
        anim.SetBool(Hit, true);
        Invoke(nameof(DamageTarget), 0.2f);
    }
    
    void DamageTarget()
    {
        target.GetComponent<EntityStats>().TakePhysicalDamage(damage);
        Destroy(gameObject, 0.2f);
    }
}
