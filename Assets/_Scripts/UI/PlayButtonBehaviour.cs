﻿/*--------------------------------------------------------------
// PlayButtonBehaviour.cs
//
// Handle the event when pressing the Play Button in MainMenu Scene.
//
// Created by Tran Minh Son on Nov 20 2020
// StudentID: 101137552
// Date last Modified: Nov 20 2020
// Rev: 1.0
//  
// Copyright © 2020 Tran Minh Son. All rights reserved.
--------------------------------------------------------------*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Event handler for PlayButtonPressed Event
    public void OnPlayButtonPressed()
    {
        Debug.Log("Play!");
        SceneManager.LoadScene("GamePlay");
    }
}