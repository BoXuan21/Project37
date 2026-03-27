using UnityEngine;
using TMPro;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    [Header("Players")]
    public GameObject p1;
    public GameObject p2;

    public Transform p1Spawn;
    public Transform p2Spawn;

    [Header("UI")]
    public TextMeshProUGUI p1Text;
    public TextMeshProUGUI p2Text;
    public GameObject koText;
    public TextMeshProUGUI roundText;

    [Header("Match")]
    public int p1Wins = 0;
    public int p2Wins = 0;
    public int maxWins = 5;

    private bool roundEnding = false;

    void Start()
    {
        UpdateUI();

        if (koText != null)
            koText.SetActive(false);

        if (roundText != null)
            roundText.gameObject.SetActive(false);
    }

    public void BeginMatch()
    {
        DisablePlayers();
        StartCoroutine(StartRound());
    }

    public void PlayerDied(GameObject deadPlayer)
    {
        if (roundEnding) return;

        roundEnding = true;

        if (deadPlayer == p1)
        {
            p2Wins++;
            GiveRoundLossRage(p1);
        }
        else if (deadPlayer == p2)
        {
            p1Wins++;
            GiveRoundLossRage(p2);
        }

        UpdateUI();
        StartCoroutine(RoundEnd());
    }

    void GiveRoundLossRage(GameObject loser)
    {
        if (loser == null) return;

        RageController rage = loser.GetComponent<RageController>();
        if (rage != null)
            rage.AddRageBar();
    }

    IEnumerator RoundEnd()
    {
        if (koText != null)
            koText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        if (koText != null)
            koText.SetActive(false);

        if (p1Wins >= maxWins || p2Wins >= maxWins)
        {
            Debug.Log("MATCH OVER");
            yield break;
        }

        RestartRound();
        roundEnding = false;
    }

    void RestartRound()
    {
        if (p1 != null)
            p1.transform.position = p1Spawn.position;

        if (p2 != null)
            p2.transform.position = p2Spawn.position;

        if (p1 != null)
        {
            p1.GetComponent<PlayerHealth>()?.ResetHealth();
            p1.GetComponent<PlayerMovement2D>()?.ResetMovementState();
        }

        if (p2 != null)
        {
            p2.GetComponent<PlayerHealth>()?.ResetHealth();
            p2.GetComponent<PlayerMovement2D>()?.ResetMovementState();
        }

        DisablePlayers();
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        if (roundText != null)
        {
            roundText.gameObject.SetActive(true);

            roundText.text = "3";
            yield return new WaitForSeconds(0.8f);

            roundText.text = "2";
            yield return new WaitForSeconds(0.8f);

            roundText.text = "1";
            yield return new WaitForSeconds(0.8f);

            roundText.text = "FIGHT!";
            yield return new WaitForSeconds(0.6f);

            roundText.gameObject.SetActive(false);
        }

        EnablePlayers();
    }

    void DisablePlayers()
    {
        if (p1 != null)
        {
            p1.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            p1.GetComponent<PlayerMovement2D>().enabled = false;
            p1.GetComponent<MeleeAttack>().enabled = false;
            p1.GetComponent<ParryController>().enabled = false;
        }

        if (p2 != null)
        {
            p2.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            p2.GetComponent<PlayerMovement2D>().enabled = false;
            p2.GetComponent<MeleeAttack>().enabled = false;
            p2.GetComponent<ParryController>().enabled = false;
        }
    }

    void EnablePlayers()
    {
        if (p1 != null)
        {
            p1.GetComponent<PlayerMovement2D>().enabled = true;
            p1.GetComponent<MeleeAttack>().enabled = true;
            p1.GetComponent<ParryController>().enabled = true;
        }

        if (p2 != null)
        {
            p2.GetComponent<PlayerMovement2D>().enabled = true;
            p2.GetComponent<MeleeAttack>().enabled = true;
            p2.GetComponent<ParryController>().enabled = true;
        }
    }

    void UpdateUI()
    {
        if (p1Text != null)
            p1Text.text = GetWinString(p1Wins);

        if (p2Text != null)
            p2Text.text = GetWinString(p2Wins);
    }

    string GetWinString(int wins)
    {
        string result = "";

        for (int i = 0; i < maxWins; i++)
        {
            result += (i < wins) ? "O " : "_ ";
        }

        return result;
    }
}