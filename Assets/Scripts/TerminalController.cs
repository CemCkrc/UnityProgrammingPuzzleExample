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