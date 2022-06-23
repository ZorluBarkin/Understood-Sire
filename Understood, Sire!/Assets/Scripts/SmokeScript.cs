using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour
{

    private float smokeDuration = 9.0f;
    private float alpha = 1f;
    private float timeCount = 0f; 

    // Update is called once per frame
    void FixedUpdate()
    {


        transform.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,alpha);

        alpha -= 0.0025f;

        timeCount += Time.deltaTime;

        if(timeCount >= smokeDuration)
        {
            
            Destroy(this.gameObject);

        }
        
    }
}
