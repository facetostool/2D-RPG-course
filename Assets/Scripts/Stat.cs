using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int baseValue;
    [SerializeField] List<int> modifiers = new List<int>();

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
    
    public int Value()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        
        return finalValue;
    }

    public void SetBaseValue(int value)
    {
        baseValue = value;
    }
}
