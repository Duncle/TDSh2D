using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action WrongLobbyNameEntered;
    public static event Action HostConnected;
    public static event Action ClientConnected;

    public static void OnWrongLobbyNameEntered()
    {
        WrongLobbyNameEntered?.Invoke();
    }
    
    public static void OnHostConnected()
    {
        HostConnected?.Invoke();
    }
    
    public static void OnClientConnected()
    {
        ClientConnected?.Invoke();
    }
}
