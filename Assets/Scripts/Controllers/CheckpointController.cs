using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public string id;
    public bool isActivated = false;
    private static readonly int Active = Animator.StringToHash("Active");


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool(Active, isActivated);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [ContextMenu("Generate ID")]
    private void GenerateID()
    {
        id = Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isActivated) return;
        Activate();
    }

    public void Activate()
    {
        isActivated = true;
        GetComponent<Animator>().SetBool(Active, isActivated);
    }
}
