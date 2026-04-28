using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour , InterfaceSavemanager
{
    public static PlayerManager Instance;
    public Player player;
    public int currency;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

 
    /*
    private static PlayerManager _instance;
    public Player player;


    public static PlayerManager Instance
    {
        get
        {
            // 如果单例还没初始化，自动查找/创建
            if (_instance == null)
            {
                // 第一步：从场景中查找已有PlayerManager
                _instance = FindObjectOfType<PlayerManager>();

                // 第二步：如果场景中没有，自动创建一个GameObject并挂载
                if (_instance == null)
                {
                    GameObject managerObj = new GameObject("PlayerManager");
                    _instance = managerObj.AddComponent<PlayerManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保留
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // 自动获取Player，防面板漏拖
        if (player == null)
            player = FindObjectOfType<Player>();
    }
    */

    public int CurrentCurrencyAmount() => currency;

    public void LoadData(Gamedata _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref Gamedata _data)
    {
        _data.currency = this.currency;
    }
}

