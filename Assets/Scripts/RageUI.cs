using UnityEngine;
using TMPro;

public class RageUI : MonoBehaviour
{
    public TMP_Text rageText;
    public RageController rageController;

    void Awake()
    {
        if (rageText == null)
            rageText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        bool rageEnabled = GameModeSettings.Instance == null || GameModeSettings.Instance.RageEnabled();

        if (!rageEnabled)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (rageText == null || rageController == null)
            return;

        if (rageController.IsRageActive())
        {
            rageText.text = "RAGE!";
        }
        else if (rageController.rageBars >= rageController.maxRageBars)
        {
            rageText.text = "RAGE READY";
        }
        else
        {
            rageText.text = "RAGE " + rageController.rageBars + "/" + rageController.maxRageBars;
        }
    }
}