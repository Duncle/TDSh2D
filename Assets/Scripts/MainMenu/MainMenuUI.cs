using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI: MonoBehaviour
{
    [SerializeField] private Button _createButon;
    [SerializeField] private Button _joinButon;
    [SerializeField] private Button _lobbyListButon;
    [SerializeField] private Button _changeNameButon;
    [SerializeField] private GameObject _menuButtons;
    [SerializeField] private GameObject _lobbyCreateWindow;
    [SerializeField] private GameObject _lobbyJoinWindow;
    [SerializeField] private GameObject _lobbyNameErrorModalWindow;
    [SerializeField] private GameObject _changeNameModalWindow;

    [SerializeField] private TMP_InputField _lobbyNameInput;
    [SerializeField] private Button _lobbyCreateButton;

    [SerializeField] private TMP_InputField _lobbyToJoinInput;
    [SerializeField] private Button _lobbyJoinButton;
    
    [SerializeField] private Button _lobbyNameErrorCloseButton;
    
    [SerializeField] private TMP_InputField _newNameInput;
    [SerializeField] private Button _saveNameButton;
    
    private string _lobbyName;

    private void Awake()
    {
        _createButon.onClick.AddListener(CreateButtonClick);
        _joinButon.onClick.AddListener(JoinButtonClick);
        _lobbyListButon.onClick.AddListener(LobbyListButtonClick);
        _changeNameButon.onClick.AddListener(ChangeNameButtonClick);
        _lobbyCreateButton.onClick.AddListener(LobbyCreateButtonClick);
        _lobbyJoinButton.onClick.AddListener(JoinLobbyButtonClick);
        _lobbyNameErrorCloseButton.onClick.AddListener(CloseLobbyNameErrorModal);
        _saveNameButton.onClick.AddListener(SaveNameErrorModal);
        
        EventManager.WrongLobbyNameEntered += ShowLobbyNameErrorModal;
    }

    private void CreateButtonClick()
    {
        _lobbyCreateWindow.SetActive(true);
        _menuButtons.SetActive(false);

        //DataHolder.СurrentPlayerType = DataHolder.PlayerType.hostPlayer;
    }

    private void JoinButtonClick()
    {
        _lobbyJoinWindow.SetActive(true);
        _menuButtons.SetActive(false);
        //DataHolder.СurrentPlayerType = DataHolder.PlayerType.clientPlayer;
    }

    private void LobbyListButtonClick()
    {
        
        //DataHolder.СurrentPlayerType = DataHolder.PlayerType.clientPlayer;
    }

    private void LobbyCreateButtonClick()
    {
        _lobbyName = _lobbyNameInput.text;
        GameLobby gameLobby = GetComponent<GameLobby>();
        gameLobby.CreateLobby(_lobbyName);
        SceneManager.LoadScene("3_Game");
    }

    private void JoinLobbyButtonClick()
    {
        GameLobby gameLobby = GetComponent<GameLobby>();
        gameLobby.JoinLobbyByName(_lobbyToJoinInput.text);
        SceneManager.LoadScene("3_Game");
    }

    private void ChangeNameButtonClick()
    {
        _changeNameModalWindow.SetActive(true);
    }
    
    private void ShowLobbyNameErrorModal()
    {
        _lobbyNameErrorModalWindow.SetActive(true);
    }
    
    private void CloseLobbyNameErrorModal()
    {
        _lobbyNameErrorModalWindow.SetActive(false);
    }
    
    private void SaveNameErrorModal()
    {
        GameLobby gameLobby = GetComponent<GameLobby>();
        gameLobby.SetPlayerName(_newNameInput.text);
        _changeNameModalWindow.SetActive(false);
    }
}
