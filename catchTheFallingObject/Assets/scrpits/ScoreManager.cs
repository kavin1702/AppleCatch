using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;
    public TMP_Text scoreText;

    private void Start()
    {
        scoreText.text = "Score :";
    }

    void Awake()
    { 
        instance = this;
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score; 
    }
}
