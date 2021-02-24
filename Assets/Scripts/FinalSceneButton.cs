using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalSceneButton : MonoBehaviour
{

    public Button b1;
    // Start is called before the first frame update
    void Start()
    {
        b1.onClick.AddListener(delegate { LoadScene1(); });
    }


    public void LoadScene1()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
