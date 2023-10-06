using Unity.Entities;
[UpdateAfter(typeof(AbilitySystem))]
public partial class SyncAbilitySystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBufferParallel();
        var delay = GameManager.instance != null ? GameManager.instance.orderCancelDelay / 1000f + 100 : 1;
        Entities.WithAll<SyncAbilityComponent>().ForEach((int entityInQueryIndex, Entity entity, DynamicBuffer<CrewElement> crewElements, ref SyncAbilityComponent syncAbilityComponent, ref RaftComponent raftComponent) =>
        {
            if (raftComponent.avgCrewCooldown <= delay)
            {
                ecb.RemoveComponent<SyncAbilityComponent>(entityInQueryIndex, entity);

                for (int i = 0; i < crewElements.Length; i++)
                {

                    var sailor = crewElements[i].entity;
                    if (!sailor.HasComponent<DeadTag>())
                    {
                        ecb.RemoveComponent<WaitingForSyncTag>(entityInQueryIndex, sailor);
                        ecb.RemoveComponent<OrderComponent>(entityInQueryIndex, sailor);
                        ecb.AddComponent(entityInQueryIndex, sailor, syncAbilityComponent.abilityComponent);
                    }

                }

            }

        }).WithoutBurst().ScheduleParallel(); ;

        UpdateECB();

    }

}
public struct WaitingForSyncTag : IComponentData
{

}
