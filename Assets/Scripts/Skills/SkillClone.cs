using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillClone : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float disappearSpeed;
    [SerializeField] private bool canUseOnCounterAttack;
    
    public void Use(Vector3 position, Transform enemyPosition)
    {
        base.Use();
        
        GameObject cloneObject = Instantiate(clonePrefab);
        cloneObject.GetComponent<CloneController>().Setup(position, disappearSpeed, enemyPosition);
    }
    
    public void UseFromCounterAttack(Transform _hit)
    {
        if (!canUseOnCounterAttack)
            return;

        StartCoroutine(UseWithDelay(new Vector3(_hit.position.x + player.faceDir * 2, _hit.position.y), _hit, 0.4f));
    }
    
    public IEnumerator UseWithDelay(Vector3 position, Transform enemyPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        Use(position, enemyPosition);
    }
}
