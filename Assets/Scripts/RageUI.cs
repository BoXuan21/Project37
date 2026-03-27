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
        if (rageText == null || rageController == null)
            return;

        if (rageController.IsRageActive())
        {
            rageText.text = "RAGE!";
        }
        else if (rageController.rageBars >= 2)
        {
            rageText.text = "RAGE READY";
        }
        else
        {
            rageText.text = "RAGE " + rageController.rageBars + "/2";
        }
    }
}