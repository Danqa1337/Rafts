public class ChangeObjectLayerExecutable : Executable
{
    public int layer;
    public async override void Execute()
    {
        gameObject.layer = layer;
        base.Execute();
    }

}
