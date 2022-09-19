using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public bool blockInputs = false;
    private Dictionary<KeyCode, List<Command>> bindings = new Dictionary<KeyCode, List<Command>>();

    public void BindKey(KeyCode code, Command binding)
    {
        if (!bindings.ContainsKey(code)) bindings.Add(code,new List<Command>()); 
        bindings[code].Add(binding);
    }

    public void Unbind(Command c)
    {
        foreach (var kv in bindings.Where(kv => kv.Value.Contains(c)))
        {
            kv.Value.Remove(c);
        }
    }

    private void Update()
    {
        if(!blockInputs)
            foreach (KeyCode code in bindings.Keys.Where(Input.GetKeyDown))
                bindings[code].ForEach(x => x.Execute());
    }
}
