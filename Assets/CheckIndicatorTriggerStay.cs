using System;
using UnityEngine;

public class CheckIndicatorTriggerStay : MonoBehaviour
{
    public static Action OnSuccessfullRelease;
    public static Action OnFailedRelease;

    private Collider2D detectedCollider; 

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && detectedCollider == null)
        {
            OnFailedRelease(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedCollider = collision;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (detectedCollider.CompareTag("Indicator") && Input.GetKeyUp(KeyCode.Space))
        {
            OnSuccessfullRelease(); 
            Debug.Log("throwing is successful !!"); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCollider = null;
    }
}
