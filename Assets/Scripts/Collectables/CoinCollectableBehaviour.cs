using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectableBehaviour : MonoBehaviour, ICollectableBehaviour
{
    [SerializeField] private int _scoreAmount;

    public void OnCollected(GameObject player)
    {
        player.GetComponent<ScoreController>().AddScore(_scoreAmount);
    }
}
