using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float climbingSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float secondJumpSpeed = 10f;
    [SerializeField] GameObject fire;
    [SerializeField] Transform hand;
    [SerializeField] AudioClip fireSfx;
    [SerializeField] AudioClip deathSfx;

    Vector2 moveInput;
    Rigidbody2D rigidBody;
    Animator animator;
    CapsuleCollider2D playerCollider;
    BoxCollider2D bPlayerCollider;

    bool isAlive = true;
    
    float gravity;
    bool isJumpedTwice = true;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        bPlayerCollider = GetComponent<BoxCollider2D>();
        gravity = rigidBody.gravityScale;
    }
    void Update()
    {
        if (!isAlive) {return;}
        Climb();
        Run();
        FlipSprite();
        IsTouchedGround();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) {return;} 
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) {return;} 
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            if (value.isPressed)
            {
                if (bPlayerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
                {
                    Vector2 playerVelocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                    rigidBody.velocity = playerVelocity;
                }
                else if (!isJumpedTwice)
                {
                    isJumpedTwice = true;
                    Vector2 playerVelocity = new Vector2(rigidBody.velocity.x, secondJumpSpeed);
                    rigidBody.velocity = playerVelocity;
                }
            }
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) {return;} 
        if (value.isPressed)
        {
            if (FindObjectOfType<GameSesion>().ThrowFire())
            {
                Instantiate(fire, hand.position, hand.rotation);
                AudioSource.PlayClipAtPoint(fireSfx, Camera.main.transform.position);
            }
        }
    }

    void OnEscape(InputValue value)
    {
        if (value.isPressed)
        {
            Application.Quit();
        }       
    }

    void Run()
    {
        float moveSpeed;
        if (Mathf.Abs(moveInput.x) > Mathf.Epsilon)
        {
            moveSpeed = playerSpeed * Mathf.Sign(moveInput.x);
            SetIsRunning(true);
        }
        else
        {
            moveSpeed = 0f;
            SetIsRunning(false);
        }
        Vector2 playerVelocity = new Vector2(moveSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;
    }

    void Climb()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rigidBody.gravityScale = 0;
            float currentSpeed;
            if (Mathf.Abs(moveInput.y) > Mathf.Epsilon)
            {
                currentSpeed = climbingSpeed * Mathf.Sign(moveInput.y);
                SetIsClimbing(true);
                SetisClimbingStill(false);
            }
            else
            {
                currentSpeed = 0f;
                if (!bPlayerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
                {
                    SetIsClimbing(true);
                    SetisClimbingStill(true);
                }
            }
            Vector2 playerVelocity = new Vector2(rigidBody.velocity.x, currentSpeed);
            rigidBody.velocity = playerVelocity;
        } 
        else
        {
            rigidBody.gravityScale = gravity;
            SetIsClimbing(false);
            SetisClimbingStill(false);
        }
    }

    void FlipSprite()
    {
        bool playerHasMovement = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        
        if (playerHasMovement)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rigidBody.velocity.x), 1f);        
        }
    }

    void SetIsRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    void SetIsClimbing(bool isClimbing)
    {
        animator.SetBool("isClimbing", isClimbing);
    }

    void SetisClimbingStill(bool isClimbingStill)
    {
        animator.SetBool("isClimbingStill", isClimbingStill);
    }

    void Kill()
    {
        animator.SetTrigger("dying");
    }

    void IsTouchedGround()
    {
        if (bPlayerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isJumpedTwice = false;
            SetIsClimbing(false);
        }
    }

    void Die()
    {
        if (bPlayerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazard")) || playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazard")))
        {
            isAlive = false;
            AudioSource.PlayClipAtPoint(deathSfx, Camera.main.transform.position);
            Kill();
            FindObjectOfType<GameSesion>().DoOnDeath();
            rigidBody.velocity = new Vector2(Random.Range(-15f, 15f), Random.Range(10f, 20f));
        }
    }
}
