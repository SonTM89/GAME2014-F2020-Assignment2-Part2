/*--------------------------------------------------------------
// BarController.cs
//
// Handle the behaviours of health bars.
//
// Created by Tran Minh Son on Dec 13 2020
// StudentID: 101137552
// Date last Modified: Dec 15 2020
// Rev: 1.1
//  
// Copyright © 2020 Tran Minh Son. All rights reserved.
--------------------------------------------------------------*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarController : MonoBehaviour
{
    public Transform bar;
    public Transform entity;
    public int currentValue;
    public int maxValue;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = 100;
        maxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(entity != null)
        {
            transform.position = entity.position + Vector3.up;
        }
    }

    public void SetValue(int new_value)
    {
        currentValue = new_value;
        bar.localScale = new Vector3((float)((double)currentValue / (double)maxValue), 1.0f, 1.0f);

        // clamp the scale on the x axis to be zero minimum
        if(bar.localScale.x < 0)
        {
            bar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        }
    }
}
