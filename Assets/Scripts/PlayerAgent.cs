﻿using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.UI;

public class PlayerAgent : Agent
{
    // Start is called before the first frame update
    Rigidbody rb;

    public float speed;
    public float maxSpeed;

    float movex = 0;
    float movez = 0;

    public int junk_collected = 0;
    public int junk_area = 0;
    public Text count_text; //display junk collected
    public Text count_text2;
    public Text time_text;

    public bool isMoving;
    public GameObject hm_obj;
    public Vector3 stored_position = Vector3.zero;
    public HouseScript house_script;
    public int active_display=1;
    public Camera[] cameras;
    //for agent definition:
    EnvironmentParameters m_resetParams;
    HeatMapRenderer hm_render;
    public bool useVecObs;

    /* First Definition Part of the Agent*/
    public override void Initialize()
    {
      //  cameras = Camera.allCameras;
        rb = this.GetComponent<Rigidbody>();
        m_resetParams = Academy.Instance.EnvironmentParameters;
        hm_render = hm_obj.GetComponent<HeatMapRenderer>();
        rb.velocity = new Vector3(2, 0, 2);
        house_script = new HouseScript();
        for (int i = 0; i < 3; i++)
        {
            if (i + 1 == active_display)
            {
                cameras[i].enabled = true;
            }
            else
            {
                cameras[i].enabled = false;
            }
        }
    }



    public void SetRobot()
    {
        //Set the attributes of the robot by fetching the information from the academy
        rb.mass = m_resetParams.GetWithDefault("mass", 1.0f);
        var scale = m_resetParams.GetWithDefault("scale", 1.0f);
        this.transform.localScale = new Vector3(scale, scale, scale);
    }
  

    public override void CollectObservations(VectorSensor sensor)
    {
        //X of the robot
        sensor.AddObservation(this.transform.position.x);
        //Z of the robot
        sensor.AddObservation(this.transform.position.z);
        sensor.AddObservation(this.rb.velocity.x);
        sensor.AddObservation(this.rb.velocity.z);
        sensor.AddObservation(this.rb.rotation.y);


        //cannot do addObservation of a matrix, need to find something else
        //sensor.AddObservation(hm_render.hm.GetMap());
    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var vectorAction = actionBuffers.DiscreteActions;
        //set the list of action of the robot and for each decision how he has to do
        /*
         * 
         * vectorActions:
         * vectorAction[0] : move forward,behind or not
         * vectorAction[1] : rotation
         * 
         * 
         */




        //forward

        if (vectorAction[0] == 1)
        {
            movez = 1f;
            movex = 0;
        }

        //backward
        else if (vectorAction[0] == 2)
        {
            movez = -1f;
            movex = 0;
        }

        //stopping
        else if (vectorAction[0] == 0)
        {
            movez = 0f;
            movex = 0;
        }


        //rotation

        //right
        else if (vectorAction[0] == 3)
        {
            movex = 3f;
            movez = 0;
        }
        //left
        else if (vectorAction[0] == 4)
        {
            movex = -3f;
            movez = 0;
        }

        


        Vector3 v = (transform.forward * -movez);
        this.transform.Rotate(0, -movex*3f, 0);
        rb.AddForce(v *speed);
        
        //set a negative reward for wasting time.
        AddReward(-1f / MaxStep);


    }





    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float moving_x = Input.GetAxis("Horizontal");
        float moving_z = Input.GetAxis("Vertical");
        //Player input
        var discreteActions = actionsOut.DiscreteActions;
        if (moving_x < 0)
        {
            discreteActions[1] = 1;
        }
        else if(moving_x == 0f)
        {
            discreteActions[1] = 0;
        }
        else
        {
            discreteActions[1] = 2;
        }

        if (moving_z > 0)
        {
            discreteActions[0] = 1;
        }
        else if (moving_z == 0f)
        {
            discreteActions[0] = 0;
        }
    }



    public override void OnEpisodeBegin()
    {
        Reset();
    }

    public void Reset()
    {

    }

  
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {

            if (active_display < 3)
            {
                active_display += 1;
            }
            else
            {
                active_display = 1;
            }
            for(int i = 0; i < 3; i++)
            {
                if (i+1 == active_display)
                {
                    cameras[i].enabled = true;
                }
                else
                {
                    cameras[i].enabled = false;
                }
            }
        }
        
        
       
        // Debug.Log("Offset X:" + (this.transform.GetChild(0).position.x, stored_position.x));
        // Debug.Log("Offset Z:" + (this.transform.GetChild(0).position.z, stored_position.z));

    }
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if ((this.transform.GetChild(0).position.x - stored_position.x) < 1f && (this.transform.GetChild(0).position.x - stored_position.x) > -1f && (this.transform.GetChild(0).position.z - stored_position.z) < 1f && (this.transform.GetChild(0).position.z - stored_position.z) > -1f)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
            HeatMapRenderer hm_render = hm_obj.GetComponent<HeatMapRenderer>();
            hm_render.hm.AddPoint((int)((rb.transform.position.x)), (int)((rb.transform.position.z)));
            /*Test for group of pixel
            *hm_render.hm.AddPoint((int)((49-rb.transform.position.x)/2), (int)((21-rb.transform.position.z)/2));
            */
            hm_render.toUpdate = true;

            stored_position.Set(this.transform.GetChild(0).position.x, 0, this.transform.GetChild(0).position.z);
        }


    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Wall"))
        {
            AddReward(-0.003f);
           
        }
        if (collision.collider.tag.Equals("Obstacle"))
        {
            AddReward(-0.003f);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Junk"))
        {
            Debug.Log("Munnezza presa");
            GameObject app = other.gameObject;
            app.SetActive(false);
            junk_collected += 1;
            junk_area += 1;
            setCountText(junk_collected);
            AddReward(5f);
        }

        if (junk_area >= 21)
        {
            junk_area = 0;
            house_script.resetTrash();
            hm_render.hm.InitializeMap();
            time_text.GetComponent<TimeController>().setGame(false);
            Results.current_time = time_text.GetComponent<TimeController>().currentTime;
            time_text.GetComponent<TimeController>().currentTime = 0f;
            Results.n_junk = junk_collected;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_finished");
        }

    }

    void setCountText(int number)
    {
        count_text.text = "" + number;
        count_text2.text = "" + number;
    }
}
