using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetStart : MonoBehaviour
{
    
    GameObject ball, walls, subTextObj;
    SpringJoint joint;
    PlayerMove moveScrpt;
    PortalMove portalScrpt;
    CollocateWalls wallsScrpt;
    DetectColliding detectScrpt;
    Camera cam;
    LineRenderer lineRenderer;
    TextMeshProUGUI text;
    float posY;
    bool Transformation;

    private void Awake()
    {
        
        cam = Camera.main;
        ball = GameObject.Find("Ball");
        walls = GameObject.Find("Walls");
        joint = ball.GetComponent<SpringJoint>();
        moveScrpt = ball.GetComponent<PlayerMove>();
        lineRenderer = ball.GetComponent<LineRenderer>();
        wallsScrpt = walls.GetComponent<CollocateWalls>();
        portalScrpt = GameObject.Find("Portal").GetComponent<PortalMove>();
        detectScrpt = GameObject.Find("Ball").GetComponent<DetectColliding>();
        text = GameObject.Find("Canvas/Text").GetComponent<TextMeshProUGUI>();
        subTextObj = GameObject.Find("Canvas/SubText");


        lineRenderer.enabled = true;
        moveScrpt.enabled = false;
        wallsScrpt.enabled = false;
        detectScrpt.enabled = false;

        if (PlayerPrefs.HasKey("highScore"))
        {
            PlayerPrefs.GetInt("highScore");
        }
        else
        {
            PlayerPrefs.SetInt("highScore",0);
        }
        subTextObj.GetComponent<TextMeshProUGUI>().text = "High Score: " + PlayerPrefs.GetInt("highScore");

    }
    private void Start()
    {
        //Collocate Walls
        for (int i = 1; i < 13; i++)
        {

            Transform currentWall = GameObject.Find("Walls/" + i).transform;
            int randomÝnt = Random.Range(0, wallsScrpt.valuesProtrusionY.Length);
            posY = wallsScrpt.valuesProtrusionY[randomÝnt];

            if(currentWall.name !="5" && currentWall.name != "4" && currentWall.name != "6")
            currentWall.localPosition = new Vector3(currentWall.localPosition.x, -posY, 0);

        }


        
    }

    void Update()
    {
        lineRenderer.SetPosition(0, ball.transform.position);

        if (Input.GetMouseButtonDown(0))
        {

            if (!Transformation)
            {

                Destroy(joint);
                subTextObj.SetActive(false);
                wallsScrpt.enabled = true;
                detectScrpt.enabled = true;
                moveScrpt.enabled = true;
                lineRenderer.enabled = false;

                cam.transform.SetParent(ball.transform);
                Transformation = true;

            }

        }
        if (Transformation)
        {
            text.color = Color.Lerp(text.color, new Color(1, 1, 1, 0), 2* Time.deltaTime);
            cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, new Vector3(-5, 7, -42), 10 * Time.deltaTime);
            if (cam.transform.localPosition.x == -5)
            {

                Destroy(this);
            }
                
        }
        
    }

}
