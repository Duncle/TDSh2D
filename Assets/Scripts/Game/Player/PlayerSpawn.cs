using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    private Transform spawnPoint1;
    private Transform spawnPoint2;
    
    private static NetworkVariable<bool> isSpawnPoint1Occupied = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private static NetworkVariable<bool> isSpawnPoint2Occupied = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        spawnPoint1 = GameObject.Find("spawnpoint1").transform;
        spawnPoint2 = GameObject.Find("spawnpoint2").transform;
        SpawnPlayer();
    }
    
    public void SpawnPlayer()
    {
        // print("Я сервер? " + NetworkManager.Singleton.IsServer
        // + ". Точка 1 занята? " + isSpawnPoint1Occupied.Value 
        // + ". Точка 2 занята? " + isSpawnPoint2Occupied.Value);
        if (!IsOwner) return;
        //Transform[] spawnPoints = new Transform[2] {spawn1Transform, spawn2Transform};
        Transform spawnPoint = GetAvailableSpawnPoint();
        //print("Точка спавна до if: " + spawnPoint.position);
        if (spawnPoint != null)
        {
            //print("Точка спавна " + spawnPoint.position);
            Transform playerTransform = GetComponent<Transform>();
            
            // Устанавливаем флаг для выбранной точки, что она занята
            if (spawnPoint == spawnPoint1)
            {
                playerTransform.position = spawnPoint1.position;
                isSpawnPoint1Occupied.Value = true;
            }
            else if (spawnPoint == spawnPoint2)
            {
                playerTransform.position = spawnPoint2.position;
                isSpawnPoint2Occupied.Value = true;
            }
        }
        
    }
    
    private Transform GetAvailableSpawnPoint()
    {
        // Проверяем, какая точка свободна и возвращаем её
        if (!isSpawnPoint1Occupied.Value)
        {
            return spawnPoint1;
        }
        else if (!isSpawnPoint2Occupied.Value)
        {
            return spawnPoint2;
        }

        // Если обе точки заняты, возвращаем null
        return null;
    }
}
