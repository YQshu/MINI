using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour , InterfaceSavemanager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

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

    private void Start()
    {
       checkpoints = FindObjectsOfType<Checkpoint>();

        player = PlayerManager.Instance.player.transform;
    }


    public void RestartGame()
    {
        Savemanage.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(Gamedata _data) => StartCoroutine(LoadWithDelay(_data));

    private void LoadCheckPoint(Gamedata _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.checkpointId == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
    }

    private void LoadLostCurrency(Gamedata _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if(lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrency>().currency = lostCurrencyAmount;
        }
        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(Gamedata _data)
    {
        yield return new WaitForSeconds(.1f);
        LoadCheckPoint(_data);
        LoadClosestCheckPoint(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref Gamedata _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if (FindCloestCheckPoint() != null)
        {
            _data.closestCheckpointId = FindCloestCheckPoint().checkpointId;
        }
        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.checkpointId, checkpoint.activationStatus);
        }
    }
    private void LoadClosestCheckPoint(Gamedata _data)
    {
        if (_data.closestCheckpointId == null)
        {
            return;
        }
        closestCheckpointId = _data.closestCheckpointId;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.checkpointId)
            {
                player.position = checkpoint.transform.position;
            }
        }
    }

    private Checkpoint FindCloestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach(var checkpoint in checkpoints)
        {
            float distancToCloestCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if(distancToCloestCheckpoint < closestDistance &&   checkpoint.activationStatus == true)
            {
                closestDistance = distancToCloestCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }
        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if(_pause)
        {
            Time.timeScale = 0;
        }else
        {
            Time.timeScale = 1;
        }
    }
}