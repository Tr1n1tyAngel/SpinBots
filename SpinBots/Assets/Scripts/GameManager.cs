using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject botSelectionPanel;
    public GameObject ADSPanel;

    public float battleTimer;
    public int roundCount;
    //Player Confirms
    public bool p1Ready;
    public bool p2Ready;
    //SpinBot Choices
    public bool p1BotSelected;
    public bool p2BotSelected;
    public bool p1AttackType;
    public bool p1DefenseType;
    public bool p1StaminaType;
    public bool p2AttackType;
    public bool p2DefenseType;
    public bool p2StaminaType;
    public TextMeshProUGUI p1ChoiceTxt;
    public TextMeshProUGUI p2ChoiceTxt;
    //SpinBotStats
    public float p1AttackStat;
    public float p1DefenseStat;
    public float p1StaminaStat;
    public float p2AttackStat;
    public float p2DefenseStat;
    public float p2StaminaStat;

    // Start is called before the first frame update
    void Start()
    {
        introPanel.SetActive(true);
        botSelectionPanel.SetActive(false);
        ADSPanel.SetActive(false);
        p1Ready = false;
        p2Ready = false;
        p1BotSelected = false;
        p2BotSelected = false;
        p1AttackType = false;
        p2AttackType = false;
        p1DefenseType = false;
        p2DefenseType = false;
        p1StaminaType = false;
        p2StaminaType = false;
        p1AttackStat = 0;
        p1DefenseStat = 0;
        p1StaminaStat = 0;
        p2AttackStat = 0;
        p2DefenseStat = 0;
        p2StaminaStat = 0;
        roundCount = 0;
        battleTimer = 360;
        Time.timeScale = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if(battleTimer > 0)
        {
            battleTimer-=Time.deltaTime;
        }
        else
        {
            Time.timeScale = 0;
            EndGame();
        }
        if(battleTimer < 359)
        {
            Time.timeScale = 0;
            ADSPanel.SetActive(true);
        }
        IntroReadyCheck();
        BotSelectionCheck();
        ADSDoneCheck();
    }

    public void IntroReadyCheck()
    {
        if (p1Ready && p2Ready &&introPanel.activeSelf)
        {
            introPanel.SetActive(false);
            botSelectionPanel.SetActive(true);
            p1Ready = false;
            p2Ready = false;
        }
    }
    public void BotSelectionCheck()
    {
        if(p1AttackType)
        {
            p1ChoiceTxt.text = "Player Choice: Attack";
        }
        if (p2AttackType)
        {
            p2ChoiceTxt.text = "Player Choice: Attack";
        }
        if (p1DefenseType)
        {
            p1ChoiceTxt.text = "Player Choice: Defense";
        }
        if (p2DefenseType)
        {
            p2ChoiceTxt.text = "Player Choice: Defense";
        }
        if (p1StaminaType)
        {
            p1ChoiceTxt.text = "Player Choice: Stamina";
        }
        if (p2StaminaType)
        {
            p2ChoiceTxt.text = "Player Choice: Stamina";
        }
        if(p1Ready&&p2Ready&&botSelectionPanel.activeSelf)
        {
            botSelectionPanel.SetActive(false);
            Time.timeScale = 1f;
            p1Ready = false;
            p2Ready = false;
        }
    }
    public void ADSDoneCheck()
    {
        if (p1Ready && p2Ready && ADSPanel.activeSelf && roundCount==3)
        {
            ADSPanel.SetActive(false);
            Time.timeScale= 1f;
            p1Ready = false;
            p2Ready = false;
        }
    }
    public void IncreaseStat(string player, string type)
    {
        if (player == "Player1")
        {
            if (type == "Attack") p1AttackStat++;
            else if (type == "Defense") p1DefenseStat++;
            else if (type == "Stamina") p1StaminaStat++;
        }
        else
        {
            if (type == "Attack") p2AttackStat++;
            else if (type == "Defense") p2DefenseStat++;
            else if (type == "Stamina") p2StaminaStat++;
        }
    }

    public void UpdateOverallWinnerStats(string winner)
    {
        if (winner == "Player1")
        {
            p1AttackStat += 0.25f;
            p1DefenseStat += 0.25f;
            p1StaminaStat += 0.25f;
        }
        else
        {
            p2AttackStat += 0.25f;
            p2DefenseStat += 0.25f;
            p2StaminaStat += 0.25f;
        }
    }

    public void EndGame()
    {

    }
}
