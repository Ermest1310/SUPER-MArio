using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance;

    public TMP_Text scoreText;
    public TMP_Text coinText;
    public TMP_Text timeText;

    private int score;
    private float timeRemaining = 300f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateCoins();
        UpdateScore(0);
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;

            GameManager.Instance.ResetLevel(0f);
        }

        timeText.text = "TIME " + Mathf.CeilToInt(timeRemaining);
    }

    public void AddScore(int amount)
    {
        score += amount;

        scoreText.text = score.ToString("D6");
    }

    public void UpdateScore(int amount)
    {
        score += amount;

        scoreText.text = score.ToString("D6");
    }

    public void UpdateCoins()
    {
        coinText.text = "x" + GameManager.Instance.coins.ToString("D2");
    }
}