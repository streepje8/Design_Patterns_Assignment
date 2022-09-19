using System;

public class Command
{
    private Action onExecute;
    public Command(Action executes)
    {
        onExecute = executes;
    }

    public void Execute()
    {
        onExecute.Invoke();
    }
}
