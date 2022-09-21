using System;
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
    private bool isRebinding = false;
    void Start()
    {
        handler = GetComponent<InputHandler>();
        handler.BindKey(KeyCode.R, new Command(() =>
        {
            if (!isRebinding)
            {
                Debug.Log("Starting Key Rebind");
                isRebinding = true;
                allCommands.ForEach(cmd =>
                {
                    handler.Unbind(cmd);
                    rebindQueue.Add(cmd);
                });
                Debug.Log("Press a key to bind: " + rebindQueue[0].name);
                handler.blockInputs = true;
            }
        }));
        
        //Setup default keybinds
        handler.BindKey(KeyCode.F, allCommands[0]);
        handler.BindKey(KeyCode.J, allCommands[1]);
        handler.BindKey(KeyCode.P, allCommands[2]);
    }

    
    void Update()
    {
        if (isRebinding)
        {
            RebindKey();
        }
    }

    private void RebindKey()
    {
        if (rebindQueue.Count > 0)
        {
            if (Input.anyKeyDown)
            {
                string key = Input.inputString[Input.inputString.Length - 1].ToString().ToUpper();
                if (key.Equals("R", StringComparison.Ordinal)) return;
                KeyCode code = (KeyCode)Enum.Parse(typeof(KeyCode), key);
                handler.BindKey(code, rebindQueue[0]);
                rebindQueue.RemoveAt(0);
                if (rebindQueue.Count > 0)
                {
                    Debug.Log("Press a key to bind: " + rebindQueue[0].name);
                }
                else
                {
                    FinishRebind();
                }
            }
        }
        else
        {
            FinishRebind();
        }
    }

    private void FinishRebind()
    {
        Debug.Log("Finished Rebinding Keys!");
        isRebinding = false;
        handler.blockInputs = false;
    }
}
