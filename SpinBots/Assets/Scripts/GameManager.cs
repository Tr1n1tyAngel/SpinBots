using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject introPanel, botSelectionPanel, obstacleSelectionPanel, obsP1Ready, obsP2Ready, p1SelectedObstacle, p2SelectedObstacle, powerupSelectionPanel, powP1Ready, powP2Ready, p1SelectedPowerup, p2SelectedPowerup, ADSPanel, ADSP1Ready, ADSP2Ready, BGPanel, BGP1Ready, BGP2Ready, P1, P2;


    public TextMeshProUGUI p1ChoiceTxt, p2ChoiceTxt, ADSP1ScoreTxt, ADSP2ScoreTxt, ADSRoundTxt, ADSWinner, BGRoundTxt, BGOutcomeTxt, p1ObstacleTxt, p2ObstacleTxt, p1PowerupTxt, p2PowerupTxt, finalWinnerResultTxt;
    public float battleTimer ,p1StatsSum, p2StatsSum;
    public bool ADS1Complete, ADS2Complete, BG1Complete, BG2Complete, op1Complete, op2Complete, op3Complete, op4Complete, op5Complete, op6Complete, p1CubeRemovedFromArray, p2CubeRemovedFromArray, p1CylinderRemovedFromArray, p2CylinderRemovedFromArray, p1SphereRemovedFromArray, p2SphereRemovedFromArray, p1ChargeRemovedFromArray, p2ChargeRemovedFromArray, p1ShieldRemovedFromArray, p2ShieldRemovedFromArray, p1SpeedBoostRemovedFromArray, p2SpeedBoostRemovedFromArray,gameOver;
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

    //SpinBotController
    public float p1InitialSpinForce, p1MovementSpeed, p1CurveMagnitude, p2InitialSpinForce, p2MovementSpeed, p2CurveMagnitude, p1CurrentSpinForce, p1CurrentMovementSpeed, p1SpinDecayPerStep, p1MovementDecayPerStep, p1LastAttackStat, p1LastDefenseStat, p2CurrentSpinForce, p2CurrentMovementSpeed, p2SpinDecayPerStep, p2MovementDecayPerStep, p2LastAttackStat, p2LastDefenseStat;
    public Rigidbody p1Rigidbody, p2Rigidbody;
    public bool isP1Grounded, isP2Grounded;

    //Random player spawning
    public Transform spawnCenter;
    public float spawnRadius;
    //Powerups
    public float chargeForce;
    public bool p1Charge, p2Charge, p1Shield, p2Shield;
    //Powerup Selection
    public bool p1PowerupSelected, p2PowerupSelected, p1ChargeSelected, p1ShieldSelected, p1SpeedBoostSelected, p2ChargeSelected, p2ShieldSelected, p2SpeedBoostSelected;
    public List<GameObject> charge = new List<GameObject>(12);
    public List<GameObject> shield = new List<GameObject>(12);
    public List<GameObject> speedBoost = new List<GameObject>(12);
    //ObstacleSelection
    public bool p1ObstacleSelected, p2ObstacleSelected, p1CubeSelected, p1CylinderSelected, p1SphereSelected, p2CubeSelected, p2CylinderSelected, p2SphereSelected;
    public List<GameObject> cube = new List<GameObject>(12);
    public List<GameObject> cylinder = new List<GameObject>(12);
    public List<GameObject> sphere = new List<GameObject>(12);

    //AI
    private MinimaxAI ai;
    GameState currentState;
    Move bestMove;
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
        obstacleSelectionPanel.SetActive(false);
        obsP1Ready.SetActive(false);
        obsP2Ready.SetActive(false);
        powerupSelectionPanel.SetActive(false);
        powP1Ready.SetActive(false);
        powP2Ready.SetActive(false);
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
        p1Charge = false;
        p2Charge = false;
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
        p1InitialSpinForce = 500f;
        p1MovementSpeed = 5f;
        p2InitialSpinForce = 500f;
        p2MovementSpeed = 5f;
        chargeForce = 25f;
        spawnRadius = 4f;
        Time.timeScale = 0;
        winner = null;
        winningType = null;
        bgP1BuffList = null;
        bgP2BuffList = null;
        ADS1Complete = false;
        BG1Complete = false;
        gameOver= false;
        

        foreach (GameObject obj in cube)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        foreach (GameObject obj in cylinder)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        foreach (GameObject obj in sphere)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        ai = new MinimaxAI();
        currentState = GetCurrentGameState();
        
        
    }


    void Update()
    {
        if (battleTimer > 0)
        {
            battleTimer -= Time.deltaTime;
        }
        else
        {
            Time.timeScale = 0;
            EndGame();
        }
        if (battleTimer <= 330 && op1Complete == false && !obstacleSelectionPanel.activeSelf)
        {
            Time.timeScale = 0;
            obstacleSelectionPanel.SetActive(true);
        }
        if (battleTimer <= 300 && ADS1Complete == false && !ADSPanel.activeSelf)
        {
            bestMove = ai.GetBestMove(currentState);
            bestMove.Type = "ADS";
            ApplyMove(bestMove);
            Time.timeScale = 0;
            ADSPanel.SetActive(true);
            ADS1Complete = true;
        }
        if (battleTimer <= 270 && op2Complete == false && !obstacleSelectionPanel.activeSelf)
        {
            Time.timeScale = 0;
            obstacleSelectionPanel.SetActive(true);
        }
        if (battleTimer <= 240 && BG1Complete == false && !BGPanel.activeSelf)
        {
            bestMove = ai.GetBestMove(currentState);
            bestMove.Type = "Buff";
            ApplyMove(bestMove);
            Time.timeScale = 0;
            BGPanel.SetActive(true);
            BG1Complete = true;
        }
        if (battleTimer <= 210 && op3Complete == false && !obstacleSelectionPanel.activeSelf)
        {
            Time.timeScale = 0;
            obstacleSelectionPanel.SetActive(true);
        }
        if (battleTimer <= 180 && op4Complete == false && !obstacleSelectionPanel.activeSelf)
        {
            Time.timeScale = 0;
            obstacleSelectionPanel.SetActive(true);
        }
        if (battleTimer <= 150 && op5Complete == false && !obstacleSelectionPanel.activeSelf)
        {
            Time.timeScale = 0;
            obstacleSelectionPanel.SetActive(true);
        }
        if (battleTimer <= 120 && BG2Complete == false && !BGPanel.activeSelf)
        {
            bestMove = ai.GetBestMove(currentState);
            bestMove.Type = "Buff";
            ApplyMove(bestMove);
            Time.timeScale = 0;
            BGPanel.SetActive(true);
            BG2Complete = true;
        }
        if (battleTimer <= 90 && op6Complete == false && !obstacleSelectionPanel.activeSelf)
        {
            Time.timeScale = 0;
            obstacleSelectionPanel.SetActive(true);
        }
        if (battleTimer <= 60 && ADS2Complete == false && !ADSPanel.activeSelf)
        {
            bestMove = ai.GetBestMove(currentState);
            bestMove.Type = "ADS";
            ApplyMove(bestMove);
            Time.timeScale = 0;
            ADSPanel.SetActive(true);
            ADS2Complete = true;
        }
        if (battleTimer <=0)
        {
            EndGame();
        }

        IntroReadyCheck();
        BotSelectionCheck();
        ObstacleSelectionCheck();
        PowerupSelectionCheck();
        ADSDisplay();
        BGDisplay();
        ADSReadyCheck();
        StartCoroutine(ADSDoneCheck());
        BGReadyCheck();
        StartCoroutine(BGDoneCheck());
        StatZeroBalance();
        currentState = GetCurrentGameState();
        
            
        
    }

    public void StatZeroBalance()
    {
        if(p1AttackStat <=0)
        {
            p1AttackStat=0;
        }
        if(p2AttackStat <=0)
        {
            p2AttackStat=0;
        }
        if(p1StaminaStat <=0)
        { 
            p1StaminaStat=0;
        }
        if(p2StaminaStat<=0)
        {
            p2StaminaStat=0;
        }
        if(p1DefenseStat <=0)
        {
            p1DefenseStat=0;
        }
        if(p2DefenseStat <=0)
        {
            p2DefenseStat=0;
        }
    }
    public void IntroReadyCheck()
    {
        
            if (p1Ready && p2Ready && introPanel.activeSelf)
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
        if(p1Ready && SceneManager.GetActiveScene().name != "PVP") 
        {
            bestMove = ai.GetBestMove(currentState);
            bestMove.Type = "SpinBotSelection";
            ApplyMove(bestMove);
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
    public void ObstacleSelectionCheck()
    {
        if (p1CubeSelected && !p1CubeRemovedFromArray)
        {
            
            int rnd = Random.Range(0, cube.Count);
            Debug.Log(rnd);
            Debug.Log(cube.Count);
            

            if (!cube[rnd].activeSelf)
            {
                p1SelectedObstacle = cube[rnd];
                p1CubeRemovedFromArray = true;
            }
            p1ObstacleTxt.text = "Player Obstacle: Cube";

        }
       
        if (p2CubeSelected && !p2CubeRemovedFromArray)
        {

            int rnd = Random.Range(0, cube.Count );
            


            if (!cube[rnd].activeSelf)
            {
                p2SelectedObstacle = cube[rnd];
                p2CubeRemovedFromArray = true;
            }

            p2ObstacleTxt.text = "Player Obstacle: Cube";
        }
       
        if (p1CylinderSelected && !p1CylinderRemovedFromArray)
        {
            int rnd = Random.Range(0, cylinder.Count);
            

            if (!cylinder[rnd].activeSelf )
            {
                p1SelectedObstacle = cylinder[rnd];
                p1CylinderRemovedFromArray = true;
            }

            p1ObstacleTxt.text = "Player Obstacle: Cylinder";
        }
       
        if (p2CylinderSelected && !p2CylinderRemovedFromArray)
        {

            int rnd = Random.Range(0, cylinder.Count);
           


            if (!cylinder[rnd].activeSelf)
            {
                p2SelectedObstacle = cylinder[rnd];
                p2CylinderRemovedFromArray = true;
            }

            p2ObstacleTxt.text = "Player Obstacle: Cylinder";
        }
       
        if (p1SphereSelected && !p1SphereRemovedFromArray)
        {

            int rnd = Random.Range(0, sphere.Count);
            


            if (!sphere[rnd].activeSelf)
            {
                p1SelectedObstacle = sphere[rnd];
                p1SphereRemovedFromArray = true;
            }

            p1ObstacleTxt.text = "Player Obstacle: Sphere";
        }
        
        if (p2SphereSelected && !p2SphereRemovedFromArray)
        {

            int rnd = Random.Range(0, sphere.Count);
            

            if (!sphere[rnd].activeSelf)
            {
                p2SelectedObstacle = sphere[rnd];
                p2SphereRemovedFromArray = true;
            }

            p2ObstacleTxt.text = "Player Obstacle: Sphere";
        }
       
        if (p1Ready && p2Ready && obstacleSelectionPanel.activeSelf)
        {

            obsP1Ready.SetActive(false);
            obsP2Ready.SetActive(false);
            
            if (!op1Complete)
            {
                op1Complete = true;
            }
            else if (op1Complete && op2Complete ==false)
            {
                op2Complete = true;
            }
            else if (op2Complete && op3Complete == false)
            {
                op3Complete = true;
            }
            else if (op3Complete && op4Complete == false)
            {
                op4Complete = true;
            }
            else if (op4Complete && op5Complete == false)
            {
                op5Complete = true;
            }
            else if (op5Complete && op6Complete == false)
            {
                op6Complete = true;
            }
            obstacleSelectionPanel.SetActive(false);
            p1CubeSelected = false;
            p2CubeSelected = false;
            p1CylinderSelected = false;
            p2CylinderSelected = false;
            p1SphereSelected = false;
            p2SphereSelected = false;
            p1CubeRemovedFromArray = false;
            p2CubeRemovedFromArray = false;
            p1CylinderRemovedFromArray = false;
            p2CylinderRemovedFromArray = false;
            p1SphereRemovedFromArray = false;
            p2SphereRemovedFromArray = false;
            p1ObstacleTxt.text = null; 
            p2ObstacleTxt.text=null;
            
            p1Ready = false;
            p2Ready = false;
            powerupSelectionPanel.SetActive(true);
            
        }
        

    }
    public void PowerupSelectionCheck()
    {
        if (p1ChargeSelected && !p1ChargeRemovedFromArray)
        {
            int rnd = Random.Range(0, charge.Count);
            

            if (!charge[rnd].activeSelf)
            {
                p1SelectedPowerup = charge[rnd];
                p1ChargeRemovedFromArray = true;
            }


            p1PowerupTxt.text = "Player Powerup: Charge";
        }
       
        if (p2ChargeSelected && !p2ChargeRemovedFromArray)
        {

            int rnd = Random.Range(0, charge.Count);
            

            if (!charge[rnd].activeSelf)
            {
                p2SelectedPowerup = charge[rnd];
                p2ChargeRemovedFromArray = true;
            }
            p2PowerupTxt.text = "Player Powerup: Charge";
        }
       
        if (p1ShieldSelected && !p1ShieldRemovedFromArray)
        {

            int rnd = Random.Range(0, shield.Count);
            

            if (!shield[rnd].activeSelf)
            {
                p1SelectedPowerup = shield[rnd];
                p1ShieldRemovedFromArray = true;
            }
            p1PowerupTxt.text = "Player Powerup: Shield";
        }
        
        if (p2ShieldSelected && !p2ShieldRemovedFromArray)
        {

            int rnd = Random.Range(0, shield.Count);
            

            if (!shield[rnd].activeSelf)
            {
                p2SelectedPowerup = shield[rnd];
                p2ShieldRemovedFromArray = true;
            }

            p2PowerupTxt.text = "Player Powerup: Shield";
        }
        
        if (p1SpeedBoostSelected && !p1SpeedBoostRemovedFromArray)
        {

            int rnd = Random.Range(0, speedBoost.Count);
            

            if (!speedBoost[rnd].activeSelf)
            {
                p1SelectedPowerup = speedBoost[rnd];
                p1SpeedBoostRemovedFromArray = true;
            }
            p1PowerupTxt.text = "Player Powerup: SpeedBoost";
        }
        
        if (p2SpeedBoostSelected && !p2SpeedBoostRemovedFromArray)
        {

            int rnd = Random.Range(0, speedBoost.Count);
            
                
            if (!speedBoost[rnd].activeSelf)
            {
                p2SelectedPowerup = speedBoost[rnd];
                p2SpeedBoostRemovedFromArray = true;
            }

            p2PowerupTxt.text = "Player Powerup: SpeedBoost";
        }
       
        if (p1Ready && p2Ready && powerupSelectionPanel.activeSelf)
        {
            powP1Ready.SetActive(false);
            powP2Ready.SetActive(false);
            powerupSelectionPanel.SetActive(false);
            p1SelectedPowerup.SetActive(true);
            p2SelectedPowerup.SetActive(true);
            p1SelectedObstacle.SetActive(true);
            p2SelectedObstacle.SetActive(true);
            p1ObstacleSelected = false;
            p2ObstacleSelected = false;
            p1PowerupSelected = false;
            p2PowerupSelected = false;
            p1SelectedObstacle = null;
            p2SelectedObstacle = null;
            p1SelectedPowerup = null;
            p2SelectedPowerup = null;
            p1ChargeSelected = false;
            p2ChargeSelected = false;
            p1ShieldSelected = false;
            p2ShieldSelected = false;
            p1ChargeRemovedFromArray = false;
            p2ChargeRemovedFromArray = false;
            p1ShieldRemovedFromArray = false;
            p2ShieldRemovedFromArray = false;
            p1SpeedBoostRemovedFromArray=false;
            p2SpeedBoostRemovedFromArray=false;
            p1SpeedBoostSelected = false;
            p2SpeedBoostSelected = false;
            p1PowerupTxt.text = null;
            p2PowerupTxt.text = null;
            
            Time.timeScale = 1.0f;
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

            Time.timeScale = 1f; 
            p1Ready = false;
            p2Ready = false;
            winner = "";
            winningType = "";
            roundCount = 1;
            player1Score = 0;
            player2Score = 0;
            ADSWinner.text = "";

        }
    }
    IEnumerator BGDoneCheck()
    {
        if (p1Ready && p2Ready && BGPanel.activeSelf && bgRoundCount == 4)
        {
            yield return new WaitForSecondsRealtime(3f);

            BGPanel.SetActive(false);
            Time.timeScale = 1f;
            p1Ready = false;
            p2Ready = false;
            bgRoundCount = 1;
            bgP1BuffList = null;
            bgP2BuffList = null;
            BGOutcomeTxt.text = "";


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
        if (bgRoundCount > 0)
        {
            BGRoundTxt.text = "Round: " + bgRoundCount;
        }
        if (bgRoundCount == 4)
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
        float angle1 = Random.Range(0, Mathf.PI * 2); // Random angle
        float x1 = Mathf.Cos(angle1) * spawnRadius;
        float z1 = Mathf.Sin(angle1) * spawnRadius;
        Vector3 spawnPosition1 = spawnCenter.position + new Vector3(x1, 0.5f, z1);
        P1.transform.position = spawnPosition1;

        float angle2 = Random.Range(0, Mathf.PI * 2); // Random angle
        float x2 = Mathf.Cos(angle2) * spawnRadius;
        float z2 = Mathf.Sin(angle2) * spawnRadius;
        Vector3 spawnPosition2 = spawnCenter.position + new Vector3(x2, 0.5f, z2);
        P2.transform.position = spawnPosition2;
    }
    public void EndGame()
    {
        gameOver = true;
        Time.timeScale = 0;
        if(battleTimer<=0)
        {
            p1StatsSum = System.MathF.Round(p1AttackStat + p1StaminaStat + p1DefenseStat, 2);
            p2StatsSum = System.MathF.Round(p2AttackStat + p2StaminaStat + p2DefenseStat, 2);
            if (p1StatsSum > p2StatsSum)
            {
                finalWinnerResultTxt.text = "Player 1 Remaining Stat Total: " + p1StatsSum + "\nPlayer 2 Remaining StatTotal: " + p2StatsSum + "\nPlayer 1 Wins";
            }
            else if (p1StatsSum < p2StatsSum)
            {
                finalWinnerResultTxt.text = "Player 1 Remaining Stat Total: " + p1StatsSum + "\nPlayer 2 Remaining StatTotal: " + p2StatsSum + "\nPlayer 2 Wins";
            }
            else
            {
                finalWinnerResultTxt.text = "Player 1 Remaining Stat Total: " + p1StatsSum + "\nPlayer 2 Remaining StatTotal: " + p2StatsSum + "\nIts a draw";
            }
        }
        
        StartCoroutine(BackToMainMenu());
    }

    public IEnumerator BackToMainMenu()
    {
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene("MainMenu");
    }

    private GameState GetCurrentGameState()
    {
        // Get the current game state
        GameState state = new GameState
        {
            p1AttackStatAI = p1AttackStat,
            p1DefenseStatAI = p1DefenseStat,
            p1StaminaStatAI = p1StaminaStat,
            p2AttackStatAI = p2AttackStat,
            p2DefenseStatAI = p2DefenseStat,
            p2StaminaStatAI = p2StaminaStat,
            p1BotSelectedAI = p1BotSelected,
            p2BotSelectedAI = p2BotSelected,
            p1ChoiceAI = player1Choice,
            p2ChoiceAI = player2Choice,
            gameOverAI = gameOver
        };

        return state;
    }

    private void ApplyMove(Move move)
    {
        switch (move.Type)
        {
            case "SpinBotSelection":
                if (move.Choice == "Attack")
                {
                    p2AttackType = true;
                    p2DefenseType = false;
                    p2StaminaType = false;
                    p2BotSelected = true;
                }
                else if (move.Choice == "Defense")
                {
                    p2AttackType = false;
                    p2DefenseType = true;
                    p2StaminaType = false;
                    p2BotSelected = true;
                }
                else if (move.Choice == "Stamina")
                {
                    p2AttackType = false;
                    p2DefenseType = false;
                    p2StaminaType = true;
                    p2BotSelected = true;
                }
                p2Ready = true;
                break;

            case "ADS":
                p2ChoiceTxt.text = "Player 2 chooses " + move.Choice;
                player2Choice = move.Choice;
                p2Ready = true;
                break;

            case "Buff":
                p2ChoiceTxt.text = "Player 2 chooses " + move.Choice;
                player2Choice = move.Choice;
                p2Ready = true;
                break;
        }
       
    }
}
