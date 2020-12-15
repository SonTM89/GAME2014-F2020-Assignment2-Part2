using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

[System.Serializable]
public enum ImpulseSounds
{
    JUMP,
    HIT1,
    HIT2,
    HIT3,
    DIE
}


public class PlayerBehavior : MonoBehaviour
{
    [Header("Control")]
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;

    [Header("Platform Detection")]
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public Transform spawnPoint;

    [Header("Player Abilities")]
    public int health;
    public int lives;
    public BarController healthBar;
    public Animator livesHUD;

    [Header("Dust Trail")]
    public ParticleSystem m_dustTrail;
    public Color dustTrailColor;

    [Header("Impulse Sounds")]
    public AudioSource[] sounds;

    [Header("Special FX")]
    public CinemachineVirtualCamera vcam1;
    public CinemachineBasicMultiChannelPerlin perlin;
    public float maxShakeTime;
    public float shakeIntensity;
    public float shakeTimer;
    public bool isCameraShaking;

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;


    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        lives = 3;

        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_dustTrail = GetComponentInChildren<ParticleSystem>();

        sounds = GetComponents<AudioSource>();

        vcam1 = FindObjectOfType<CinemachineVirtualCamera>();
        perlin = vcam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Move();

        if (isCameraShaking)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0.0f) // time out
            {
                perlin.m_AmplitudeGain = 0.0f;
                isCameraShaking = false;
                shakeTimer = maxShakeTime;

            }
        }
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

                    CreateDustTrail();
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    m_rigidBody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = true;
                    m_animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);

                    CreateDustTrail();
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

                sounds[(int)ImpulseSounds.JUMP].Play();
                CreateDustTrail();
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
            TakeDamage(10);
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platforms"))
        {
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Delay Enemy damage
            if (Time.frameCount % 60 == 0)
            {
                TakeDamage(5);
            }

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
            TakeDamage(5);
        }

    }


    public void LoseLife()
    {
        lives -= 1;

        sounds[(int)ImpulseSounds.DIE].Play();

        ShakeCamera();

        livesHUD.SetInteger("LivesState", lives);

        if (lives > 0)
        {            
            transform.position = spawnPoint.position;
            health = 100;
            healthBar.SetValue(health);
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

        PlayRandomHitSound();
        ShakeCamera();

        if (health <= 0)
        {
            LoseLife();
        }
    }

    private void CreateDustTrail()
    {
        m_dustTrail.GetComponent<Renderer>().material.SetColor("_Color", dustTrailColor);

        m_dustTrail.Play();
    }

    private void PlayRandomHitSound()
    {
        var randomSound = Random.Range(1, 3);
        sounds[randomSound].Play();
    }

    private void ShakeCamera()
    {
        perlin.m_AmplitudeGain = shakeIntensity;
        isCameraShaking = true;
    }
}
