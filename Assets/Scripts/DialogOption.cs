using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogOption
{
    public string option;
    public string response;
    public bool isTemporary;
    public InteractiveHumanoid target;
}
