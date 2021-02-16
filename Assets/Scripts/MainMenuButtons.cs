using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuButtons : MonoBehaviour
{


    public Button b1 ;
    public Button b2 ;
    public Button b3 ;

    // Start is called before the first frame update
    void Start()
    {
        b1.onClick.AddListener(delegate{LoadScene1();});
        
        b2.onClick.AddListener(delegate{LoadScene2();});
        
        b3.onClick.AddListener(delegate{Debug.Log("aaaa");LoadScene3();});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene1(){
        Debug.Log("aaaaaaa");
        SceneManager.LoadScene("Scene1_v2");
    }
    
    public void LoadScene2(){
        SceneManager.LoadScene("Scene2");
    }
    
    public void LoadScene3(){
        SceneManager.LoadScene("Scene3_v2");
    }
}
