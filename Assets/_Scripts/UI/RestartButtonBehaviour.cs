/*--------------------------------------------------------------
// RestartButtonBehaviour.cs
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
using TMPro;

public class RestartButtonBehaviour : MonoBehaviour
{
    // reference to score UI
    public TextMeshProUGUI totalScore;

    // Start is called before the first frame update
    void Start()
    {
        totalScore.text = ScoreManager.Instance().playerScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Event handler for RestartButtonPressed Event
    public void OnRestartButtonPressed()
    {
        Debug.Log("Restart!");
        SceneManager.LoadScene("GamePlay");
    }
}
