using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSfx;

    bool hasCollected = false;

    void OnTriggerEnter2D() 
    {   
        if (!hasCollected)
        {
            AudioSource.PlayClipAtPoint(coinPickupSfx, Camera.main.transform.position);
            FindObjectOfType<GameSesion>().CollectCoin();
            hasCollected = true;
            Destroy(gameObject);
        }
    }
}
