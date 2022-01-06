using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float fireSpeed = 1f;
    [SerializeField] PlayerMovementScript player;

    float xSpeed;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovementScript>();
        xSpeed = Mathf.Sign(player.transform.localScale.x) * fireSpeed;
        rigidBody.transform.localScale = new Vector2 (Mathf.Sign(player.transform.localScale.x) * rigidBody.transform.localScale.x, rigidBody.transform.localScale.y);
    }

    void Update()
    {
        rigidBody.velocity = new Vector2 (xSpeed, 0);
    }

    void OnCollisionEnter2D (Collision2D other) 
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
