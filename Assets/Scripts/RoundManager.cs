using UnityEngine;
using TMPro;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI p1Text;
    public TextMeshProUGUI p2Text;
    public GameObject koText;

    public int p1Wins = 0;
    public int p2Wins = 0;

    public int maxWins = 5;

    public Transform p1Spawn;
    public Transform p2Spawn;

    public GameObject p1;
    public GameObject p2;

    bool roundEnding = false;

    void Start()
    {
        UpdateUI();
    }

    IEnumerator StartRound()
{
    DisablePlayers();

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

    EnablePlayers();
}
void DisablePlayers()
{
    p1.GetComponent<PlayerMovement2D>().enabled = false;
    p2.GetComponent<PlayerMovement2D>().enabled = false;

    p1.GetComponent<MeleeAttack>().enabled = false;
    p2.GetComponent<MeleeAttack>().enabled = false;
}

void EnablePlayers()
{
    p1.GetComponent<PlayerMovement2D>().enabled = true;
    p2.GetComponent<PlayerMovement2D>().enabled = true;

    p1.GetComponent<MeleeAttack>().enabled = true;
    p2.GetComponent<MeleeAttack>().enabled = true;
}

    public void PlayerDied(GameObject deadPlayer)
    {
        if (roundEnding) return;

        roundEnding = true;

        if (deadPlayer == p1)
            p2Wins++;
        else
            p1Wins++;

        UpdateUI();

        StartCoroutine(RoundEnd());
    }

    IEnumerator RoundEnd()
    {
        koText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        koText.SetActive(false);

        if (p1Wins >= maxWins || p2Wins >= maxWins)
        {
            Debug.Log("MATCH OVER");
        }
        else
        {
            RestartRound();
        }

        roundEnding = false;
    }

void RestartRound()
{
    p1.transform.position = p1Spawn.position;
    p2.transform.position = p2Spawn.position;

    p1.GetComponent<PlayerHealth>().ResetHealth();
    p2.GetComponent<PlayerHealth>().ResetHealth();

    ResetAnimator(p1);
    ResetAnimator(p2);
    StartCoroutine(StartRound());
}

void ResetAnimator(GameObject player)
{
    Animator anim = player.GetComponent<Animator>();

    anim.Rebind();
    anim.Update(0f);
    player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
}

    void UpdateUI()
    {
        p1Text.text = GetWinString(p1Wins);
        p2Text.text = GetWinString(p2Wins);
    }

    string GetWinString(int wins)
    {
        string s = "";

        for (int i = 0; i < maxWins; i++)
        {
            if (i < wins)
                s += "W ";
            else
                s += "_ ";
        }

        return s;
    }
}