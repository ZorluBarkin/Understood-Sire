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

    private bool clicked;

    public List<string> input = new List<string>();

    private bool send = false;
    private int lineIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        clicked = false;

        paper.lineType = InputField.LineType.MultiLineSubmit;
        paper.contentType = InputField.ContentType.Alphanumeric;
        
        commandButton.onClick.AddListener(GetPaper);

        paper.GetComponent<RectTransform>().localPosition = new Vector3(0f, -500f, 0f); 
        // -500 is out of screen, -200 is writing position
    }

    private void FixedUpdate()
    {
        // make the paper go up and down
        if (clicked)
        {
            if (paper.GetComponent<RectTransform>().localPosition.y <= -200)
            {
                paper.GetComponent<RectTransform>().localPosition += new Vector3(0f, 4f, 0f);
            }
        }
        else
        {
            if(paper.GetComponent<RectTransform>().localPosition.y >= -500)
            {
                paper.GetComponent<RectTransform>().localPosition += new Vector3(0f, -2f, 0f);
            }
        }

    }

    private void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            //input.Add(parsePaperInput(paper.text, lineIndex));
            input.Add(paper.text);
            lineIndex++;
        }

        if (send)
        {
            // send the orders

            lineIndex = 0;
        }

    }

    private void GetPaper()
    {
        if(clicked == true)
        {
            clicked = false;
            return;
        }
        
        clicked = true;
        
    }

    private string parsePaperInput(string text, int index) 
    {
        string s = "";
        //for (int i = 0; i<lineIndex; i++)
        //{
        //    s = "\n" + text;
        //}

        //if (lineIndex == 0)
            s = text;

        return s;
    }
}
