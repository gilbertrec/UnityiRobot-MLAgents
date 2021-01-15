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

    void InitializeMap()
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
        map[x,y]+=1f;
        Debug.Log("Added Point, X:" + x + " Y:"+y + "value:" + map[x, y]);
        
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
                //tex.SetPixel(i, j, new Color(map[i, j]/10f, (10f - map[i, j])/10f, 0, 1f));
                if (map[i, j] < 5f)
                {
                    tex.SetPixel(i, j, new Color((map[i, j])*3 / 10f, (3f - (map[i, j]) / 10f), 0, 1f));
                
                }
                else
                {
                   tex.SetPixel(i, j, new Color((map[i, j]) / 10f, 0, 0, 1f));
                
                }


            }

        }

        tex.SetPixel(0, 0, new Color(0f, 0f, 1.0f, 1.0f));
        tex.SetPixel(98, 69, new Color(0f, 0f, 1.0f, 1.0f));
        tex.Apply();
        return tex;
    }
}
