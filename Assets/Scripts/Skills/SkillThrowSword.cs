using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

enum SwordThrowType
{
    Normal,
    Bounce,
    Pierce
}

public class SkillThrowSword : Skill
{
    [Header("Throw Sword Settings")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 throwForce;
    [SerializeField] private float normalGravityScale;
    [SerializeField] private float returnSpeed;
    
    [Header("Trajectory Settings")]
    [SerializeField] private int trajectoryPointsNumber;
    [SerializeField] private float trajectoryPointSpacing;
    [SerializeField] private GameObject trajectoryPointPrefab;
    [SerializeField] private GameObject trajectoryPointParent;
    private List<GameObject> trajectoryPoints = new List<GameObject>();
    
    [Header("Sword Type Settings")]
    [SerializeField] private SwordThrowType swordThrowType;
    
    [Header("Bouncing info")]
    [SerializeField] private int bounceNumber;
    [SerializeField] private float bounceRadius;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceGravityScale;
    
    [Header("Pierce info")]
    [SerializeField] private int pierceNumber;
    [SerializeField] private float pierceGravityScale;
    
    private float gravityScale;
    protected override void Start()
    {
        base.Start();

        GenerateDots();
    }

    protected override void Update()
    {
        base.Update();

        SetupGravityScale();
    }

    public override bool CanUse()
    {
        return base.CanUse();
    }

    public void EnablePoints()
    {
        trajectoryPointParent.SetActive(true);
    }

    public void DisablePoints()
    {
        trajectoryPointParent.SetActive(false);
    }
    
    private void GenerateDots()
    {
        for (int i = 0; i < trajectoryPointsNumber; i++)
        {
            GameObject trajectoryPoint = Instantiate(trajectoryPointPrefab, player.transform.position, Quaternion.identity); 
            trajectoryPoint.transform.parent = trajectoryPointParent.transform;
            trajectoryPoints.Add(trajectoryPoint);
        }
    }
    
    public void UpdateDotsPosition()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 throwForce = CalculateThrowVector();
        
        for (int i = 0; i < trajectoryPointsNumber; i++)
        {
            float time = i * trajectoryPointSpacing;
            Vector2 position = playerPosition + throwForce * time + Physics2D.gravity * (0.5f * gravityScale * time * time);
            trajectoryPoints[i].transform.position = position;
        }
    }

    public override void Use()
    {
        base.Use();
        
        GameObject swordObject = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        Vector2 throwVector = CalculateThrowVector();
        swordObject.GetComponent<SwordController>().Setup(throwVector, gravityScale, returnSpeed);
        switch (swordThrowType)
        {
            case SwordThrowType.Normal:
                break;
            case SwordThrowType.Bounce:
                swordObject.GetComponent<SwordController>().SetupBounce(bounceNumber, bounceRadius, bounceSpeed);
                break;
            case SwordThrowType.Pierce:
                swordObject.GetComponent<SwordController>().SetupPierce(pierceNumber);
                break;
        }
    }
    
    private Vector2 CalculateThrowVector()
    {
       Vector2 playerPosition = player.transform.position;
       Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       Vector2 throwVector = mousePosition - playerPosition;
       return new Vector2(throwVector.normalized.x * throwForce.x, throwVector.normalized.y * throwForce.y);
    }
    
    private void SetupGravityScale()
    {
        switch (swordThrowType)
        {
            case SwordThrowType.Normal:
                gravityScale = normalGravityScale;
                break;
            case SwordThrowType.Bounce:
                gravityScale = bounceGravityScale;
                break;
            case SwordThrowType.Pierce:
                gravityScale = pierceGravityScale;
                break;
        }
    }
}
