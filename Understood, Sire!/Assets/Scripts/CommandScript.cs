using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandScript : MonoBehaviour
{

    [SerializeField] private Button commandButton;

    [SerializeField] private InputField paper;

    [SerializeField] private Camera main;

    private RectTransform paperPos;
    
    // Start is called before the first frame update
    void Start()
    {
        commandButton.onClick.AddListener(InputField);
        paperPos = paper.GetComponent<RectTransform>();
        paperPos.localPosition = new Vector3(0f, -500f, 0f);
    }

    private void InputField()
    {

        
        if(paper.GetComponent<RectTransform>().localPosition.y < paperPos.localPosition.y)
        {
            // implement something here
        }
        else{
            return;
        }
    }

}
