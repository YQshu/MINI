using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CheckpointSaveManager : MonoBehaviour
{
    public static CheckpointSaveManager Instance { get; private set; }

    private string savePath;
    private HashSet<int> activatedCheckpoints = new HashSet<int>();

    public Vector2 LastRespawnPosition { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "checkpoint_save.txt");
        LoadSave();
    }

    public bool IsActivated(int checkpointId)
    {
        return activatedCheckpoints.Contains(checkpointId);
    }

    public void ActivateCheckpoint(int checkpointId, Vector2 respawnPosition)
    {
        if (activatedCheckpoints.Contains(checkpointId))
            return;

        activatedCheckpoints.Add(checkpointId);
        LastRespawnPosition = respawnPosition;
        SaveToFile();
    }

    private void LoadSave()
    {
        if (!File.Exists(savePath))
            return;

        string[] lines = File.ReadAllLines(savePath);
        foreach (string line in lines)
        {
            if (int.TryParse(line.Trim(), out int id))
                activatedCheckpoints.Add(id);
        }
    }

    private void SaveToFile()
    {
        using (StreamWriter writer = new StreamWriter(savePath, false))
        {
            foreach (int id in activatedCheckpoints)
                writer.WriteLine(id);
        }
    }
}