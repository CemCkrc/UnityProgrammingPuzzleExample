using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalInteraction : MonoBehaviour {

    //Terminal Light (Green)
    private Light terLight;
    
    //Connected objects 
    [SerializeField]
    List<GameObject> connectedObjects = new List<GameObject>();

    //Terminal states
    public bool isActiveted = false;
    public bool isOpened = false;
    public bool wantExit = false;
    private bool isWorking = false;
    private bool isUser = false;
    private bool isSelectObj = false;

    // <will> If object closed (e.g. Open/Close door)
    // <will> selected object
    private bool objClosed = true;
    GameObject tempObj;

    //Terminal screen string
    //Player inputs screen
    [SerializeField] Text terScreen;
    private string temp;

    //Initialize variables before game starts
    void Awake()
    {
        temp = "";
        tempObj = null;
        terLight = GetComponentInChildren<Light>();
        terScreen.enabled = false;
    }

    //Update terminal later
    void LateUpdate()
    {
        if (isActiveted)
        {
            if (!isOpened && !isWorking)
            {
                terScreen.enabled = true;
                StartCoroutine(OpeningTerminal());
            }
            else if (isWorking && isOpened)
            {
                TerminalUpdate();
            }
        }
    }

    //Check player inputs to terminal screen 
    private void TerminalUpdate()
    {
        foreach (char letter in Input.inputString)
        {
            if (letter == '\b')
            {
                if (temp != "")
                {
                    temp = temp.Substring(0, temp.Length - 1);
                    terScreen.text = terScreen.text.Substring(0, terScreen.text.Length - 1); ;
                }
            }
            else if (letter == '\r')
            {
                CommandUpdate(temp);
            }
            else
            {
                terScreen.text += letter;
                temp += letter;
            }
            
        }
    }

    //Execute command
    private void CommandUpdate(string command)
    {
            int selected = TerminalCommands.computerCommands.IndexOf(command);

            if(selected == 0)
            {
                TerminalHelp();
            }
            else if(selected == 1)
            {
                terScreen.text += "\nRestarting...";
                StartCoroutine(OpeningTerminal());
            }
            else if(selected == 2)
            {
                TerminalClear();
            }
            else if(selected == 3)
            {
                TerminalList(command);
            }
            else if(selected == 4)
            {
                TerminalConnect();
            }
            else if(selected == 5)
            {
                TerminalFunction();
            }
            else if(selected == 6)
            {
                TerminalDo();
            }
            else if(selected == 7)
            {
                TerminalLogs();
            }
            else if(selected == 8)
            {
                wantExit = true;
            }
            else
            {
                terScreen.text += "\nWrong command.. Please enter again : ";
            }
        temp = "";
    }

    //If player interacts to terminal 
    //Terminal screen will available for commands after this function
    private IEnumerator OpeningTerminal()
    {
        isOpened = false;
        isWorking = true;
        yield return new WaitForSeconds(1.5f);
        terLight.range = 0.7f;
        terScreen.text = "Terminal is connecting. Please wait...";

        if (isActiveted)
        {
            yield return new WaitForSeconds(2f);
            terScreen.text = "Welcome to the Videoyun. Please enter 'help':\n";
            isOpened = true;
        }
        else
        {
            isWorking = false;
        }
    }

    // Return help screen to terminal
    private void TerminalHelp()
    {
        terScreen.text = TerminalCommands.helpCommand;
        terScreen.text += "Enter command : ";
    }

    // <will> Connect object
    private void TerminalConnect()
    {
        return;
    }

    // <will> Return object function(s)
    private void TerminalFunction()
    {
        return;
    }

    // <will> Do gameobject's job which selected from terminal
    private void TerminalDo()
    {
        return;
    }

    //Return logs to terminal screen
    private void TerminalLogs()
    {
        terScreen.text = TerminalCommands.log;
        terScreen.text += "Enter command : ";
    }

    //Check available gameobjects which ones connected terminal
    private void TerminalList(string command)
    {
        if (command.Equals("list") && connectedObjects.Count != 0)
        {
            terScreen.text = "Available Objects :";
            foreach (GameObject obj in connectedObjects)
            {
                terScreen.text += "\n-" + obj.name;
            }
        }
        else
        {
            terScreen.text = "Can't find connected object";
        }
        terScreen.text += "\nEnter command : ";
    }

    //Clear terminal screen
    private void TerminalClear()
    {
        terScreen.text = "Enter command : ";
    }

    //Exit terminal
    public void Exit()
    {
        terScreen.text = "";
        isActiveted = false;
        isOpened = false;
        isWorking = false;
        isUser = false;
        isSelectObj = false;
        terLight.range = 0f;
        terScreen.enabled = false;
    }

}
