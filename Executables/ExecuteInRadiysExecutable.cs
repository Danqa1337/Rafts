using UnityEngine;

public class ExecuteInRadiysExecutable : ExecutableWithTarget
{
    public float radius;
    public LayerMask layerMask;
    public override void Execute(GameObject target)
    {
        Execute();
    }
    public override void Execute()
    {

        var colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        Debug.Log("mine explode, hit " + colliders.Length);
        foreach (var item in executables)
        {
            if (item is ExecutableWithTarget withTarget)
            {
                foreach (var collider in colliders)
                {
                    withTarget.Execute(collider.gameObject);
                }

            }
        }

    }
}
