using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthBarUI : NetworkBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _healthBarForegroundImage;

    public void Start()
    {
        if (!GetComponent<HealthBarUI>().IsLocalPlayer)
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateHealthBar(HealthController healthController)
    {
        print("Осталось хп: " + healthController.RemainingHealthPrecentage);
        //private UnityEngine.UI.Image _healthBarForegroundImageForChange = healthController.
        _healthBarForegroundImage.fillAmount = healthController.RemainingHealthPrecentage;
    }
}
