using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CheckpointSaveManager : MonoBehaviour
{
    public static CheckpointSaveManager Instance { get; private set; }

    private string savePath;
    private HashSet<int> activatedCheckpoints = new HashSet<int>();
    private Dictionary<int, Vector2> checkpointPositions = new Dictionary<int, Vector2>();

    public Vector2 LastRespawnPosition { get; private set; }
    public bool HasSavedPosition { get; private set; }

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

    public Vector2? GetCheckpointPosition(int checkpointId)
    {
        if (checkpointPositions.TryGetValue(checkpointId, out Vector2 pos))
            return pos;
        return null;
    }

    public void ActivateCheckpoint(int checkpointId, Vector2 respawnPosition)
    {
        if (activatedCheckpoints.Contains(checkpointId))
            return;

        activatedCheckpoints.Add(checkpointId);
        checkpointPositions[checkpointId] = respawnPosition;
        LastRespawnPosition = respawnPosition;
        HasSavedPosition = true;
        SaveToFile();
    }

    private void LoadSave()
    {
        if (!File.Exists(savePath))
            return;

        string[] lines = File.ReadAllLines(savePath);
        int lastId = -1;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');
            if (parts.Length >= 3 && int.TryParse(parts[0].Trim(), out int id)
                && float.TryParse(parts[1].Trim(), out float x)
                && float.TryParse(parts[2].Trim(), out float y))
            {
                activatedCheckpoints.Add(id);
                checkpointPositions[id] = new Vector2(x, y);
                lastId = id;
            }
        }

        if (lastId >= 0)
        {
            LastRespawnPosition = checkpointPositions[lastId];
            HasSavedPosition = true;
        }
    }

    private void SaveToFile()
    {
        using (StreamWriter writer = new StreamWriter(savePath, false))
        {
            foreach (int id in activatedCheckpoints)
            {
                Vector2 pos = checkpointPositions[id];
                writer.WriteLine($"{id},{pos.x},{pos.y}");
            }
        }
    }

    public void ClearSave()
    {
        activatedCheckpoints.Clear();
        checkpointPositions.Clear();
        LastRespawnPosition = Vector2.zero;
        HasSavedPosition = false;

        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
