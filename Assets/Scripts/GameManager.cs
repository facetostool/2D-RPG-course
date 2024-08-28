using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    #region singleton

    public static GameManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #endregion

    [SerializeField] private List<CheckpointController> checkpoints;
    private Player player;
    private Scene currScene;
    private void Start()
    {
        checkpoints = new List<CheckpointController>(FindObjectsOfType<CheckpointController>());
        player = PlayerManager.instance.player;
        currScene = SceneManager.GetActiveScene();
    }

    public void RestartCurrentScene() {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(currScene.name);
    }
    
    public Vector2 FindClosestCheckpoint()
    {
        Vector2 closestActiveCheckpointPosition = Vector2.zero;   
        foreach (CheckpointController checkpoint in checkpoints)
        {
            if (checkpoint.isActivated)
            {
                if (Vector2.Distance(player.transform.position, checkpoint.transform.position) <
                    Vector2.Distance(player.transform.position, closestActiveCheckpointPosition))
                {
                    closestActiveCheckpointPosition = checkpoint.transform.position;
                }
            }
        }

        return closestActiveCheckpointPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.checkpoints.Clear();
        
        data.closestCheckpoint = FindClosestCheckpoint();
        
        foreach (CheckpointController checkpoint in checkpoints)
        {
            data.checkpoints[checkpoint.id] = checkpoint.isActivated;
        }
    }

    public void LoadData(GameData data)
    {
        if (data.closestCheckpoint != Vector2.zero)
        {
            player.transform.position = data.closestCheckpoint;
        }
        
        foreach (CheckpointController checkpoint in checkpoints)
        {
            if (!data.checkpoints.TryGetValue(checkpoint.id, out var dataCheckpoint)) continue;
            if (dataCheckpoint)
                checkpoint.Activate();
        }
    }
}
