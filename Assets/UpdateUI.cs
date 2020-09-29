using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public List <TMP_Text> highScoreTMPList;

    bool ballIsThrown;
    public bool resetHighScore = false;

    public GameObject winPanel;
    public GameObject gameOverPanel;

    public const int scoreMultiplier = 10;

    private AsyncOperation asyncOperation; 

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

        foreach(TMP_Text item in highScoreTMPList)
        {
            item.text = $"High score : {highScoreData.CurrentHighScore}";
        }

        score.text = $"Your score is : {newScore}";


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

    public void PlayAgain()
    {
        asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false; 

        if (asyncOperation.progress >= 0.9f)
        {
            StartCoroutine(LoadNewGame());
            Debug.Log("scene ready to be loaded"); 
        }
    }

    IEnumerator LoadNewGame()
    {
        yield return new WaitForSecondsRealtime(2f);
        asyncOperation.allowSceneActivation = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        CheckIndicatorTriggerStay.OnSuccessfullRelease -= UpdateGameState;
        CheckIndicatorTriggerStay.OnFailedRelease -= ShowGameOverPanel;

        if (resetHighScore)
            highScoreData.CurrentHighScore = 0; 
    }
}
