﻿using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.UI;

public class PlayerAgent_2_heatmap : Agent
{
    // Start is called before the first frame update
    Rigidbody rb;

    public float speed;
    public float maxSpeed;

    float movex = 0;
    float movez = 0;

    public int junk_collected = 0;
    public Text count_text; //display junk collected
    public Text count_text2;
    public bool isMoving;
    public GameObject hm_obj;
    public Vector3 stored_position = Vector3.zero;


    //for agent definition:
    EnvironmentParameters m_resetParams;
    HeatMapRenderer hm_render;
    public bool useVecObs;

    /* First Definition Part of the Agent*/
    public override void Initialize()
    {
        rb = this.GetComponent<Rigidbody>();
        m_resetParams = Academy.Instance.EnvironmentParameters;
        hm_render = hm_obj.GetComponent<HeatMapRenderer>();
        rb.velocity = new Vector3(2, 0, 2);
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
         



        if (vectorAction[0] == 1)
        {
            movez = 1f;
        }
        else if (vectorAction[0] == 2)
        {
            movez = -1f;
        }else
        {
            movez = 0f;
        }


        //rotation

        if (vectorAction[1] == 1)
        {
            movex = 1f;
        }
        else if (vectorAction[1] == 2)
        {
            movex = -1f;
        }
        else if (vectorAction[1] == 0)
        {
            movex = 0f;
        }

        


        Vector3 v = (transform.forward * -movez *2f);
        this.transform.Rotate(0, movex, 0);
        rb.AddForce(v, ForceMode.VelocityChange);
        
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
        

        
        
        if ((this.transform.GetChild(0).position.x - stored_position.x) < 1f && (this.transform.GetChild(0).position.x - stored_position.x) > -1f && (this.transform.GetChild(0).position.z - stored_position.z) < 1f && (this.transform.GetChild(0).position.z - stored_position.z) > -1f)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
            HeatMapRenderer hm_render = hm_obj.GetComponent<HeatMapRenderer>();
            hm_render.hm.AddPoint((int)((49 - rb.transform.position.x)), (int)((21 - rb.transform.position.z)));
            /*Test for group of pixel
            *hm_render.hm.AddPoint((int)((49-rb.transform.position.x)/2), (int)((21-rb.transform.position.z)/2));
            */
            hm_render.toUpdate = true;

            stored_position.Set(this.transform.GetChild(0).position.x, 0, this.transform.GetChild(0).position.z);
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
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Wall"))
        {
            AddReward(-0.001f);
           
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
            Destroy(app);
            junk_collected += 1;
            setCountText(junk_collected);
            AddReward(1f);
        }
    }

    void setCountText(int number)
    {
        count_text.text = "" + number;
        count_text2.text = "" + number;
    }
}