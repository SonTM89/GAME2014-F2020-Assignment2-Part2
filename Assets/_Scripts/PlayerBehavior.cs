using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Control")]
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;

    [Header ("Platform Detection")]
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public Transform spawnPoint;

    [Header("Player Abilities")]
    public int health;
    public int lives;
    public BarController healthBar;
    //public Animator livesHUD;

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Move();
    }

    private void _Move()
    {
        if (isGrounded)
        {
            if (!isJumping && !isCrouching)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    // move right
                    m_rigidBody2D.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = false;
                    m_animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    m_rigidBody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = true;
                    m_animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);
                }
                else
                {
                    m_animator.SetInteger("AnimState", (int)PlayerAnimationType.IDLE);
                }
            }

            if ((joystick.Vertical > joystickVerticalSensitivity) && (!isJumping))
            {
                // jump
                m_rigidBody2D.AddForce(Vector2.up * verticalForce);
                m_animator.SetInteger("AnimState", (int)PlayerAnimationType.JUMP);
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }

            if ((joystick.Vertical < -joystickVerticalSensitivity))
            {
                // crouch
                m_animator.SetInteger("AnimState", (int)PlayerAnimationType.CROUCH);
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platforms"))
        {
            isGrounded = true;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(15);
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platforms"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // respawn
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            LoseLife();
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(10);
        }

    }


    public void LoseLife()
    {
        lives -= 1;

        //livesHUD.SetInteger("LivesState", lives);

        if (lives > 0)
        {
            health = 100;
            healthBar.SetValue(health);
            transform.position = spawnPoint.position;
        }
        else
        {
            // go to the game over scene
            SceneManager.LoadScene("GameOver");
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetValue(health);

        if (health <= 0)
        {
            LoseLife();
        }
    }
}
