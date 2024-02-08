using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private float xPositionStart;

    [SerializeField] public float parallaxEffect;
    [SerializeField] private GameObject cam;
    void Start()
    {
        xPositionStart = cam.transform.position.x;
    }
    
    void Update()
    {
        float movedDistance = cam.transform.position.x + xPositionStart;
        transform.position = new Vector3(movedDistance * parallaxEffect, 0);
    }
}
