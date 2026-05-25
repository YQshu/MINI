using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Savemanage : MonoBehaviour
{
    public static Savemanage instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private Gamedata gamedata;
    private List<InterfaceSavemanager> Savemanages;
    private FileDataHandle dataHandle;

    [ContextMenu("Delete Save file")]
    public void DeleteSavedData()
    {
        dataHandle = new FileDataHandle(Application.persistentDataPath, fileName , encryptData);
        dataHandle.Delete();
    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        dataHandle = new FileDataHandle(Application.persistentDataPath, fileName , encryptData);
        Debug.Log("Path:" + Application.persistentDataPath);
        Savemanages = FindAllSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gamedata = new Gamedata();
    }
    public void LoadGame()
    {
        gamedata = dataHandle.Load();

        if(this.gamedata == null)
        {
            NewGame(); 
        }

        foreach(InterfaceSavemanager savemanager in Savemanages)
        {
            savemanager.LoadData(gamedata);
        }
    }
    public void SaveGame()
    {
        foreach (InterfaceSavemanager savemanager in Savemanages)
        {
            savemanager.SaveData(ref gamedata);
        }
        dataHandle.Save(gamedata);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<InterfaceSavemanager>FindAllSaveManagers()
    {
        IEnumerable<InterfaceSavemanager> savemanagers = FindObjectsOfType<MonoBehaviour>().OfType<InterfaceSavemanager>();

        return new List<InterfaceSavemanager>(savemanagers);
    }
    public bool HasSaveData()
    {
        if (dataHandle.Load() != null)
        {
            return true;
        }
        return false;
    }
}
