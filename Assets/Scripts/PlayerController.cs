using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
using System;

public class PlayerController : Agent
{
    // Start is called before the first frame update
    Rigidbody rb;
    public bool useVecObs;
    public float speed;
    public Text count_text;
    float movex = 0;
    float movez = 0;
    public float maxSpeed;
    public int junk_collected = 0;
    public bool isMoving;
    public GameObject hm_obj;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.velocity=new Vector3(2, 0, 2);
       
    }

    // Update is called once per frame
    void Update()
    {
            movex = Input.GetAxis("Horizontal");


            movez = Input.GetAxis("Vertical");

        /* Senza WASD
         * if(Input.GetKeyDown(KeyCode.UpArrow))
         {
             movex = -1 * speed;
         }
         if (Input.GetKeyDown(KeyCode.DownArrow))
         {
             movex = 1 * speed;
         }
         if (Input.GetKeyDown(KeyCode.LeftArrow))
         {
             movez = -1 * speed;
         }
         if (Input.GetKeyDown(KeyCode.LeftArrow))
         {
             movez = +1 * speed;
         }*/
        Vector3 v =(transform.forward *-movez);
        this.transform.Rotate(0,movex,0);
        rb.AddForce(v * speed);
        if(v == Vector3.zero)
        {
            isMoving = false;
            


        }else
        {
            isMoving = true;
            HeatMapRenderer hm_render = hm_obj.GetComponent<HeatMapRenderer>();
            hm_render.hm.AddPoint((int)((48-rb.transform.position.x)), (int)((20-rb.transform.position.z)));
            hm_render.toUpdate = true;
        }
        
    }
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Enemy_Wall"))
        {
            SceneManager.LoadScene("GameOver");
        }
        if (collision.collider.tag.Equals("Award"))
        {
            GameObject obj = collision.collider.gameObject;
            Destroy(obj);

        }
        if (collision.collider.tag.Equals("WinWall"))
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Junk"))
        {
            Debug.Log("Munnezza presa");
            GameObject app = other.gameObject;
            Destroy(app);
            junk_collected += 1;
            setCountText(junk_collected);
        }
    }

    private void setCountText ( int number)
    {
        count_text.text = ""+number;
    }
}
