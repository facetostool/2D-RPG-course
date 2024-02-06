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

    private void Start()
    {
        dash = GetComponent<SkillDash>();
        throwSword = GetComponent<SkillThrowSword>();
    }
}