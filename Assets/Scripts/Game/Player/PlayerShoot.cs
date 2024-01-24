using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private float _bulletSpeed;

    [SerializeField] private Transform _gunOffset;

    [SerializeField] private float _timeBetweenShots;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootingAudioClip;
   
    private float _lastFireTime;
    private bool _fireSingle;
    private bool _fireContinuously;

    void Update()
    {
        if (_fireContinuously || _fireSingle)
        {
            float timeSinceLastFire = Time.time - _lastFireTime;

            if (timeSinceLastFire >= _timeBetweenShots)
            {
                FireBullet();
                _audioSource.PlayOneShot(_shootingAudioClip);
                
                _lastFireTime = Time.time;
                _fireSingle = false;
            }          
        }
    }

    //[ServerRpc(RequireOwnership = false)]
    //[ClientRpc]
    private void FireBullet()
    {
         if (GetComponent<PlayerShoot>().IsLocalPlayer)
         {
            if (NetworkManager.Singleton.IsServer)
            {
                SpawnBullet();
            }
            else
            {
                FireBulletServerRpc();
            }
         }
    }

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, transform.rotation);
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.velocity = _bulletSpeed * transform.up;
    }
    
    [ServerRpc]
    private void FireBulletServerRpc()
    {
        SpawnBullet();
    }

    private void OnFire(InputValue inputValue)
    {
        _fireContinuously = inputValue.isPressed;

        if (inputValue.isPressed)
        {
            _fireSingle = true;
        }
    }
}
