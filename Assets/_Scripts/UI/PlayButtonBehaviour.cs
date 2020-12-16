/*--------------------------------------------------------------
// PlayButtonBehaviour.cs
//
// Handle the event when pressing the Play Button in MainMenu Scene.
//
// Created by Tran Minh Son on Nov 20 2020
// StudentID: 101137552
// Date last Modified: Dec 15 2020
// Rev: 1.1
//  
// Copyright © 2020 Tran Minh Son. All rights reserved.
--------------------------------------------------------------*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonBehaviour : MonoBehaviour
{
    // button click sound
    public AudioSource clickSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Event handler for PlayButtonPressed Event
    public void OnPlayButtonPressed()
    {
        clickSound.Play();
        Debug.Log("Go to GamePlay Scene");
        StartCoroutine(LoadLevel("GamePlay", 0.3f));
    }


    // Waiting for _delay seconds to load new scene
    IEnumerator LoadLevel(string _name, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_name);
    }
}
