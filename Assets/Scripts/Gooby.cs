using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gooby : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float size = 1f;
    Rigidbody2D rb2d;
    

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb2d.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFace();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player")
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFace();
        }
    }

    void FlipEnemyFace()
    {
        transform.localScale = size * new Vector2(-(Mathf.Sign(rb2d.velocity.x)), 1f);
    }
}
