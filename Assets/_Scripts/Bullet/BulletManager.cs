using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum PoolType
{
    ENEMY,
    PLAYER
}

public class BulletManager
{
    // step 1. create a private static instance
    private static BulletManager m_instance = null;

    // step 2. make our default constructor private
    private BulletManager()
    {

    }

    // step 3. make a public static creational method for class access
    public static BulletManager Instance()
    {
        if (m_instance == null)
        {
            m_instance = new BulletManager();
        }
        return m_instance;
    }
    
    public int MaxBullets { get; set; }

    private Queue<GameObject> m_playerBulletPool;
    private Queue<GameObject> m_enemyBulletPool;


    /// <summary>
    /// This function initializes the bullet pool with the number of bullets specified and the bullet enumeration type
    /// </summary>
    /// <param name="max_bullets"></param>
    /// <param name="playerBulletType"></param>
    /// <param name="enemyBulletType"></param>
    public void Init(int max_bullets = 50, BulletType playerBulletType = BulletType.PULSING, BulletType enemyBulletType = BulletType.FAT)
    {   // step 4 initialize class variables and start the bullet pool build
        MaxBullets = max_bullets;
        _BuildBulletPool(playerBulletType, enemyBulletType);
    }

    /// <summary>
    /// This function creates the Object Pool for bullet Game Objects
    /// </summary>
    /// <param name="playerBulletType"></param>
    /// <param name="enemyBulletType"></param>
    private void _BuildBulletPool(BulletType playerBulletType, BulletType enemyBulletType)
    {
        // create empty Queue structure
        m_playerBulletPool = new Queue<GameObject>();
        m_enemyBulletPool = new Queue<GameObject>();

        for (int count = 0; count < MaxBullets; count++)
        {
            var tempPlayerBullet = BulletFactory.Instance().createBullet(playerBulletType);
            m_playerBulletPool.Enqueue(tempPlayerBullet);

            var tempEnemyBullet = BulletFactory.Instance().createBullet(enemyBulletType);
            m_enemyBulletPool.Enqueue(tempEnemyBullet);
        }
    }

    public GameObject GetBullet(PoolType pool, Vector3 position, Vector3 direction)
    {
        GameObject newBullet = null;
        switch (pool)
        {
            case PoolType.PLAYER:
                newBullet = m_playerBulletPool.Dequeue();
                newBullet.SetActive(true);
                newBullet.transform.position = position;
                newBullet.GetComponent<GrenadeBehaviour>().direction = direction;
                newBullet.GetComponent<GrenadeBehaviour>().Initialize();
                break;
            case PoolType.ENEMY:
                newBullet = m_enemyBulletPool.Dequeue();
                newBullet.SetActive(true);
                newBullet.transform.position = position;
                newBullet.GetComponent<BulletController>().direction = direction;
                break;
            
        }

        return newBullet;
    }

    public bool HasBullets(PoolType pool)
    {
        switch (pool)
        {
            case PoolType.ENEMY:
                return m_enemyBulletPool.Count > 0;
            case PoolType.PLAYER:
                return m_playerBulletPool.Count > 0;
            default:
                return false;
        }
    }

    public void ReturnBullet(PoolType pool, GameObject returnedBullet)
    {
        returnedBullet.SetActive(false);
        switch (pool)
        {
            case PoolType.ENEMY:
                m_enemyBulletPool.Enqueue(returnedBullet);
                break;
            case PoolType.PLAYER:
                m_playerBulletPool.Enqueue(returnedBullet);
                break;
        }
    }
}
