/*--------------------------------------------------------------
// VirtualGuyBehaviour.cs
//
// Handle all enemy behaviours
//
// Created by Tran Minh Son on Dec 15 2020
// StudentID: 101137552
// Date last Modified: Dec 15 2020
// Rev: 1.1
//  
// Copyright © 2020 Tran Minh Son. All rights reserved.
--------------------------------------------------------------*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualGuyBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float runForce;
    public Rigidbody2D rigidbody;
    public Transform lookInFrontPoint;
    public Transform lookAheadPoint;
    public LayerMask collisionWallLayer;
    public LayerMask collisionGroundLayer;
    public bool isGroundAhead;

    [Header("AI")]
    public LOS virtualGuyLOS;

    [Header("Abilities")]
    public int health;
    public BarController healthBar;

    [Header("Bullet Firing")]
    public Transform bulletSpawn;
    public float fireDelay;
    public PlayerBehavior player;

    private AudioSource hitSound;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<PlayerBehavior>();
        hitSound = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_hasLOS())
        {
            Debug.Log("See player!");
            _FireBullet();
        }
        else
        {
            Debug.Log("No");
        }

        _LookInFront();
        _LookAhead();
        _Move();
    }


    private void _FireBullet()
    {
        //delay bullet firing
        if (Time.frameCount % fireDelay == 0 && BulletManager.Instance().HasBullets(PoolType.ENEMY))
        {
            var playerPosition = player.transform.position;
            var firingDirection = Vector3.Normalize(playerPosition - bulletSpawn.position);

            Debug.Log(firingDirection.ToString());

            BulletManager.Instance().GetBullet(PoolType.ENEMY, bulletSpawn.position, firingDirection);
        }
    }



    private bool _hasLOS()
    {
        if (virtualGuyLOS.colliders.Count > 0)
        {
            if (virtualGuyLOS.collidesWith.gameObject.name == "Player" && virtualGuyLOS.colliders[0].gameObject.name == "Player")
            {
                return true;
            }
        }
        return false;
    }


        private void _LookInFront()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookInFrontPoint.position, collisionWallLayer);

        if (wallHit)
        {
            _FlipX();
        }

        Debug.DrawLine(transform.position, lookInFrontPoint.position, Color.red);
    }


    private void _LookAhead()
    {
        isGroundAhead = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);

        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.green);
    }


    private void _Move()
    {
        if (isGroundAhead)
        {
            rigidbody.AddForce(Vector2.right * runForce * Time.deltaTime * transform.localScale.x);

            rigidbody.velocity *= 0.90f;
        }
        else
        {
            _FlipX();
        }

    }


    private void _FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            Dead();
        }

        if (other.gameObject.CompareTag("Apple"))
        {
            TakeDamage(25);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetValue(health);
        hitSound.Play();
        

        if (health <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        ScoreManager.Instance().playerScore += ScoreManager.ENEMY_POINT;
        transform.parent.gameObject.SetActive(false);
    }
}
