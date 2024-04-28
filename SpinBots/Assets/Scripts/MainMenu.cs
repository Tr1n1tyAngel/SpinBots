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
    }
    public void PvAI()
    {
        SceneManager.LoadScene("PVAI");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
