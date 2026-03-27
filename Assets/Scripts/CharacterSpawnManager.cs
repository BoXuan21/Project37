using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
    public Transform p1Spawn;
    public Transform p2Spawn;
    public RoundManager roundManager;

    [Header("UI")]
    public HeartUI p1HeartUI;
    public HeartUI p2HeartUI;

    public RageUI p1RageUI;
    public RageUI p2RageUI;

    void Start()
    {
        if (CharacterSelectionData.player1Prefab == null || CharacterSelectionData.player2Prefab == null)
        {
            Debug.LogError("Character selections are missing.");
            return;
        }

        GameObject p1 = Instantiate(CharacterSelectionData.player1Prefab, p1Spawn.position, Quaternion.identity);
        GameObject p2 = Instantiate(CharacterSelectionData.player2Prefab, p2Spawn.position, Quaternion.identity);

        SetupPlayer(p1, 1, p1HeartUI);
        SetupPlayer(p2, 2, p2HeartUI);

        roundManager.p1 = p1;
        roundManager.p2 = p2;
        roundManager.p1Spawn = p1Spawn;
        roundManager.p2Spawn = p2Spawn;

        RageController p1Rage = p1.GetComponent<RageController>();
        RageController p2Rage = p2.GetComponent<RageController>();

        if (p1RageUI != null)
            p1RageUI.rageController = p1Rage;

        if (p2RageUI != null)
            p2RageUI.rageController = p2Rage;

        // Ganz wichtig: ERST NACH DEM SPAWN starten
        roundManager.BeginMatch();
    }

    void SetupPlayer(GameObject player, int playerIndex, HeartUI heartUI)
    {
        PlayerMovement2D movement = player.GetComponent<PlayerMovement2D>();
        if (movement != null)
            movement.playerIndex = playerIndex;

        MeleeAttack attack = player.GetComponent<MeleeAttack>();
        if (attack != null)
            attack.playerIndex = playerIndex;

        ParryController parry = player.GetComponent<ParryController>();
        if (parry != null)
            parry.playerIndex = playerIndex;

        RageController rage = player.GetComponent<RageController>();
        if (rage != null)
            rage.playerIndex = playerIndex;

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
            health.SetHeartUI(heartUI);
    }
}