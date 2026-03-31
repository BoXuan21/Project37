using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public string sceneName = "CharacterSelect";
    public string settingsSceneName = "Settings";

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(settingsSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}