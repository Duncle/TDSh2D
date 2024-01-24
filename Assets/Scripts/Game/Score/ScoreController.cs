using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class ScoreController : NetworkBehaviour
{
    public UnityEvent OnScoreChanged;
    public int Score { get; private set; }

    public void AddScore(int amount)
    {
        //GameLobby lobby = GameObject.Find("GameLobby").GetComponent<GameLobby>();
        if (GetComponent<ScoreController>().IsLocalPlayer)
        {
            Score += amount;
            
            if (NetworkManager.Singleton.IsServer)
            {
                AddScoreAsServer(amount);
            }
            else
            {
                AddScoreServerRpc(amount);
            }
            //GameState.Instance.IncreasePlayerScore(lobby.PlayerName, amount);
            OnScoreChanged.Invoke();
            //GameState.Instance.ShowPlayerScores();
        }
    }

    private void AddScoreAsServer(int amount)
    {
        // GameLobby lobby = GameObject.Find("GameLobby").GetComponent<GameLobby>();
        // GameState.Instance.IncreasePlayerScore(lobby.PlayerName, amount);
        // GameState.Instance.ShowPlayerScores();
    }

    [ServerRpc]
    private void AddScoreServerRpc(int amount)
    {
        AddScoreAsServer(amount);
    }
}
