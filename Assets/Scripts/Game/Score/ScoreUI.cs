using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScoreUI : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _scorePointsUI;

    public void Start()
    {
        if (!GetComponent<ScoreUI>().IsLocalPlayer)
        {
            gameObject.SetActive(false);
        }
    }
    
    public void UpdatePlayerScore(ScoreController scoreController)
    {
        _scorePointsUI.text = "Coins: " + scoreController.Score.ToString();
    }
}
