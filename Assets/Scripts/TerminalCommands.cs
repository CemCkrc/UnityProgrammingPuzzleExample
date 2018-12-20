using System;
using System.Collections.Generic;
using UnityEngine;

public class TerminalCommands
{
    //Terminal Commands
    public static readonly List<string> computerCommands = new List<string>()
    {
        "help","restart","clear","list","connect","functions","do","logs","exit"
    };

    //Terminal 'help' command
    public static readonly string helpCommand = "Commands:\n'restart' : Restart terminal\n'exit' : Exit terminal\n" +
            "'list' : List objects which terminal can connect\nobject + 'list' : List objects which connected an object\n" +
        "'connect' + object : Connect object to terminal\nobject + 'connect' + object : Connect two object using terminal\n" +
        "'functions' : Connected object's properties\nobject + 'functions' : Object's properties\n" +
        "'do' + function : Perform the task of connected object\n'logs' : Show computer's data\n" +
        "'clear' : Clear the terminal screen\n";

    //Terminal log
    public static readonly string log = "Videoyun\n";
}