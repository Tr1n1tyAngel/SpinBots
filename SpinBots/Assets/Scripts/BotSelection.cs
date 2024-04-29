using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSelection : MonoBehaviour
{
    public GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && gameManager.p1AttackType==false)
        {
            gameManager.p1AttackType = true;
            gameManager.p1DefenseType = false;
            gameManager.p1StaminaType = false;
            gameManager.p1BotSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && gameManager.p1DefenseType == false)
        {
            gameManager.p1AttackType = false;
            gameManager.p1DefenseType = true;
            gameManager.p1StaminaType = false;
            gameManager.p1BotSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.D) && gameManager.p1StaminaType == false)
        {
            gameManager.p1AttackType = false;
            gameManager.p1DefenseType = false;
            gameManager.p1StaminaType = true;
            gameManager.p1BotSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.J) && gameManager.p2AttackType == false)
        {
            gameManager.p2AttackType = true;
            gameManager.p2DefenseType = false;
            gameManager.p2StaminaType = false;
            gameManager.p2BotSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.K) && gameManager.p2DefenseType == false)
        {
            gameManager.p2AttackType = false;
            gameManager.p2DefenseType = true;
            gameManager.p2StaminaType = false;
            gameManager.p2BotSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.L) && gameManager.p2StaminaType == false)
        {
            gameManager.p2AttackType = false;
            gameManager.p2DefenseType = false;
            gameManager.p2StaminaType = true;
            gameManager.p2BotSelected = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && gameManager.p1BotSelected )
        {
            gameManager.p1Ready = true;
        }
        if (Input.GetKeyDown(KeyCode.O) && gameManager.p2BotSelected)
        {
            gameManager.p2Ready = true;
        }
    }
}
