using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : NetworkBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    
    // [SerializeField] private NetworkVariable<float> _currentHealth = new NetworkVariable<float>(
    //     new float(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    // );
    //
    // [SerializeField] private NetworkVariable<float> _maxHealth = new NetworkVariable<float>(
    //     new float(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    // );

    public float RemainingHealthPrecentage
    {
        get {
            return _currentHealth / _maxHealth;
        }
    }

    public bool IsInvincible { get; set; }

    public UnityEvent OnDied;

    public UnityEvent OnDamaged;

    public UnityEvent OnHealthChanged;

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damageAmount)
    {
        // if (!IsOwner)
        // {
        //     GetComponent<NetworkObject>().ChangeOwnership(damagedCharacter.GetComponent<NetworkObject>().OwnerClientId);
        // }
        
        if (_currentHealth == 0)
        {
            return;
        }

        if (IsInvincible)
        {
            return;
        }

        _currentHealth -= damageAmount;

        OnHealthChanged.Invoke();

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
        } 
        else
        {
            OnDamaged.Invoke();
            //Получили дамаг
        }
    }

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maxHealth)
        {
            return;
        }

        _currentHealth += amountToAdd;

        OnHealthChanged.Invoke();

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
}
