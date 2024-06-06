using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject gamemodePanel;
    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(true);
        gamemodePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameStart()
    {
        menuPanel.SetActive(false);
        gamemodePanel.SetActive(true);
    }
    public void PvP()
    {
        SceneManager.LoadScene("PVP");
        if (SceneManager.GetActiveScene().name == "PVP")
        {
            Debug.Log("PVP scene is active");
        }
    }
    public void PvAI()
    {
        SceneManager.LoadScene("PvAI");
        if (SceneManager.GetActiveScene().name == "PVAI")
        {
            Debug.Log("PvAI scene is active");
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
