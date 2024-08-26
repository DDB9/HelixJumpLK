using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI HighScoreText;

    public void PlayAgain()
    {
        GameManager.Instance.GameEndScreen.SetActive(false);
        GameManager.Instance.LoadLevel(0);
    }

    public void QuitGame() { Application.Quit(); }

    public void RestartLevel() { GameManager.Instance.RestartLevel(); }
}
