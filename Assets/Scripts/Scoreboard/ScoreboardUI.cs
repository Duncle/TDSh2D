using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    [SerializeField] private GameObject _ScoreboardUI;
    
    private void ShowScoreUI()
    {
        GameState.Instance.ShowPlayerScores();
        _ScoreboardUI.SetActive(true);
    }
    
    private void CloseScoreUI()
    {
        _ScoreboardUI.SetActive(false);
    }
}
