using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommand : Command
{
    public string name;

    public InputCommand(string name, Action executes) : base(executes)
    {
        this.name = name;
    }
}
