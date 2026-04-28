using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gamedata
{
    public int currency;

    public Serializabledictionary <string, int> inventory;
    public List<string> equipmentID;
    public Serializabledictionary <string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public Gamedata()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;


        this.currency = 0;
        inventory = new Serializabledictionary<string, int>();
        equipmentID = new List<string>();
        closestCheckpointId = string.Empty;
        checkpoints = new Serializabledictionary<string, bool>();
    }
}
