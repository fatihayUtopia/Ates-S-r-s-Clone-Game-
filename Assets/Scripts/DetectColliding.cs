using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DetectColliding : MonoBehaviour
{
    
    public Material materialOfPortal, materialOfCenterPortal;
    public GameObject collideEffect, deadEffect, speedEffect;
    public ParticleSystem speedEffectPartical;
    ParticleSystem.EmissionModule speedEffectEMModule;
    int baseRateOverTime;
    CollocateWalls colWallsScrpt;
    public Transform portal;
    bool isCollidedtoCenter, isCollidedtoPortal, isDead;
    int increasingScore = 2, constantScore = 1;
    public TextMeshProUGUI text,subText;
    float score, counter;
    Color portalColorEM, centerPortalColorEM, dimColor;
    public AudioSource aSource;
    public AudioClip colPortalClip, colCenterClip, deadClip;

    private void Start()
    {

        deadEffect = gameObject.transform.GetChild(0).gameObject;
        colWallsScrpt = GameObject.Find("Walls").GetComponent<CollocateWalls>();
        speedEffectEMModule = speedEffectPartical.emission;
        baseRateOverTime = 10;

        portalColorEM = new Color(1, 1, 1, 0);
        centerPortalColorEM = new Color(0, 5, 0, 2.7f);
        dimColor = new Color(0, 0, 0, 0);

    }
    
    
    #region collidings
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Portal") && !isCollidedtoCenter)
        {
            
            isCollidedtoPortal = true;
            aSource.PlayOneShot(colPortalClip);
            counter = Time.time + 0.5f;

            materialOfPortal.SetColor("_EmissionColor", dimColor);

            score += constantScore;
            text.text = ((int)(score)) + " +" + constantScore.ToString();
            
            increasingScore = 2;
        }
        else if (other.CompareTag("CenterPortal") && !isCollidedtoPortal)
        {
            
            isCollidedtoCenter = true;
            aSource.PlayOneShot(colCenterClip);
            counter = Time.time + 0.5f;
            
            materialOfPortal.SetColor("_EmissionColor", dimColor);
            materialOfCenterPortal.SetColor("_EmissionColor", dimColor);

            text.text = ((int)(score)) + " +" + increasingScore.ToString();

            if (increasingScore < 5)
            {
                increasingScore += 1;
                baseRateOverTime += 40;
                speedEffectEMModule.rateOverTime = baseRateOverTime;
            }
            score += increasingScore;

            
            collideEffect.SetActive(true);
        }

        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        isDead = true;
        deadEffect.SetActive(true);
        aSource.PlayOneShot(deadClip);

        DestroyAndHideObjects();
        Invoke("ShowSubText",0.5f);
    }
    #endregion


    private void Update()
    {
        
        if (text.color.a < 1)
            text.color = Color.Lerp(text.color, Color.white, 3 * Time.deltaTime);

        if(!isDead)
        score += Time.deltaTime;

        if (counter < Time.time)
        {
            text.text = ((int)(score)).ToString();

            materialOfPortal.SetColor("_EmissionColor", portalColorEM);
            materialOfCenterPortal.SetColor("_EmissionColor", centerPortalColorEM);
        }
        
        
        //if portal's re-lineUp
        if(transform.position.x < portal.position.x -4)
        {
            isCollidedtoCenter = false;
            isCollidedtoPortal = false;
            collideEffect.SetActive(false);
        }
        //if passed up the center portal
        else if (transform.position.x > portal.position.x +4 && !isCollidedtoCenter)
        {
            increasingScore = 2;
            baseRateOverTime = 10;
            speedEffectEMModule.rateOverTime = baseRateOverTime;
        }


        if (subText.gameObject.activeSelf == true && isDead)
        {
            subText.color = Color.Lerp(subText.color, Color.white, Time.deltaTime);
            
            if (subText.color.a>0.4f)
            {
                if((int)score> PlayerPrefs.GetInt("highScore"))
                {
                    PlayerPrefs.SetInt("highScore", (int)score);
                }
                
                if (Input.GetMouseButtonDown(0))
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
    void DestroyAndHideObjects()
    {
        
        gameObject.GetComponent<PlayerMove>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        if (gameObject.GetComponent<SpringJoint>() != null)
            Destroy(gameObject.GetComponent<SpringJoint>());


        Destroy(gameObject.GetComponent<LineRenderer>());
        Destroy(gameObject.GetComponent<Rigidbody>());
        speedEffect.SetActive(false);
        collideEffect.SetActive(false);
        colWallsScrpt.enabled = false;
    }
    void ShowSubText()
    {
        subText.color = new Color(1, 1, 1, 0);
        subText.text = "Tap to Play Again";
        subText.gameObject.SetActive(true);
    }
}
