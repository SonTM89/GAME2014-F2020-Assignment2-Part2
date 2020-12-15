using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Bullet Related")]
    public int MaxBullets;
    public BulletType playerBulletType;
    public BulletType enemyBulletType;

    // Start is called before the first frame update
    void Start()
    {
        BulletManager.Instance().Init(MaxBullets, playerBulletType, enemyBulletType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
