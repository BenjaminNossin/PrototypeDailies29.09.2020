using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public Slider slider;
    public float acceleration = 0.02f;
    public RectTransform window;
    private float temp; 

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UpdateUIVisual();
        }
    }

    void UpdateUIVisual()
    {
        
        slider.value += acceleration;
        slider.value = Mathf.Repeat(slider.value, 1f + Mathf.Epsilon);
        window.sizeDelta = new Vector2(window.sizeDelta.x - slider.value, window.sizeDelta.y);
    }
}
