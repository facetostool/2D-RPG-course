using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThrowSword : Skill
{
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 throwForce;
    [SerializeField] private float gravityScale;
    [SerializeField] private float returnSpeed;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUse()
    {
        return base.CanUse();
    }

    public override void Use()
    {
        base.Use();
        
        GameObject swordObject = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        Vector2 throwVector = CalculateThrowVector();
        Vector2 finalThrowVector = new Vector2(throwVector.x * throwForce.x, throwVector.y * throwForce.y);
        swordObject.GetComponent<SwordController>().Setup(finalThrowVector, gravityScale, returnSpeed);
    }
    
    private Vector2 CalculateThrowVector()
    {
       Vector2 playerPosition = player.transform.position;
       Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       Vector2 throwVector = mousePosition - playerPosition;
       return throwVector.normalized;
    }
}
