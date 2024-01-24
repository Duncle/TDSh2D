using QFSW.QC;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    private static Lobby _hostLobby;
    private static Lobby _joinedLobby;
    private float _heartbeatTimer;
    private float _lobbyUpdateTimer;
    private static string _playerName;

    public string PlayerName
    {
        get { return _playerName; }
    }
    
    public void SetPlayerName(string newName)
    {
        _playerName = newName;
        print("Имя игрока изменено на " + _playerName);
    }
    
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (AuthenticationService.Instance.IsAuthorized == true)
        {
            return;
        }

        AuthenticationService.Instance.SignedIn += () =>
        {
            //print("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        _playerName = "Player " + UnityEngine.Random.Range(10, 99);
        //print(_playerName);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdate();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (_hostLobby != null)
        {
            _heartbeatTimer -= Time.deltaTime;
            if (_heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                _heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdate()
    {
        if (_joinedLobby != null)
        {
            _lobbyUpdateTimer -= Time.deltaTime;
            if (_lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimer = 10.1f;
                _lobbyUpdateTimer = lobbyUpdateTimer;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id);
                _joinedLobby = lobby;
            }
        }
    }

    [Command]
    public async void CreateLobby(string lobbyName)
    {
        try
        {
            int maxPlayers = 2;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag") },
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "Factory") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            _hostLobby = lobby;
            _joinedLobby = _hostLobby;
            NetworkManager.Singleton.StartHost();

            //print("Created lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            PrintPlayers(_hostLobby);
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }

    [Command]
    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            print("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                print(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
            }

        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }

    public async void JoinLobbyByName(string lobbyName)
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.Name, lobbyName, QueryFilter.OpOptions.EQ)
                },
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            if (lobbyName == queryResponse.Results[0].Name)
            {
                
                JoinLobbyById(queryResponse.Results[0].Id);
                NetworkManager.Singleton.StartClient();

            }
            else
            {
                EventManager.OnWrongLobbyNameEntered();
            }
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }

    [Command]
    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            _joinedLobby = lobby;

            print("Joined Lobby with code " + lobbyCode);

            PrintPlayers(_joinedLobby);
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }
    
    [Command]
    public async void JoinLobbyById(string lobbyId)
    {
        try
        {
            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer(),
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, joinLobbyByIdOptions);
            _joinedLobby = lobby;

            print("Joined Lobby with code " + lobbyId);

            PrintPlayers(_joinedLobby);
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }

    [Command]
    private async void QuickJoinLobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (Exception e)
        {

            print(e);
        }

    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, _playerName) }
                    }
        };
    }

    [Command]
    private void PrintPlayers()
    {
        PrintPlayers(_joinedLobby);
    }

    private void PrintPlayers(Lobby lobby)
    {
        print("Players in lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);
        foreach (Player player in lobby.Players)
        {
            print(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    [Command]
    private async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            _joinedLobby = _hostLobby;

            PrintPlayers(_hostLobby);
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    [Command]
    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            _playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, _playerName) }
                }
            });
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    [Command]
    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (Exception e)
        {

            print(e);
        }
    }

    [Command]
    private async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, _joinedLobby.Players[1].Id);
        }
        catch (Exception e)
        {

            print(e);
        }
    }

    [Command]
    private async void MigrateLobbyHost()
    {
        try
        {
            _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = _joinedLobby.Players[1].Id
            });
            _joinedLobby = _hostLobby;

            PrintPlayers(_hostLobby);
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    [Command]
    private async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);
        }
        catch (Exception e)
        {

            print(e);
        }
    }
}
