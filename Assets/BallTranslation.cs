using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTranslation : MonoBehaviour
{
    public PlayerRotation rotation;
    [Range(2, 4)] public float speedMultiplier = 3; 

    private void OnEnable()
    {
        InvokeRepeating(nameof(Translate), 0f, Time.fixedDeltaTime); 
    }

    void Translate()
    {
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * rotation.SendForceInfo() * 3f);
    }

}
