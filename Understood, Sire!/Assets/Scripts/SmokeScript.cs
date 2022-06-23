using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour
{

    // duration and alpha
    private float smokeDuration = 9.0f; // disperse duration is about 8 seconds + 1 is for the guarantee
    private float alpha = 1f;
    private float timeCount = 0f;

    // scale values
    private float sizeX = 5f;
    private float sizeY = 5f;

    // Update is called once per frame
    void FixedUpdate()
    {


        transform.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,alpha);

        transform.GetComponent<Transform>().localScale = new Vector3(sizeX, sizeY, 1f);

        alpha -= 0.0025f;

        sizeX += 0.0025f;
        sizeY += 0.01f;

        timeCount += Time.deltaTime;

        if(timeCount >= smokeDuration)
        {
            
            Destroy(this.gameObject);

        }
        
    }
}
