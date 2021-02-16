using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatmapv2
{
    
    private float[,] map;
    private int width;
    private int height;

    // returns the highest sample
  

    public Heatmapv2(int aWidth, int aHeight )
    {
        width = aWidth;
        height = aHeight;

        InitializeMap();
    }

    public void InitializeMap()
    {
        map = new float [width,height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = 0f;
            }
        }
    }
    public void AddPoint(int x, int y)
    {

        /** Heatmap version 2 with fixed values
         * 
         * 
         * 
        **/
        /*
         * this is for a map of 199x144
        x = x * 2;
        y = y * 2;*/

        //position of the robot
        if (map[x, y] > 0.5f)
        {
            Debug.Log("Refreshed Point");
        }
        else
        {
            Debug.Log("Added Point");
        }
        map[x, y] = 1;
        
        for (int i = -2; i < 3; i++)
        {

            for (int j = -2; j < 3; j++)
            {

                if (x + i >= 0 && x + i <= 197 && y + j >= 0 && y+ j <= 143)
                {
                    if (i != 0 || j != 0)
                    {

                        if ((i != 2 && i != -2) && (j != 2 && j != -2))
                        {
                            //if first contour
                            map[x + i, y + j] = Math.Max(0.5f, map[x + i, y + j]);

            
                        }
                        else
                        {
                            //if second contour
                            map[x + i, y + j] = Math.Max(0.1f, map[x + i, y + j]);
                            
                        }
                    }
                }
               
            }
        }
        
        Debug.Log("-> X:" + x + " Y:"+y + "value:" + map[x, y]);
        
    }

    public float GetPoint(int x,int y)
    {
        return map[x, y];
    }

    public float[,] GetMap()
    {
        return map;
    }

    public Texture2D GetImage()
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        for(int i=0; i<width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                /*Yellow =
                 * Orange = 255,117,20
                 * 
                 * 
                tex.SetPixel(i, j, new Color(map[i, j]/10f, (10f - map[i, j])/10f, 0, 1f));
                /*if (map[i, j] < 5f)
                {
                    tex.SetPixel(i, j, new Color((map[i, j])*3 / 10f, (3f - (map[i, j]) / 10f), 0, 1f));
                }
                else
                {
                   tex.SetPixel(i, j, new Color((map[i, j]) / 10f, 0, 0, 1f));
                }*/
                if (map[i, j] == 0.0f)
                {
                    tex.SetPixel(i, j, new Color(0.0f, 1f, 0, 1f));
                }
                else
                {
                    if (map[i, j] < 0.2f)
                    {
                        tex.SetPixel(i, j, new Color(1f, 1f, 0, 1f));
                    }


                    else
                    {
                        if (map[i, j] < 0.6f)
                        {
                            tex.SetPixel(i, j, new Color(1f, 0.45f, 0.07f, 1f));
                        }
                        else
                        {
                            tex.SetPixel(i, j, new Color(1f, 0f, 0f, 1f));
                        }
                    }
                }

            }

        }

        tex.SetPixel(0, 0, new Color(0f, 0f, 1.0f, 1.0f));
        tex.SetPixel(98, 69, new Color(1f, 1f, 0.07f, 1.0f));
        tex.Apply();
        return tex;
    }
}
