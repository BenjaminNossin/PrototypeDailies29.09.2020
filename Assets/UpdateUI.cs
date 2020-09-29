using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public TMP_Text countDownTMP;
    public TMP_Text scoreTMP;
    public TMP_Text congratsTMP;
    public List <TMP_Text> highScoreTMPList;

    bool gameStateIsUpdated;
    public bool resetHighScore = false;

    public GameObject winPanel;
    public GameObject gameOverPanel;
    public GameObject countDownObj;

    private float scoreMultiplier;
    private float countDown;

    private bool hasMissed;

    private int clipSelector;
    public List<AudioClip> clipToPlay;
    public AudioSource badassMusicSource;

    private void OnEnable()
    {
        gameStateIsUpdated = false;
        scoreMultiplier = 8f;
        countDown = 4f; 

        CheckIndicatorTriggerStay.OnSuccessfullRelease += ShowWinPanel;
        CheckIndicatorTriggerStay.OnFailedRelease += ShowGameOverPanel;

        clipSelector = UnityEngine.Random.Range(0, clipToPlay.Count);
        badassMusicSource.PlayOneShot(clipToPlay[clipSelector]);
    }

    void FixedUpdate()
    {
        if (!gameStateIsUpdated && Input.GetKey(KeyCode.Space))
            UpdateUIVisual();
    }

    void UpdateUIVisual()
    {
        // increase score multiplier
        scoreMultiplier += Time.fixedDeltaTime;

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
        else
        {
            countDownObj.SetActive(true);
            countDown -= Time.fixedDeltaTime;
            countDownTMP.text = $"{Math.Truncate(countDown)} seconds left !"; 
        }

        if (countDown < 1f)
        {
            CheckIndicatorTriggerStay.OnFailedRelease(); 
        }
    }

    IEnumerator UpdateGameState()
    {
        yield return new WaitForFixedUpdate(); 

        gameStateIsUpdated = true;

        // update score on win
        if (!hasMissed)
        {
            newScore += (int)rotation.SendForceInfo() * (int)scoreMultiplier; 

            if (newScore > highScoreData.CurrentHighScore)
                highScoreData.CurrentHighScore = newScore;

            scoreTMP.text = $"Your score is : {newScore}";

            if (newScore >= 200 && newScore < 300)
            {
                congratsTMP.text = "Quite Impressive !";
            }
            else if (newScore >= 300 && newScore < 350)
            {
                congratsTMP.text = "Very Impressive !!";
            }
            else if (newScore >= 350)
            {
                congratsTMP.text = "Truly Impressive !!!";
            }
            else
            {
                congratsTMP.text = "Nice Throw";
            }
        }

        // show highscore anyway
        foreach(TMP_Text item in highScoreTMPList)
        {
            item.text = $"High score : {highScoreData.CurrentHighScore}";
        }
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(2f);

        winPanel.SetActive(true);
    }

    IEnumerator Lose()
    {
        yield return new WaitForFixedUpdate();

        gameOverPanel.SetActive(true);
    }
    void ShowWinPanel()
    {
        if (countDown > 0f)
        {
            hasMissed = false; 

            StartCoroutine(UpdateGameState());
            StartCoroutine(Win());
        }
    }

    void ShowGameOverPanel()
    {
        if (!gameStateIsUpdated) // avoid double check from countDonw loss 
        {
            hasMissed = true; 

            StartCoroutine(UpdateGameState());
            StartCoroutine(Lose());
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        CheckIndicatorTriggerStay.OnSuccessfullRelease -= ShowWinPanel;
        CheckIndicatorTriggerStay.OnFailedRelease -= ShowGameOverPanel;

        if (resetHighScore)
            highScoreData.CurrentHighScore = 0; 
    }
}
