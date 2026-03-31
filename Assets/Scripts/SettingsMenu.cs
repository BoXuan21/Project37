using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("P1 UI")]
    public TMP_Text p1AttackText;
    public TMP_Text p1ParryText;
    public TMP_Text p1RageText;

    [Header("P2 UI")]
    public TMP_Text p2AttackText;
    public TMP_Text p2ParryText;
    public TMP_Text p2RageText;

    [Header("Status")]
    public TMP_Text statusText;

    private bool waitingForKey = false;
    private string bindingKey = "";

    void Start()
    {
        RefreshUI();
    }

    void Update()
    {
        if (!waitingForKey) return;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                InputBindings.SetKey(bindingKey, key);
                waitingForKey = false;
                bindingKey = "";

                if (statusText != null)
                    statusText.text = "";

                RefreshUI();
                return;
            }
        }
    }

    void RefreshUI()
    {
        if (p1AttackText != null) p1AttackText.text = InputBindings.P1Attack.ToString();
        if (p1ParryText != null) p1ParryText.text = InputBindings.P1Parry.ToString();
        if (p1RageText != null) p1RageText.text = InputBindings.P1Rage.ToString();

        if (p2AttackText != null) p2AttackText.text = InputBindings.P2Attack.ToString();
        if (p2ParryText != null) p2ParryText.text = InputBindings.P2Parry.ToString();
        if (p2RageText != null) p2RageText.text = InputBindings.P2Rage.ToString();
    }

    void StartRebind(string keyName)
    {
        if (waitingForKey) return;

        waitingForKey = true;
        bindingKey = keyName;

        if (statusText != null)
            statusText.text = "Press any key...";
    }

    public void RebindP1Attack() => StartRebind("P1Attack");
    public void RebindP1Parry()  => StartRebind("P1Parry");
    public void RebindP1Rage()   => StartRebind("P1Rage");

    public void RebindP2Attack() => StartRebind("P2Attack");
    public void RebindP2Parry()  => StartRebind("P2Parry");
    public void RebindP2Rage()   => StartRebind("P2Rage");

    public void ResetDefaults()
    {
        InputBindings.SetKey("P1Attack", KeyCode.J);
        InputBindings.SetKey("P1Parry", KeyCode.K);
        InputBindings.SetKey("P1Rage", KeyCode.L);

        InputBindings.SetKey("P2Attack", KeyCode.I);
        InputBindings.SetKey("P2Parry", KeyCode.O);
        InputBindings.SetKey("P2Rage", KeyCode.P);

        waitingForKey = false;
        bindingKey = "";

        if (statusText != null)
            statusText.text = "Defaults restored";

        RefreshUI();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}