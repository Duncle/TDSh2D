using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _bulletDamage;

    private Camera _camera;
    private HealthController _healthController;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        //Only for non-closed areas
        DestroyWhenOffScreen();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>())
        {
            _healthController = collision.GetComponent<HealthController>();
            _healthController.TakeDamageServerRpc(_bulletDamage);
            Destroy(gameObject);
        } else if (collision.GetComponent<BoxCollider2D>() || collision.GetComponent<CircleCollider2D>())
        {
            Destroy(gameObject);
        }
    }

    private void DestroyWhenOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        
        if (screenPosition.x < 0 ||
            screenPosition.x > _camera.pixelWidth ||
            screenPosition.y < 0 ||
            screenPosition.y > _camera.pixelHeight)
        {
            Destroy(gameObject);
            //StartCoroutine(DelayedDestroy());
        }
    }
    
    
    // private IEnumerator DelayedDestroy()
    // {
    //     // Подождать 1 секунду
    //     yield return new WaitForSeconds(1.0f);
    //
    //     // Уничтожить объект
    //     Destroy(gameObject);
    // }
}
