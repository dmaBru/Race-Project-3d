using UnityEngine;
using System;

[Serializable]
public class RecordData
{
    public string playerName;
    public float time;
    public string formattedTime;

    public RecordData(string name, float timeValue)
    {
        playerName = name;
        time = timeValue;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int millisecond = Mathf.FloorToInt((time * 100f) % 100f);

        formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, millisecond);
    }
}
