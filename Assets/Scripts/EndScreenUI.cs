using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    public GameObject root;
    public TMP_Text winnerText;
    public TMP_Text statsText;

    public void ShowWinner(string winnerName, float totalSetTime, float averageRoundTime)
    {
        root.SetActive(true);

        if (winnerText != null)
            winnerText.text = winnerName + " WINS!";

        if (statsText != null)
            statsText.text =
                "Set Time: " + FormatTime(totalSetTime) + "\n" +
                "Avg Round: " + Mathf.RoundToInt(averageRoundTime) + "s";

        Time.timeScale = 0f;
    }

    string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);

        if (mins > 0)
            return mins + "m " + secs + "s";

        return secs + "s";
    }

    public void Rematch()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CharacterSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CharacterSelect");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}