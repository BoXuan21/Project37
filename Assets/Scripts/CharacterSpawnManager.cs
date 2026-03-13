using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
    public Transform p1Spawn;
    public Transform p2Spawn;
    public RoundManager roundManager;

    void Start()
    {
        if (CharacterSelectionData.player1Prefab == null || CharacterSelectionData.player2Prefab == null)
        {
            Debug.LogError("Character selections are missing.");
            return;
        }

        GameObject p1 = Instantiate(CharacterSelectionData.player1Prefab, p1Spawn.position, Quaternion.identity);
        GameObject p2 = Instantiate(CharacterSelectionData.player2Prefab, p2Spawn.position, Quaternion.identity);

        SetupPlayer(p1, 1);
        SetupPlayer(p2, 2);

        roundManager.p1 = p1;
        roundManager.p2 = p2;
        roundManager.p1Spawn = p1Spawn;
        roundManager.p2Spawn = p2Spawn;
    }

    void SetupPlayer(GameObject player, int playerIndex)
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
    }
}