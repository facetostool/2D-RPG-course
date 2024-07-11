using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemEffect : ScriptableObject
{
    public virtual void Apply(Transform target)
    {
        Debug.Log("Applying effect");
    }
}
