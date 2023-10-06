using Unity.Entities;
[DisableAutoCreation]
public partial class OrderSetSystem : MySystemBase
{
    public static OrderComponent currentOrder;
    public async static void SetOrder(OrderComponent abilityComponent)
    {
        currentOrder = abilityComponent;
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<OrderSetSystem>().Update();
        await System.Threading.Tasks.Task.Delay(GameManager.instance.orderCancelDelay);
    }

    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBufferParallel();
        Entities.WithAll<PlayerTag, SailorAIComponent>().WithNone<OrderComponent, AbilityComponent, WaitingForSyncTag>().ForEach((int entityInQueryIndex, Entity entity) =>
        {
            ecb.AddComponent(entityInQueryIndex, entity, currentOrder);

        }).WithoutBurst().ScheduleParallel();

        UpdateECB();
    }
}
[DisableAutoCreation]
public partial class OrderCancelSystem : MySystemBase
{
    public static OrderComponent currentOrder;
    public static void SetOrder(OrderComponent abilityComponent)
    {
        currentOrder = abilityComponent;
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<OrderSetSystem>().Update();

    }

    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBufferParallel();

        Entities.WithAll<OrderComponent, SailorAIComponent>().WithNone<AbilityComponent, WaitingForSyncTag>().ForEach((int entityInQueryIndex, Entity entity) =>
        {
            ecb.RemoveComponent<OrderComponent>(entityInQueryIndex, entity);

        }).WithBurst().ScheduleParallel();

        UpdateECB();
    }
}
