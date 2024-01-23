using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private GameObject cam;
    private float xPositionStart;

    [SerializeField] public float parallaxEffect;
    
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPositionStart = cam.transform.position.x;
    }
    
    void Update()
    {
        float movedDistance = cam.transform.position.x + xPositionStart;
        transform.position = new Vector3(movedDistance * parallaxEffect, 0);
        // transform.position = cam.transform.position;
    } 
}
