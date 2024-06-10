using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public TMP_Dropdown difficultyDropdown;
    public GameManager gameManager;

    private void Start()
    {
        // Ensure the dropdown is populated and set its default value
        difficultyDropdown.ClearOptions();
        difficultyDropdown.AddOptions(new List<string> { "Easy", "Normal", "Hard" });
        difficultyDropdown.value = 1; // Set "Normal" as default
        difficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);

        // Set the initial difficulty
        SetDifficulty("Normal");
    }

    private void OnDifficultyChanged(int index)
    {
        string difficulty = difficultyDropdown.options[index].text;
        SetDifficulty(difficulty);
    }

    private void SetDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                gameManager.ai.SetDepth(1);
                break;
            case "Normal":
                gameManager.ai.SetDepth(3);
                break;
            case "Hard":
                gameManager.ai.SetDepth(5);
                break;
        }
    }
}