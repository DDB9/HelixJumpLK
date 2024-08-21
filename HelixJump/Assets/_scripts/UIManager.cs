using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI HighScoreText;

    private void Update()
    {
        // VERY simple score system.
        ScoreText.text = GameManager.Instance.Score.ToString();
        HighScoreText.text = "High Score: " + GameManager.Instance.HighScore.ToString();
    }
}
