using UnityEngine;

public class SpawnPoolableExecutable : Executable
{
    public PoolableItem poolableItem;
    public Vector3 offset;
    public override void Execute()
    {

        var item = Pooler.Take(poolableItem, transform.position + offset);

        base.Execute();
    }
}
