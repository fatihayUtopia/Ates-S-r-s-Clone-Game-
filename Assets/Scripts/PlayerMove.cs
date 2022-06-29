using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rB,objectToHoldRB;
    GameObject portal;
    LineRenderer lR;
    SpringJoint sJ;
    Transform objectToHold;
    Vector2 moveDir;
    Vector3 linePoint,lineTractPoint;
    public float speed;
    bool pressed, pressing, pressKey,isHolded,holdKey;
    public AudioSource aSource;
    public AudioClip ropeClip;
    RaycastHit hit,hit2;



    void Start()
    {
        portal = GameObject.Find("Portal");

        rB = GetComponent<Rigidbody>();
        lR = GetComponent<LineRenderer>();
        sJ = GetComponent<SpringJoint>();

    }

   
    void FixedUpdate()
    {
        
        if (pressed)
        {
            
            objectToHold = findObject();
            objectToHoldRB = objectToHold.gameObject.AddComponent<Rigidbody>();
            objectToHoldRB.isKinematic = true;

            linePoint = new Vector3(objectToHold.position.x, objectToHold.position.y - (objectToHold.localScale.y * 0.5f), objectToHold.localPosition.z);
            lR.enabled = true;
            lineTractPoint = transform.position;


            aSource.PlayOneShot(ropeClip);
            pressKey = true;
            holdKey = true;
            pressed = false;
        }
        
        if (pressing)
        {
            if (holdKey)
            {
                
                lineTractPoint = Vector3.MoveTowards(lineTractPoint, linePoint, 3);
                
                if (lineTractPoint == linePoint)
                {
                    isHolded = true;

                    //Component Settings
                    sJ = gameObject.AddComponent<SpringJoint>();
                    sJ.connectedBody = objectToHoldRB;
                    sJ.autoConfigureConnectedAnchor = false;
                    sJ.connectedAnchor = new Vector3(0, -0.5f, 0);
                    sJ.spring = 70;
                    sJ.damper = 90;

                    holdKey = false;
                }

            }
            


            
        }
        else if (!pressing)
        {
            
            if (pressKey)
            {

                Destroy(objectToHoldRB);
                Destroy(sJ);

                rB.velocity = rB.velocity.normalized;
                rB.AddForce(rB.velocity.normalized * speed, ForceMode.VelocityChange);

                isHolded = false;
                pressKey = false;
            }

        }


    }
    private void Update()
    {

        #region inputs
        
        if (Input.GetMouseButtonDown(0))
        {
            pressing = false;
            pressed = false;
            
            pressed = true;
            pressing = true;
        }
        
        else if (Input.GetMouseButtonUp(0))
        {
            pressing = false;
            pressed = false;
        }

        #endregion

        if (pressing)
        {
            
            lR.SetPosition(0, transform.position);

            if (isHolded)
            {
                lR.SetPosition(1, linePoint);
            }
            else
            {
                lR.SetPosition(1, lineTractPoint);
            }
          
        }
        else if (!pressing)
        {
            lR.enabled = false;
        }

    }

    Transform findObject()
    {
        if(Physics.Raycast(transform.position,new Vector3(2,3,0),out hit, Mathf.Infinity))
        {
            Transform hitObjectTransform = hit.transform;
            
            if (hitObjectTransform.gameObject == portal.gameObject || hitObjectTransform.gameObject == portal.transform.GetChild(0).gameObject)
            {
                                            
                if (Physics.Raycast(portal.transform.position - new Vector3(0, portal.transform.localScale.y, 0), new Vector3(2, 3, 0), out hit, Mathf.Infinity))
                {
                    hitObjectTransform = hit.transform;
                }
            }
            
            
            return hitObjectTransform;

        }
        else
        {
            Time.timeScale = 0;
            return null;
        }
    }

}
