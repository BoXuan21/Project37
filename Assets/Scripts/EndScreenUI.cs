using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    public GameObject root;
    public TMP_Text winnerText;

    public void ShowWinner(string winnerName)
    {
        root.SetActive(true);
        winnerText.text = winnerName + " WINS!";
        Time.timeScale = 0f;
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