using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    #region singleton

    public static SkillManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #endregion

    public SkillDash dash { get; private set; }
    public SkillThrowSword throwSword { get; private set; }
    public SkillBlackHole blackHole { get; private set; }
    public SkillCrystal crystal { get; private set; }
    public SkillClone clone { get; private set; }
    public SkillParry parry { get; private set; }
    public SkillDodge dodge { get; private set; }

    private void Start()
    {
        dash = GetComponent<SkillDash>();
        throwSword = GetComponent<SkillThrowSword>();
        blackHole = GetComponent<SkillBlackHole>();
        crystal = GetComponent<SkillCrystal>();
        clone = GetComponent<SkillClone>();
        parry = GetComponent<SkillParry>();
        dodge = GetComponent<SkillDodge>();
    }
}
