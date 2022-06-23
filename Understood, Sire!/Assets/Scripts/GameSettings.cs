using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 62;
        QualitySettings.vSyncCount = 1;

        // add a slider which controls the main channel of the AudioMixer and seperate one for white noise
    }

}
