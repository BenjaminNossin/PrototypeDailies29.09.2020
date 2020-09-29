using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [Header("In Game")]
    public Slider slider;
    public RectTransform window;
    public BoxCollider2D windowCollider;

    private float accelerationDelta = 0f;
    private float windowShrinkingDelta = 0f;
    [Range(0.5f, 2)] public float windowShrinkingSpeedMutliplier = 1f;
    [Range(0.1f, 0.4f)] public float accelerationMultiplier = 0.25f;
    [Range(0.9f, 0.99f)] public float colliderDeltaModifier = 0.95f;

    [Header("Score")]
    public HighScoreData highScoreData;
    private int newScore;
    public PlayerRotation rotation;
    public TMP_Text score;
    public TMP_Text highScore;

    bool ballIsThrown;
    public bool resetHighScore = false;

    public GameObject winPanel;
    public GameObject gameOverPanel;

    public const int scoreMultiplier = 10; 


    private void OnEnable()
    {
        ballIsThrown = false; 
        CheckIndicatorTriggerStay.OnSuccessfullRelease += UpdateGameState;
        CheckIndicatorTriggerStay.OnFailedRelease += ShowGameOverPanel; 
    }

    void FixedUpdate()
    {
        if (!ballIsThrown && Input.GetKey(KeyCode.Space))
            UpdateUIVisual();
    }

    void UpdateUIVisual()
    {
        // increase linear delta 
        windowShrinkingDelta += Time.fixedDeltaTime * windowShrinkingSpeedMutliplier;
        accelerationDelta += Time.fixedDeltaTime * accelerationMultiplier;

        // indicator loop movement
        slider.value += Time.fixedDeltaTime * accelerationDelta;
        slider.value = Mathf.Repeat(slider.value, 1f + Mathf.Epsilon);

        // avoid shrinking in negative
        if (window.sizeDelta.x > 5f)
        {
            // window visual shrinking
            window.sizeDelta = new Vector2(window.sizeDelta.x - (windowShrinkingDelta * windowShrinkingSpeedMutliplier), window.sizeDelta.y);

            // windox collider shrinking
            windowCollider.size = new Vector2(windowCollider.size.x - (windowShrinkingDelta * colliderDeltaModifier), windowCollider.size.y); 
        }
    }

    void UpdateGameState()
    {
        ballIsThrown = true;
        newScore += (int)rotation.SendForceInfo() * scoreMultiplier; 

        if (newScore > highScoreData.CurrentHighScore)
            highScoreData.CurrentHighScore = newScore;

        score.text = $"Your score is : {newScore}";
        highScore.text = $"High score : {highScoreData.CurrentHighScore}";

        StartCoroutine(ShowWinPanel()); 
    }

    void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    IEnumerator ShowWinPanel()
    {
        yield return new WaitForSecondsRealtime(2f); 
        winPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    private void OnDisable()
    {
        CheckIndicatorTriggerStay.OnSuccessfullRelease -= UpdateGameState;
        CheckIndicatorTriggerStay.OnFailedRelease -= ShowGameOverPanel;

        if (resetHighScore)
            highScoreData.CurrentHighScore = 0; 
    }
}
