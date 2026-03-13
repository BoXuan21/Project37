using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Character Prefabs")]
    public GameObject orcPrefab;
    public GameObject soldierPrefab;
    public GameObject swordsmanPrefab;

    [Header("UI")]
    public TMP_Text p1Text;
    public TMP_Text p2Text;
    public TMP_Text editModeText;

    private int currentPlayerToEdit = 1;

    void Start()
    {
        CharacterSelectionData.player1Prefab = null;
        CharacterSelectionData.player2Prefab = null;

        if (p1Text != null) p1Text.text = "P1: -";
        if (p2Text != null) p2Text.text = "P2: -";
        if (editModeText != null) editModeText.text = "Currently Editing: P1";
    }

    public void SelectOrc() => SelectCharacter(orcPrefab, "Orc");
    public void SelectSoldier() => SelectCharacter(soldierPrefab, "Soldier");
    public void SelectSwordsman() => SelectCharacter(swordsmanPrefab, "Swordsman");

    void SelectCharacter(GameObject prefab, string characterName)
    {
        if (currentPlayerToEdit == 1)
        {
            CharacterSelectionData.player1Prefab = prefab;
            if (p1Text != null) p1Text.text = "P1: " + characterName;
        }
        else
        {
            CharacterSelectionData.player2Prefab = prefab;
            if (p2Text != null) p2Text.text = "P2: " + characterName;
        }
    }

    public void EditPlayer1()
    {
        currentPlayerToEdit = 1;
        if (editModeText != null) editModeText.text = "Currently Editing: P1";
    }

    public void EditPlayer2()
    {
        currentPlayerToEdit = 2;
        if (editModeText != null) editModeText.text = "Currently Editing: P2";
    }

    public void StartFight()
    {
        if (CharacterSelectionData.player1Prefab == null || CharacterSelectionData.player2Prefab == null)
        {
            Debug.LogWarning("Both players must select a character.");
            return;
        }

        SceneManager.LoadScene("FightScene");
    }

    public void BackToMainMenu()
    {
        CharacterSelectionData.player1Prefab = null;
        CharacterSelectionData.player2Prefab = null;
        SceneManager.LoadScene("MainMenu");
    }
}