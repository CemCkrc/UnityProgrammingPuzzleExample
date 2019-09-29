using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    public static Functions instance { get; private set; }

    private void Awake() => instance =  instance ? instance : this;
    
    private Dictionary<string, Func<dynamic, int>> functions = new Dictionary<string, Func<dynamic, int>>();
    
    public void AddFunction(string funcName, Func<dynamic, int> function)
    {
        funcName = funcName.ToUpperInvariant();

        foreach (string fName in functions.Keys)
        {
            if (fName == funcName)
                return;
        }

        functions.Add(funcName, function);
    }

    public int ExecuteFunction(string funcName, dynamic input)
    {
        funcName = funcName.ToUpperInvariant();

        foreach (var function in functions)
        {
            if (function.Key == funcName)
            {
                return function.Value(input);
            }
        }
        return -1;
    }

    public bool DeleteFunction(string funcName)
    {
        //TODO: find & delete function 
        return true;
    }
    
    public string GetFunctions()
    {
        string allFunctions = "";

        foreach (var function in functions.Keys)
        {
            allFunctions += function + "\n";
        }
        return allFunctions;
    }
}