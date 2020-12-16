using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Bullet Related")]
    public int MaxBullets;
    public BulletType playerBulletType;
    public BulletType enemyBulletType;

    [Header("Moving Platforms")]
    public List<MovingPlatformController> movingPlatforms;

    // Start is called before the first frame update
    void Start()
    {
        movingPlatforms = FindObjectsOfType<MovingPlatformController>().ToList();

        BulletManager.Instance().Init(MaxBullets, playerBulletType, enemyBulletType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetAllPlatforms()
    {
        foreach (var platform in movingPlatforms)
        {
            platform.Reset();
        }
    }
}
