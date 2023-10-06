using System.Collections.Generic;
using UnityEngine;

public abstract class Executable : MonoBehaviour
{
    public virtual void Execute()
    {
        foreach (var item in executables)
        {
            item.Execute();
        }
    }
    public List<Executable> executables;
}

public abstract class ExecutableWithTarget : Executable
{

    public override void Execute()
    {
        base.Execute();
    }
    public virtual void Execute(GameObject target)
    {
        foreach (var item in executables)
        {
            if (item is ExecutableWithTarget executableWT)
            {
                executableWT.Execute(target);
            }
            else
            {
                item.Execute();
            }
        }
    }

}
