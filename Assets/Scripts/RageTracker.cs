using UnityEngine;

public class RageTracker : MonoBehaviour
{
    [Header("Parry Rage")]
    public int parriesNeededForRageBar = 2;

    private int successfulParryCount = 0;
    private RageController rageController;

    void Awake()
    {
        rageController = GetComponent<RageController>();
    }

    public void RegisterSuccessfulParry()
    {
        successfulParryCount++;

        Debug.Log(name + " successful parries: " + successfulParryCount);

        if (successfulParryCount >= parriesNeededForRageBar)
        {
            successfulParryCount = 0;

            if (rageController != null)
            {
                rageController.AddRageBar();
                Debug.Log(name + " gained 1 Rage Bar from parrying!");
            }
        }
    }

    public void ResetParryCounter()
    {
        successfulParryCount = 0;
    }
}