using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float forceToAdd = 2f;
    [Range(0.2f, 10)] public float speedIncreaser = 5f;
    public float RotationForce { get; set; }
    public Transform hand;
    public BallTranslation ballBehaviour; 

    public static Action OnThrowing;

    private void OnEnable()
    {
        CheckIndicatorTriggerStay.OnSuccessfullRelease += ReleaseBallLikeCrazy; 
    }

    private void Awake()
    {
        RotationForce = 0f;
        ballBehaviour.enabled = false; 
    }

    void FixedUpdate()
    {
        RotateLikeCrazy();
    }

    private void RotateLikeCrazy()
    {
        forceToAdd = Mathf.Clamp(forceToAdd, 0, 60);
        if (Input.GetKey(KeyCode.Space))
        {
            forceToAdd += speedIncreaser * Time.fixedDeltaTime;
            RotationForce += forceToAdd;
            RotationForce = Mathf.Repeat(RotationForce, 360f);
            transform.rotation = Quaternion.Euler(0f, RotationForce, 0f);
        }
    }

    private void ReleaseBallLikeCrazy()
    {
        ballBehaviour.enabled = true; 
        hand.DetachChildren(); 
    }

    public float SendForceInfo() => forceToAdd;

    private void OnDisable()
    {
        CheckIndicatorTriggerStay.OnSuccessfullRelease -= ReleaseBallLikeCrazy;
    }
}
