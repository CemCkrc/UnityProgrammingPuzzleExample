using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalController : MonoBehaviour
{
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

    public delegate void DelegateVoid();

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
        mainScreen.text = "Wrong command";
        terInput.text = "";
        return;
    }
    
    void TerHelp()
    {
        mainScreen.text = "Enter 'commands'";
    }

    void TerCommands()
    {
        foreach (var commands in functions)
        {
            mainScreen.text += commands.Key + "\n";
        }
    }

    void TerClear()
    {
        mainScreen.text = "";
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
    
    public static void AddFunction(string funcName, DelegateVoid del)
    {
        functions.Add(funcName.ToUpperInvariant(), () => del());
    }
    
    string[] SplitString(string input)
    {
        return input.Split(char.Parse(" "));
    }
}
