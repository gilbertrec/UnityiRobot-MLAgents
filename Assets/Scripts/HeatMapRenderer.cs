﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapRenderer : MonoBehaviour
{
    
    public Texture2D textref;
    public RenderTexture rt;
    public Camera test_c;
    private Heatmap hm;
    // Start is called before the first frame update
    void Start()
    {

        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        Gradient gradient = new Gradient();
        hm = new Heatmap(50, 50);
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

       
        
        textref = new Texture2D(rt.height, rt.width, TextureFormat.RGBA32, false);
        RenderTexture.active = rt;
        for (int i = 0; i < 4; i++)
        {
            hm.AddPoint(10, 10);
        }

        textref = hm.GetImage();
        textref.SetPixel(40, 40, new Color(0f, 0f, 1.0f, 1.0f));
        textref.Apply();
        GetComponent<Renderer>().material.mainTexture = textref;
        Graphics.Blit(textref, rt);
    }

    // Update is called once per frame
    void Update()
    {
       

       
    }
}
