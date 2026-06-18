using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

[System.Serializable]
public class RecordsList
{
    public List<RecordData> records = new List<RecordData>();
}

public class RecordsManager : MonoBehaviour
{
    private const int MAX_RECORDS = 10;
    private string savePath;

    private List<RecordData> records = new List<RecordData>();

    void Awake()
    {
#if UNITY_EDITOR
        savePath = Path.Combine(Application.dataPath, "Records.json");
#else
            savePath = Path.Combine(Application.persistentDataPath, "Records.json");
#endif

        LoadRecords();
    }

    public void LoadRecords()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);

                RecordsList recordsList = JsonUtility.FromJson<RecordsList>(json);
                if (recordsList != null && recordsList.records != null)
                {
                    records = recordsList.records;
                    SortRecords();
                }
                else
                {
                    records = new List<RecordData>();
                }
            }
            else
            {
                records = new List<RecordData>();
            }
        }
        catch (Exception e)
        {
            records = new List<RecordData>();
        }
    }

    public void SaveRecords()
    {
        try
        {
            SortRecords();

            if (records.Count > MAX_RECORDS)
            {
                records = records.Take(MAX_RECORDS).ToList();
            }

            RecordsList recordsList = new RecordsList();
            recordsList.records = records;
            string json = JsonUtility.ToJson(recordsList, true);

            File.WriteAllText(savePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Save records in file ERROR: {e.Message}");
        }
    }
    public bool AddRecord(string playerName, float time)
    {
        try
        {
            RecordData newRecord = new RecordData(playerName, time);
            records.Add(newRecord);
            SortRecords();
            SaveRecords();

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Adding record ERROR: {e.Message}");
            return false;
        }
    }
    public bool IsRecordWorthy(float time)
    {
        if (records == null || records.Count == 0)
            return true;

        if (records.Count < MAX_RECORDS)
            return true;

        float worstTime = float.MinValue;
        foreach (var record in records)
        {
            if (record.time > worstTime)
                worstTime = record.time;
        }

        return time < worstTime;
    }

    private void SortRecords()
    {
        records = records.OrderBy(r => r.time).ToList();
    }

    public List<RecordData> GetRecords()
    {
        return records;
    }

    public void ClearRecords()
    {
        records.Clear();
        SaveRecords();
    }

    [ContextMenu("Path to file")]
    void ShowFilePath()
    {
        Debug.Log($"Records File: {savePath}");
    }

    [ContextMenu("Open package with records file")]
    void OpenFolder()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(savePath);
#elif UNITY_STANDALONE_WIN
            System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{savePath}\"");
#endif
    }
}