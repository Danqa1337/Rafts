using Unity.Entities;

public abstract partial class MySystemBase : SystemBase
{
    private static ManualCommandBufferSystem _manualCommanBufferSytem;
    protected override void OnCreate()
    {
        base.OnCreate();
        _manualCommanBufferSytem = ManualCommandBufferSystem.Instance;
    }

    protected EntityCommandBuffer CreateEntityCommandBuffer()
    {
        return _manualCommanBufferSytem.CreateCommandBuffer();
    }
    protected EntityCommandBuffer.ParallelWriter CreateEntityCommandBufferParallel()
    {
        return _manualCommanBufferSytem.CreateCommandBuffer().AsParallelWriter();
    }
    protected void UpdateECB()
    {

        Dependency.Complete();
        _manualCommanBufferSytem.Update();
    }

}
