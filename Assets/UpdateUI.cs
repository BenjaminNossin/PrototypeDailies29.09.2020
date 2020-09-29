using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public Slider slider;
    public RectTransform window;
    public BoxCollider2D windowCollider; 

    private float accelerationDelta = 0f; 
    private float windowShrinkingDelta = 0f;
    [Range(0.5f, 2)] public float windowShrinkingSpeedMutliplier = 1f;
    [Range(0.5f, 2)] public float accelerationMultiplier = 1f;
    [Range(0.9f, 0.99f)] public float colliderDeltaModifier = 0.95f;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UpdateUIVisual();
        }
    }

    void UpdateUIVisual()
    {
        // increase linear delta 
        windowShrinkingDelta += Time.fixedDeltaTime * windowShrinkingSpeedMutliplier;
        accelerationDelta += Time.fixedDeltaTime * accelerationMultiplier; 
        
        // indicator loop movement
        slider.value += accelerationDelta;
        slider.value = Mathf.Repeat(slider.value, 1f + Mathf.Epsilon);

        // window visual shrinking
        window.sizeDelta = new Vector2(window.sizeDelta.x - (windowShrinkingDelta * windowShrinkingSpeedMutliplier), window.sizeDelta.y);

        // windox collider shrinking
        windowCollider.size = new Vector2(windowCollider.size.x - (windowShrinkingDelta * colliderDeltaModifier), windowCollider.size.y); 
    }
}
