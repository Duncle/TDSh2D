using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    public static GameState Instance;

    private Dictionary<string, int> playerScores = new Dictionary<string, int>();

    private void Start()
    {
        //print("Попытка создать GameState");
        
        // Leave only 1 instance of the GameState
        if (NetworkManager.Singleton.IsServer)
        {
            StartAsServer();
        }
        else
        {
            StartServerRpc();
        }
    }

    private void StartAsServer()
    {
        if (Instance == null)
        {
            Instance = this;
            //print("Произициализировали GameState!!!");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [ServerRpc]
    private void StartServerRpc()
    {
        StartAsServer();
    }
    
    public void IncreasePlayerScore(string playerName, int pointsToAdd)
    {
        if (playerScores.ContainsKey(playerName))
        {
            playerScores[playerName] += pointsToAdd;
        }
        else
        {
            playerScores[playerName] = pointsToAdd;
        }
    }
    
    public int GetPlayerScore(string playerName)
    {
        return playerScores.ContainsKey(playerName) ? playerScores[playerName] : 0;
    }
    
    public void ShowPlayerScores()
    {
        //Scoreboard game logic
        Debug.Log("Player Scores:");

        foreach (var player in playerScores)
        {
            Debug.Log($"{player.Key}: {player.Value} points");
        }
    }
}
