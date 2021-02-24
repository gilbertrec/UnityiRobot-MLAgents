using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetJunkResults : MonoBehaviour
{
    // Start is called before the first frame update

    public Text junk_text;
    public Text time_text;
    void Start()
    {
        junk_text.text = Results.n_junk.ToString();
        time_text.text = Mathf.Floor(Results.current_time / 60).ToString("00") + ":" + Mathf.Floor(Results.current_time % 60).ToString("00");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
