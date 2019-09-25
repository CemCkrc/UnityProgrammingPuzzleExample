using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TerminalController : Interactable
{
    #region Values
    Animator anim;
    [SerializeField] AudioClip[] audioClips;
    AudioSource terAudio;

    Canvas terCanvas;
    private static TMP_Text mainText;
    TMP_InputField terInput;
    [SerializeField] GameObject startScreen;

    public static bool controllerChange;
    #endregion

    private void Awake()
    {
        terAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        terCanvas = GetComponentInChildren<Canvas>();
        terInput = terCanvas.GetComponentInChildren<TMP_InputField>();
        mainText = terCanvas.transform.Find("MainText").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        AddDefaultFunctions();
        anim.SetBool("IsOpened", false);
        controllerChange = false;

        Functions.instance.ExecuteFunction("HELP", null);
    }

    public override void Interact(Transform playerCam)
    {
        base.Interact(playerCam);

        TerAnim();
        StartCoroutine(WelcomeScreen());
    }

    private void TerAnim()
    {
        if (isWorking)
        {
            anim.SetBool("IsOpened", true);
            terAudio.PlayOneShot(audioClips[0]);
        }
        else
        {
            anim.SetBool("IsOpened", false);
            terAudio.PlayOneShot(audioClips[1]);
        }
    }

    private void Update()
    {
        if (!isWorking)
            return;
        
        if (!terInput.isFocused && !controllerChange)
            terInput.ActivateInputField();

        TerminalUpdate();

        if (Input.GetMouseButtonDown(1))
            TerminalExit();
    }

    void TerminalUpdate()
    {
        if (Input.GetButtonDown("Submit"))
            TerSubmit();
    }

    void TerSubmit()
    {
        if(controllerChange)
        {
            terInput.DeactivateInputField();
            return;
        }
        ClearMainScreen();

        string[] input = SplitString(terInput.text);
        string funcName, funcVal;

        if (input[0] == "")
        {
            ShowError(-2);
            return;
        }

        funcName = input[0].ToUpperInvariant();

        if (input.Length == 1)
            funcVal = "";
        else
            funcVal = input[1].ToUpperInvariant();

        int result = Functions.instance.ExecuteFunction(funcName, funcVal);

        if (result != 0)
            ShowError(result);
        
        ClearInput();
    }

    void TerminalExit()
    {
        terInput.DeactivateInputField();
        ClearInput();
        ClearMainScreen();

        isWorking = false;
        controllerChange = false;
        TerAnim();
    }

    void ShowError(int errorVal)
    {
        if (errorVal == -1)
            mainText.text = "Wrong Command";
        else if (errorVal == -2)
            mainText.text = "Enter function and parameter";

        mainText.text += "\nType 'help'or for more information";
    }

    string[] SplitString(string input) => input.Split(char.Parse(" "));

    public static void ShowMainScreen(string screenText) => mainText.text = screenText;

    void ClearMainScreen() => mainText.text = "";

    void ClearInput() => terInput.text = "";

    #region DefaultFunctions

    void AddDefaultFunctions()
    {
        Functions.instance.AddFunction("help", TerHelp);
    }

    int TerHelp(dynamic empty)
    {
        ClearMainScreen();
        
        string commands = Functions.instance.GetFunctions();

        commands = "Commands :\n" + commands;

        ShowMainScreen(commands);

        return 0;
    }
    
    #endregion

    IEnumerator WelcomeScreen()
    {
        ClearMainScreen();
        ClearInput();

        startScreen.SetActive(true);
        terInput.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        terInput.gameObject.SetActive(true);
        startScreen.SetActive(false);
    }
}


