using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    //Get all doors in scene
    List<Door> doors;

    private void Awake() => doors = new List<Door>();

    private void Start()
    {
        doors.AddRange(FindObjectsOfType<Door>());
        
        foreach (Door door in doors)
        {
            door.doorName = door.doorName.ToUpperInvariant();
        }
        
        //Add functions for reach from terminal
        Functions.instance.AddFunction("openDoor", OpenDoor);
        Functions.instance.AddFunction("closeDoor", CloseDoor);
        Functions.instance.AddFunction("getDoors", GetDoors);
    }

    int OpenDoor(dynamic _doorName) => SetDoor(_doorName, true) ? 0 : -1;

    int CloseDoor(dynamic _doorName) => SetDoor(_doorName, false) ? 0 : -1;
    
    bool SetDoor(string _doorName, bool OC)
    {
        string dName = _doorName.ToUpperInvariant();
        foreach (Door door in doors)
        {
            if (door.doorName == _doorName)
            {
                if(door.isLocked)
                {
                    TerminalController.ShowMainScreen("Door is locked");
                    return true;
                }
                door.gameObject.GetComponent<Animator>().SetBool("OpenDoor",OC);
                return true;
            }
        }
        return false;
    }

    int GetDoors(dynamic empty)
    {
        string doorNames = "";

        foreach (Door door in doors)
        {
            doorNames += door.doorName + "\n";
        }

        TerminalController.ShowMainScreen(doorNames);
        return 0;
    }

}
