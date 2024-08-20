using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
  public int currency;
  public SerializableDictionary<string, int> items;
  public List<string> equipmentGUIDs;
  public SerializableDictionary<string, bool> skills;

  public SerializableDictionary<string, bool> checkpoints;
  public Vector2 closestCheckpoint;
  
  public Vector2 lastDeadLightPosition;
  public int lastDeadLightCurrency;
  
  public GameData()
  {
    this.currency = 0;
    this.items = new SerializableDictionary<string, int>();
    this.equipmentGUIDs = new List<string>();
    this.skills = new SerializableDictionary<string, bool>();
    this.checkpoints = new SerializableDictionary<string, bool>();
    this.closestCheckpoint = Vector2.zero;
    this.lastDeadLightPosition = Vector2.zero;
    this.lastDeadLightCurrency = 0;
  }
}
