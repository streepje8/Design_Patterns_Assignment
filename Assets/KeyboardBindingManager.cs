using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class KeyboardBindingManager : MonoBehaviour
{
    public List<InputCommand> allCommands = new List<InputCommand>()
    {
        new InputCommand("BlueNote", () => GameController.Instance.BlueKey()),
        new InputCommand("RedNote", () => GameController.Instance.RedKey()),
        new InputCommand("PrintRecording", () => GameController.Instance.PrintRecordedData())
    };
    private List<InputCommand> rebindQueue = new List<InputCommand>();
    private InputHandler handler;
    private bool IsRebinding = false;
    void Start()
    {
        handler = GetComponent<InputHandler>();
        handler.BindKey(KeyCode.R, new Command(() =>
        {
            if (!IsRebinding)
            {
                Debug.Log("Starting Key Rebind");
                IsRebinding = true;
                allCommands.ForEach(cmd =>
                {
                    handler.Unbind(cmd);
                    rebindQueue.Add(cmd);
                });
                Debug.Log("Press a key to bind: " + rebindQueue[0].name);
                handler.blockInputs = true;
            }
        }));
    }

    
    void Update()
    {
        if (IsRebinding)
        {
            if(rebindQueue.Count > 0)
            {
                if (Input.anyKeyDown)
                {
                    string key = Input.inputString[Input.inputString.Length - 1].ToString().ToUpper();
                    if (key.Equals("R", StringComparison.Ordinal)) return;
                    KeyCode code = (KeyCode)Enum.Parse(typeof(KeyCode), key);
                    handler.BindKey(code,rebindQueue[0]);
                    rebindQueue.RemoveAt(0);
                    if (rebindQueue.Count > 0)
                    {
                        Debug.Log("Press a key to bind: " + rebindQueue[0].name);
                    }
                    else
                    {
                        Debug.Log("Finished Rebinding Keys!");
                        IsRebinding = false;
                        handler.blockInputs = false;
                    }
                }
            } else
            {
                Debug.Log("Finished Rebinding Keys!");
                IsRebinding = false;
                handler.blockInputs = false;
            }
        }
    }
}
