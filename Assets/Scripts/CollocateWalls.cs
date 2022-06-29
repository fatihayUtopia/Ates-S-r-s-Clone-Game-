using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
public class CollocateWalls : MonoBehaviour
{

    public Transform ball;
    GameObject referanceObject,currentObject;
    GameObject portal;
    float protrusion, posY, reducer;
    float narrowing;
    float timer;
    int counter;
    public int level =1;
    public Material material1_0,material1_1, material2_0,material2_1, material3_0, material3_1, material4_0, material4_1;
    RaycastHit hit, hit2;

    Queue<Transform> wallGroups = new Queue<Transform>(12);
    public float[] valuesProtrusionY;
    public float[] valuesReducerY = new float[3];



    void Start()
    {

        portal = GameObject.Find("Portal");

        for(int i =0; i<13; i++)
        {

            Transform currentWall = GameObject.Find("Walls/" + i).transform;
            wallGroups.Enqueue(currentWall);
             
        }

    }
    
    void FixedUpdate()
    {
        
        if (Physics.Raycast(ball.position,Vector3.down,out hit,Mathf.Infinity))
        {

            currentObject = hit.transform.gameObject;
            
            if (currentObject == portal || currentObject == portal.transform.GetChild(0).gameObject)
            {
                
                if (Physics.Raycast(portal.transform.position - new Vector3(0,portal.transform.localScale.y,0), Vector3.down, out hit, Mathf.Infinity))
                {
                    currentObject = hit.transform.gameObject;
                }

            }
            
            
            if(referanceObject == null)
            {
                referanceObject = currentObject;
            }
            else
            {
                if (referanceObject != currentObject)
                {
                    LineUp();
                    referanceObject = currentObject;
                }
            }
            
            
        

        }

    }
    private void Update()
    {
        #region levelManage

        timer += Time.deltaTime;

        if (timer > 25)
        {
            if (level < 4)
                level += 1;


            timer = 0;
        }

        #endregion
    }

    void LineUp()
    {

        Transform firstObject = wallGroups.Dequeue();
        Transform upperWall = firstObject.GetChild(0);
        Transform lowerWall = firstObject.GetChild(1);
        Transform backWall = firstObject.GetChild(2);

        SetMaterialColor(upperWall, lowerWall,backWall);
        

        switch (level)
        {
            
            case 1:
                narrowing = 27;
                reducer = valuesReducerY[0];
                break;

            case 2:
                narrowing = 25f;
                reducer = valuesReducerY[1];
                break;

            case 3:
                narrowing = 23;
                reducer = valuesReducerY[2];
                break;

            case 4:
                narrowing = 22;
                reducer = valuesReducerY[2];
                break;

        }
        
        int randomÝnt = Random.Range(0, valuesProtrusionY.Length);
        protrusion = valuesProtrusionY[randomÝnt];
        
        int randomValue = Random.Range(0, 10);

        if(randomValue>2)
            posY -= reducer;
        else
        {
            posY += reducer;
        }
        
       
        firstObject.localPosition = new Vector3(firstObject.localPosition.x + 35.1f, posY - protrusion, 0);
        upperWall.localPosition = new Vector3(upperWall.localPosition.x, narrowing, upperWall.localPosition.z);
        lowerWall.localPosition = new Vector3(lowerWall.localPosition.x, -1 * narrowing, lowerWall.localPosition.z);

        if (firstObject.name == "1")
        {
            portal.SetActive(false);
            portal.SetActive(true);
        }

        wallGroups.Enqueue(firstObject);
        
    }
    void SetMaterialColor(Transform wall1, Transform wall2, Transform wall3)
    {
        Renderer upperWallRenderer = wall1.gameObject.GetComponent<Renderer>();
        Renderer lowerWallRenderer = wall2.gameObject.GetComponent<Renderer>();
        Renderer backWallRenderer = wall3.gameObject.GetComponent<Renderer>();
        counter += 1;

        int materialMod = counter % 2;
        Material findMaterial = (Material)this.GetType().GetField("material" + level + "_" + materialMod).GetValue(this);
        upperWallRenderer.material = findMaterial;
        
        if(upperWallRenderer.material != lowerWallRenderer.material)
        {
            lowerWallRenderer.material = upperWallRenderer.material;
            backWallRenderer.material = upperWallRenderer.material;

            switch (level)
            {
                case 1:
                    RenderSettings.fogColor = new Color(0.42f, 0.42f, 0.36f, 1);

                    break;
                case 2:
                    RenderSettings.fogColor = new Color(0.36f, 0.42f, 0.37f, 1);

                    break;
                case 3:
                    RenderSettings.fogColor = new Color(0.36f, 0.42f, 0.40f, 1);

                    break;
                case 4:
                    RenderSettings.fogColor = new Color(0.42f, 0.36f, 0.37f, 1);

                    break;

            }
            Camera.main.backgroundColor = RenderSettings.fogColor;
        }
        


    }
    

}


