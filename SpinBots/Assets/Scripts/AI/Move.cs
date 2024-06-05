using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public string Type; // e.g., "BeybladeSelection", "ObstacleSelection", "PowerupSelection", "ADS", "Buff"
    public string Choice; // e.g., "Attack", "Defense", "Stamina", "Cube", "Shield"

    public Move(string type, string choice)
    {
        Type = type;
        Choice = choice;
    }
}

