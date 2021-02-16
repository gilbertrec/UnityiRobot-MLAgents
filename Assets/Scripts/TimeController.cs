﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    bool in_game;
    float currentTime;
    public Text time_text; 
    // Start is called before the first frame update
    void Start()
    {
        setGame(true);
        Time.timeScale=1.0f;
    }

    // Update is called once per frame
    private void Update() {
        if(in_game){
            currentTime = Time.time;
            time_text.text = Mathf.Floor(currentTime  / 60).ToString("00")+ ":" + Mathf.Floor(currentTime % 60).ToString("00");
        }
    }
    public void setGame(bool a) {
        in_game=a;
    }
}
