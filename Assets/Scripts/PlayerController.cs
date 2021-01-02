﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public float speed;
    float movex = 0;
    float movez = 0;
    public float maxSpeed;
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
}