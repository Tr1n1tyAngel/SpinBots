using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject introPanel, botSelectionPanel, ADSPanel, ADSP1Ready,ADSP2Ready, BGPanel, BGP1Ready, BGP2Ready, P1, P2;

    public TextMeshProUGUI p1ChoiceTxt, p2ChoiceTxt, ADSP1ScoreTxt, ADSP2ScoreTxt, ADSRoundTxt, ADSWinner, BGRoundTxt, BGOutcomeTxt;
    public float battleTimer;
    public bool ADS1Complete, BG1Complete;
    //ADS
    public int roundCount, player1Score, player2Score;
    public string winner, winningType, player1Choice, player2Choice;
    //BuffGame
    public int bgRoundCount;
    public string bgP1BuffList;
    public string bgP2BuffList;
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
    
    //SpinBotStats
    public float p1AttackStat;
    public float p1DefenseStat;
    public float p1StaminaStat;
    public float p2AttackStat;
    public float p2DefenseStat;
    public float p2StaminaStat;

    //Random player spawning
    public Vector3 areaCenter = new Vector3(0, 0, 0);  
    public Vector3 areaSize = new Vector3(1f, 0, 1f);
    // Start is called before the first frame update
    void Start()
    {
        introPanel.SetActive(true);
        botSelectionPanel.SetActive(false);
        ADSPanel.SetActive(false);
        ADSP1Ready.SetActive(false);
        ADSP2Ready.SetActive(false);
        BGPanel.SetActive(false);
        BGP1Ready.SetActive(false);
        BGP2Ready.SetActive(false);
        P1.SetActive(false);
        P2.SetActive(false);
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
        roundCount = 1;
        bgRoundCount = 1;
        player1Score = 0;
        player2Score = 0;
        battleTimer = 360;
        Time.timeScale = 0;
        winner = null;
        winningType = null;
        bgP1BuffList = null;
        bgP2BuffList = null;
        ADS1Complete = false;
        BG1Complete = false;

    }

    
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
        if(battleTimer <=300 && ADS1Complete==false)
        {
            Time.timeScale = 0;
            ADSPanel.SetActive(true);
        }
        if (battleTimer <= 240 && BG1Complete == false)
        {
            Time.timeScale = 0;
            BGPanel.SetActive(true);
        }
        IntroReadyCheck();
        BotSelectionCheck();
        ADSDisplay();
        ADSReadyCheck();
        StartCoroutine(ADSDoneCheck());
        BGReadyCheck();
        StartCoroutine(BGDoneCheck());
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
            BotStats();
            P1.SetActive(true);
            P2.SetActive(true);
            SpawnPlayers();
            botSelectionPanel.SetActive(false);
            Time.timeScale = 1f;
            p1Ready = false;
            p2Ready = false;
        }
    }
    public void ADSReadyCheck()
    {
        if(p1Ready)
        {
            ADSP1Ready.SetActive(true);
        }
        else
        {
            ADSP1Ready.SetActive(false);
        }
        if (p2Ready)
        {
            ADSP2Ready.SetActive(true);
        }
        else
        {
            ADSP2Ready.SetActive(false);
        }

    }
    public void BGReadyCheck()
    {
        if (p1Ready)
        {
           BGP1Ready.SetActive(true);
        }
        else
        {
            BGP1Ready.SetActive(false);
        }
        if (p2Ready)
        {
            BGP2Ready.SetActive(true);
        }
        else
        {
            BGP2Ready.SetActive(false);
        }

    }
    IEnumerator ADSDoneCheck()
    {
        if (p1Ready && p2Ready && ADSPanel.activeSelf && roundCount == 4)
        {
            yield return new WaitForSecondsRealtime(3f); 

            ADSPanel.SetActive(false);
            ADS1Complete = true;
            Time.timeScale = 1f; 
            p1Ready = false;
            p2Ready = false;
            winner = "";
            winningType = "";
            roundCount = 0;
            player1Score = 0;
            player2Score = 0;
            

        }
    }
    IEnumerator BGDoneCheck()
    {
        if (p1Ready && p2Ready && BGPanel.activeSelf && bgRoundCount == 4)
        {
            yield return new WaitForSecondsRealtime(3f);

            BGPanel.SetActive(false);
            BG1Complete = true;
            Time.timeScale = 1f;
            p1Ready = false;
            p2Ready = false;
            bgRoundCount = 0;
            bgP1BuffList = null;
            bgP2BuffList = null;


        }
    }
    public void ADSDisplay()
    {
        if(roundCount > 0)
        {
            ADSRoundTxt.text = "Round: " + roundCount;
        }
       
        ADSP1ScoreTxt.text = "Score: " + player1Score;
        ADSP2ScoreTxt.text = "Score: " + player2Score;
        
            if (roundCount == 4)
            {
                ADSWinner.text = "Game Over: Player 1 Score: " + player1Score + "\nPlayer 2 Score: " + player2Score;
                if (player1Score > player2Score)
                {
                    ADSWinner.text = ADSWinner.text + "\nPlayer 1 Wins Overall";
                }
                else if (player1Score < player2Score)
                {
                    ADSWinner.text = ADSWinner.text + "\nPlayer 2 Wins overall";
                }
                else
                {
                    ADSWinner.text = ADSWinner.text + "\nNeither player won it was a draw overall";
                }
            }
    }
    public void BGDisplay()
    {
        if (roundCount > 0)
        {
            BGRoundTxt.text = "Round: " + roundCount;
        }
        if (roundCount == 4)
        {
            BGOutcomeTxt.text = "Player 1 got these buffs: " + bgP1BuffList + "\nPlayer 2 got these buffs:" + bgP2BuffList;
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
    public void BotStats()
    {
        if(p1AttackType)
        {
            p1AttackStat = 8;
            p1DefenseStat = 3;
            p1StaminaStat = 4;
        }
        else if(p1DefenseType)
        {
            p1AttackStat = 3;
            p1DefenseStat = 8;
            p1StaminaStat = 4;
        }
        else if(p1StaminaType) 
        {
            p1AttackStat = 4;
            p1DefenseStat = 3;
            p1StaminaStat = 8;
        }
        if (p2AttackType)
        {
            p2AttackStat = 8;
            p2DefenseStat = 3;
            p2StaminaStat = 4;
        }
        else if (p2DefenseType)
        {
            p2AttackStat = 3;
            p2DefenseStat = 8;
            p2StaminaStat = 4;
        }
        else if (p2StaminaType)
        {
            p2AttackStat = 4;
            p2DefenseStat = 3;
            p2StaminaStat = 8;
        }
        if(p1AttackStat<=0) { p1AttackStat = 0; }
        if (p1DefenseStat <= 0) { p1DefenseStat = 0; }
        if (p1StaminaStat <= 0) { p1StaminaStat = 0; }
        if (p2AttackStat <= 0) { p2AttackStat = 0; }
        if (p2DefenseStat <= 0) { p2DefenseStat = 0; }
        if (p2StaminaStat <= 0) { p2StaminaStat = 0; }
    }
    void SpawnPlayers()
    {
        // Generate a random position within the defined area
        float x1 = Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2);
        float z1 = Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2);
        float y1 = areaCenter.y+1;  // Assuming flat terrain; adjust if terrain is uneven
        float x2 = Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2);
        float z2 = Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2);
        float y2 = areaCenter.y + 1;  // Assuming flat terrain; adjust if terrain is uneven

        // Create the player instance at the generated position
        Vector3 spawnPosition1 = new Vector3(x1, y1, z1);
        P1.transform.position = spawnPosition1;

        Vector3 spawnPosition2 = new Vector3(x2, y2, z2);
        P2.transform.position = spawnPosition2;
    }
    public void EndGame()
    {

    }
}
