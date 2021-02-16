using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript
{
    // Start is called before the first frame update

    GameObject[] trashbags;
    public HouseScript()
    {
        trashbags = GameObject.FindGameObjectsWithTag("Junk");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetTrash()
    {
        foreach (GameObject trashbag in trashbags)
        {
            trashbag.SetActive(true);
        }
    }
}
