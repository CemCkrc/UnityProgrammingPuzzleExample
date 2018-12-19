using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalInteraction : MonoBehaviour {

    private Light terLight;

    [SerializeField]
    List<GameObject> connectedObjects = new List<GameObject>();

    public bool isActiveted = false;
    public bool isOpened = false;
    public bool wantExit = false;
    private bool isWorking = false;
    private bool isUser = false;
    private bool isSelectObj = false;

    private bool objClosed = true;
    GameObject tempObj;

    [SerializeField] Text terScreen;
    private string temp;

    void Awake()
    {
        temp = "";
        tempObj = null;
        terLight = GetComponentInChildren<Light>();
    }

    void LateUpdate()
    {
        if (isActiveted)
        {
            if (!isOpened && !isWorking)
            {
                StartCoroutine(OpeningTerminal());
            }
            else if (isWorking && isOpened)
            {
                TerminalUpdate();
            }
        }
    }

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

    private void CommandUpdate(string command)
    {

        if (TerminalCommands.computerCommands.Contains(command))
        {
            int selected = TerminalCommands.computerCommands.IndexOf(command);
            switch (selected)   //Degistirilecek
            {
                case 0:
                    TerminalHelp();
                    break;
                case 1:
                    terScreen.text += "\nRestarting...";
                    StartCoroutine(OpeningTerminal());
                    break;
                case 2:
                    TerminalClear();
                    break;
                case 3:
                    TerminalList(command);
                    break;
                case 4:
                    TerminalConnect();
                    break;
                case 5:
                    TerminalFunction();
                    break;
                case 6:
                    TerminalDo();
                    break;
                case 7:
                    TerminalLogs();
                    break;
                case 8:
                    wantExit = true;
                    break;
                /*
                case 9:
                    gameObject.funtion(bool);
                    break;
                */
                default:
                    terScreen.text += "\nWrong command.. Please enter again : ";
                    break;
            }
        }
        else
        {
            terScreen.text += "\nWrong command.. Please enter again : ";
        }




        //User kontrolu oldugunda
        /*if (!isUser && command == "user")
        {
            isUser = true;
            terScreen.text = "Welcome user.\n";
            foreach (GameObject obj in connectedObjects)
            {
                terScreen.text += "- " + obj.name + "\n";
            }
            terScreen.text += "Select item: ";
        }
        else if (command == "exit")
        {
            temp = "";
            wantExit = true;
        }
        else
        {
            terScreen.text = "Wrong command.. Please enter again : ";
        }
        else if (isUser)
        {
            if (!isSelectObj)
            {
                foreach (GameObject obj in connectedObjects)
                {
                    if (temp == obj.name)
                    {
                        tempObj = obj;
                        isSelectObj = true;
                        terScreen.text = "open or close? : ";
                    }
                    else
                    {
                        terScreen.text = "Wrong command.. Please enter again : ";
                    }
                }
            }
            else
            {
                if (temp == "open" && objClosed)
                {
                    objClosed = false;
                    terScreen.text = tempObj.name + " is opened. Enter 'exit' to close program :\n";
                    Vector3 newPos = new Vector3(tempObj.transform.position.x, tempObj.transform.position.y + 3f, tempObj.transform.position.z);
                    tempObj.transform.position = newPos;
                }
                else if (temp == "close" && !objClosed)
                {
                    objClosed = true;
                    terScreen.text = tempObj.name + " is closed. Enter 'exit' to close program :\n";
                    Vector3 newPos = new Vector3(tempObj.transform.position.x, tempObj.transform.position.y - 3f, tempObj.transform.position.z);
                    tempObj.transform.position = newPos;
                }
                else
                {
                    terScreen.text = "Wrong command.. Please enter again : ";
                }
            }
        }
        else
        {
            terScreen.text = "Wrong command.. Please enter again : ";
        }*/
        temp = "";
    }

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

    private void TerminalHelp()
    {
        terScreen.text = TerminalCommands.helpCommand;
        terScreen.text += "Enter command : ";
    }

    private void TerminalConnect()
    {
        return;
    }

    private void TerminalFunction()
    {
        return;
    }

    private void TerminalDo()
    {
        return;
    }

    private void TerminalLogs()
    {
        terScreen.text = TerminalCommands.log;
        terScreen.text += "Enter command : ";
    }

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
        /*else
        {
            foreach (GameObject obj in connectedObjects)
            {

                if (command == obj.name)
                {
                    tempObj = obj;
                    isSelectObj = true;
                    terScreen.text = tempObj.name + "connected terminal";
                }
                else
                {
                    terScreen.text += "\nWrong command.. Please enter again : ";
                }
            }
        }*/
    }

    private void TerminalClear()
    {
        terScreen.text = "Enter command : ";
    }

    public void Exit()
    {
        terScreen.text = "";
        isActiveted = false;
        isOpened = false;
        isWorking = false;
        isUser = false;
        isSelectObj = false;
        terLight.range = 0f;
    }

}
