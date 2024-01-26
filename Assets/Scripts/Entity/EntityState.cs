using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityState
{

    public string anim;
    public float stateTime;
    public bool stopAnimations;

    public EntityState(string _anim)
    {
        anim = _anim;
        
        stateTime = 0;
    }
    
    public virtual void Enter()
    {
        stopAnimations = false;
    }
    
    public virtual void Update()
    {
        if (stateTime >= 0)
        {
            stateTime -= Time.deltaTime;
        }
    }

    public virtual void Exit()
    {

    }

    public void StopAnimationsTrigger()
    {
        stopAnimations = true;
    }
}
