using UnityEngine;

public class CheckIndicatorTriggerStay : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Indicator"))
            Debug.Log("indicator is inside trigger"); 
    }
}
