/*--------------------------------------------------------------
// ScoreManager.cs
//
// Manage Player score
//
// Created by Tran Minh Son on Oct 24 2020
// StudentID: 101137552
// Date last Modified: Oct 25 2020
// Rev: 1.1
//  
// Copyright © 2020 Tran Minh Son. All rights reserved.
--------------------------------------------------------------*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    private static ScoreManager m_instance = null;

    public static ScoreManager Instance()
    {
        if (m_instance == null)
        {
            m_instance = new ScoreManager();
        }

        return m_instance;
    }

    public int playerScore;

    public const int ENEMY_POINT = 100;
    public const int REWARD_POINT = 70;
}
