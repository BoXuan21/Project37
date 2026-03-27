using UnityEngine;

public class RageReadyIndicator : MonoBehaviour
{
    public GameObject indicatorObject;
    public RageController rageController;

    void Update()
    {
        if (indicatorObject == null || rageController == null) return;

        indicatorObject.SetActive(rageController.rageBars >= 2 && !rageController.IsRageActive());
    }
}