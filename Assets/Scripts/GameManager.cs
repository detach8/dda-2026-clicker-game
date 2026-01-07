using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    TMP_Text welcomeText;
    [SerializeField]
    TMP_Text scoreText;

    private Queue<float> clicks = new Queue<float>();
    private float highestCps;
    private float currentCps;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicks.Enqueue(Time.time);
            if (clicks.Count > 3) clicks.Dequeue();
            CalculateClicksPerSecond();
        }

    }

    void OnGUI()
    {
        if (DatabaseManager.Instance.DisplayName != null)
        {
            welcomeText.text = $"Welcome, {DatabaseManager.Instance.DisplayName}";
        }
    }

    public void ResetScore()
    {
        scoreText.text = "0.00";
        highestCps = 0f;
        currentCps = 0f;
        clicks.Clear();
    }

    void CalculateClicksPerSecond()
    {
        float first = 0f;
        float last = 0f;
        short size = 0;

        foreach (float t in clicks)
        {
            if (first == 0f) first = t;
            else last = t;
            size++;
        }

        if (size >= 3)
        {
            currentCps = 1 / ((last - first) / (size - 1));
            UpdateClicksPerSecond();
        }
    }

    void UpdateClicksPerSecond()
    {
        if (currentCps > 0 && currentCps > highestCps)
        {
            highestCps = currentCps;
            scoreText.text = highestCps.ToString("n2");

            // Updates the latest high score to database
            DatabaseManager.Instance.UpdateScore(highestCps);
        }
    }

    private Coroutine scoreAnimationCoroutine;

    void OnEnable()
    {
        // Read the display name from database
        DatabaseManager.Instance.GetDisplayName();

        // Animate the title colours when GameObject is active
        StartCoroutine(ColourAnimator.Animate(scoreText));
    }

    void OnDisable()
    {
        // Stop animating the title colours when GameObject is disabled
        if (scoreAnimationCoroutine != null) StopCoroutine(scoreAnimationCoroutine);
    }
}
