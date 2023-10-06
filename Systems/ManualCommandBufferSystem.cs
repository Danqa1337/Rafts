using Unity.Entities;

[DisableAutoCreation]
public partial class ManualCommandBufferSystem : EntityCommandBufferSystem
{

    public static ManualCommandBufferSystem Instance => World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ManualCommandBufferSystem>();
    public static void PlayBack()
    {

        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ManualCommandBufferSystem>().Update();
    }
}
