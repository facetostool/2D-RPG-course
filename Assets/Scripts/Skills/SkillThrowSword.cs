using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

enum SwordThrowType
{
    Normal,
    Bounce,
    Pierce,
    Hover,
}

public class SkillThrowSword : Skill
{
    [Header("Skill UI")]
    [SerializeField] private SkillTreeSlot throwSwordSlot;
    [SerializeField] private bool throwSwordUnlocked;
    
    [SerializeField] private SkillTreeSlot timeStopSlot;
    [SerializeField] public bool timeStopUnlocked;
    [SerializeField] private SkillTreeSlot vulnerabilitySlot;
    [SerializeField] public bool vulnerabilityUnlocked;
    
    [SerializeField] private SkillTreeSlot throwSwordBounceSlot;
    [SerializeField] private bool throwSwordBounceUnlocked;
    [SerializeField] private SkillTreeSlot throwSwordPierceSlot;
    [SerializeField] private bool throwSwordPierceUnlocked;
    [SerializeField] private SkillTreeSlot throwSwordHoverSlot;
    [SerializeField] private bool throwSwordHoverUnlocked;
    
    [Header("Throw Sword Settings")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 throwForce;
    [SerializeField] private float normalGravityScale;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float freezeTime;
    
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
    
    [Header("Hover info")]
    [SerializeField] private float hoverGravityScale;
    [SerializeField] private float hoverMaxDistance;
    [SerializeField] private float hoverTime;
    [SerializeField] private float hoverHitTime;
    private float gravityScale;
    
    protected override void Start()
    {
        base.Start();

        trajectoryPointParent.SetActive(false);
        GenerateDots();
        
        AssignUnlockActions();
    }

    private void AssignUnlockActions()
    {
        throwSwordSlot.onUnlock += () =>
        {
            throwSwordUnlocked = true;
            swordThrowType = SwordThrowType.Normal;
        };
        timeStopSlot.onUnlock += () => timeStopUnlocked = true;
        vulnerabilitySlot.onUnlock += () => vulnerabilityUnlocked = true;
        
        throwSwordBounceSlot.onUnlock += () =>
        {
            throwSwordBounceUnlocked = true;
            swordThrowType = SwordThrowType.Bounce;
        };
        
        throwSwordPierceSlot.onUnlock += () =>
        {
            throwSwordPierceUnlocked = true;
            swordThrowType = SwordThrowType.Pierce;
        };
        
        throwSwordHoverSlot.onUnlock += () =>
        {
            throwSwordHoverUnlocked = true;
            swordThrowType = SwordThrowType.Hover;
        };
    }

    protected override void Update()
    {
        base.Update();
        
        SetupGravityScale();
    }

    public override bool CanUse()
    {
        return throwSwordUnlocked && base.CanUse();
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
        
        // EnableController(swordObject, swordThrowType);
        switch (swordThrowType)
        {
            case SwordThrowType.Normal:
                swordObject.GetComponent<NormalSwordController>().Setup(throwVector, gravityScale, returnSpeed, freezeTime);
                break;
            case SwordThrowType.Hover:
                swordObject.GetComponent<HoverSwordController>().Setup(throwVector, gravityScale, returnSpeed, freezeTime, hoverMaxDistance, hoverTime, hoverHitTime);
                break;
            case SwordThrowType.Bounce:
                swordObject.GetComponent<BounceSwordController>().Setup(throwVector, gravityScale, returnSpeed, freezeTime, bounceNumber, bounceRadius, bounceSpeed);
                break;
            case SwordThrowType.Pierce:
                swordObject.GetComponent<PierceSwordController>().Setup(throwVector, gravityScale, returnSpeed, freezeTime, pierceNumber);
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
            case SwordThrowType.Hover:
                gravityScale = hoverGravityScale;
                break;
        }
    }
}