/*
[HideInInspector] public Transform terminalPos;

Canvas terCanvas;
TMP_InputField terInput;
Light terLight;
[SerializeField] TMP_Text mainScreen;
[SerializeField] AudioClip[] audioClips;
Animation terAnimaton;
AudioSource terAudio;

public bool isWorking = false;
bool isPaused = false;
bool isSubmit = false;
bool isOpened = false;
float openingTime = 1f;

public static Dictionary<string, System.Action> functions = new Dictionary<string, System.Action>();
public static Dictionary<string, System.Action<dynamic>> functionsInt = new Dictionary<string, System.Action<dynamic>>();

public delegate void DelegateVoid();
public delegate void DelegateVoidInt(dynamic a);

void Awake()
{

    terCanvas = transform.GetComponentInChildren<Canvas>();
    terInput = terCanvas.GetComponentInChildren<TMP_InputField>();
    terLight = GetComponentInChildren<Light>();
    terAnimaton = GetComponent<Animation>();
    terAudio = GetComponent<AudioSource>();
}

void Start()
{
    terminalPos = transform.Find("TerminalPos").GetComponent<Transform>();

    terCanvas.gameObject.SetActive(false);
    terLight.enabled = false;

    AddFunction("Help", TerHelp);
    AddFunction("Commands", TerCommands);
    AddFunction("Clear", TerClear);
    AddFunction("Exit", TerExit);
}

void Update()
{
    if (isWorking)
    {
        if (Input.GetMouseButtonDown(0))
        {
            PauseTer();
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            TerExit();
            return;
        }

        TerminalUpdate();
    }
}

#region Terminal Default Functions

void TerminalUpdate()
{
    if (Input.GetButtonDown("Submit"))
    {
        isSubmit = true;
        mainScreen.text = "";
        TerSubmit();
    }

    if (!terInput.isFocused)
        terInput.ActivateInputField();

}

void TerSubmit()
{
    string[] input = SplitString(terInput.text);

    string funcName = input[0].ToUpperInvariant();

    foreach (var function in functions)
    {
        if(function.Key == funcName)
        {
            function.Value.Invoke();
            terInput.text = "";
            return;
        }
    }

    WrongCommand();
}


void WrongCommand()
{
    mainScreen.text = "Wrong command ('help')";
    terInput.text = "";
    return;
}

void TerHelp() => mainScreen.text = "Enter 'commands'";

void TerClear() => mainScreen.text = "";

void TerCommands()
{
    foreach (var commands in functions)
    {
        mainScreen.text += commands.Key + "\n";
    }
}

public void StartTer()
{

    if (isPaused)
    {
        isPaused = false;
        isWorking = true;
    }
    else
    {
        TerAnimation();
        mainScreen.text = "";
    }
    StartCoroutine(TerminalStartScreen());
}



private void PauseTer()
{
    isPaused = true;
    isWorking = false;
    terInput.DeactivateInputField();
    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ExitTerminal();
}

private void TerExit()
{
    isWorking = false;
    terInput.DeactivateInputField();
    terInput.text = "";
    terCanvas.gameObject.SetActive(false);
    terLight.enabled = false;
    TerAnimation();
    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ExitTerminal();
}

IEnumerator TerminalStartScreen()
{
    isWorking = false;
    yield return new WaitForSeconds(openingTime);
    terCanvas.gameObject.SetActive(true);
    isWorking = true;
    terLight.enabled = true;
}

void TerAnimation()
{
    if (terAnimaton.isPlaying)
        return;

    if (isOpened)
    {
        terAnimaton["terminalAnim"].speed = 1;
        terAudio.clip = audioClips[0];
    }
    else
    {
        terAudio.clip = audioClips[1];
        terAnimaton["terminalAnim"].time = terAnimaton["terminalAnim"].length;
        terAnimaton["terminalAnim"].speed = -1;
    }

    terAudio.Play();
    terAnimaton.Play();
    isOpened = !isOpened;
}

#endregion

/// <param name="funcName">Fuction Name</param>
/// <param name="del">Void Fuction without Parameters</param>
public static void AddFunction(string funcName, DelegateVoid del)
{
    funcName = funcName.ToUpperInvariant();
    foreach (string item in functions.Keys)
    {
        if (item.ToUpperInvariant() == funcName)
            return;
    }
    functions.Add(funcName.ToUpperInvariant(), () => del());
}

public static void AddFunction(string funcName, DelegateVoidInt del, dynamic a)
{
    a =12;
    AddFunction("asd", asdasd, 12);
    funcName = funcName.ToUpperInvariant();
    foreach (string item in functions.Keys)
    {
        if (item.ToUpperInvariant() == funcName)
            return;
    }
    functions.Add(funcName.ToUpperInvariant(), () => del(a));
}

private static void AddFunction(string v1, Action asdasd, int v2)
{
    throw new NotImplementedException();
}

private static void asdasd()
{
    throw new NotImplementedException();
}

void asdasd(string asd)
{

}

string[] SplitString(string input)
{
    return input.Split(char.Parse(" "));
}
*/
