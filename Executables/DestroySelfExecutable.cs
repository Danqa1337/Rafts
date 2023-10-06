public class DestroySelfExecutable : Executable
{
    public float delay;
    public override void Execute()
    {
        Destroy(gameObject, delay);
        base.Execute();
    }
}
