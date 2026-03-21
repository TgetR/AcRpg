using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    int attempCounter = 0;
    public int GetAttempCount() { return attempCounter; }
    double hoursPlayed = 0;
    float oldHoursPlayed = 0;
    public int GetHoursPlayed() { return (int)hoursPlayed; }

    void Start()
    {
        attempCounter = PlayerPrefs.GetInt("Attempts", 0);
        oldHoursPlayed = PlayerPrefs.GetFloat("HoursPlayed", 0f);
    }
    void Update()
    {
        hoursPlayed = Math.Round(Time.time / 3600f, 2) ; // Convert seconds to hours
    }

    public void EndOfGame()
    {
        attempCounter++;
        hoursPlayed += oldHoursPlayed;
        PlayerPrefs.SetInt("Attempts", attempCounter);
        PlayerPrefs.SetFloat("HoursPlayed", (float)hoursPlayed);
        PlayerPrefs.Save();
    }
}
