using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public partial class SortingOrderUpdateSystem : MySystemBase
{

    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        Entities.WithAll<UpdateSortingOrderTag, PositionChangedTag>().WithNone<DestroyTag, SleepTag>().ForEach((Entity entity, Transform transform) =>
        {
            ecb.RemoveComponent<PositionChangedTag>(entity);
            var sortingOrder = -(int)(transform.position.y * 100);
            var setSortingOrdecComponent = new SetSortingOrderComponent(sortingOrder);
            ecb.AddComponent(entity, setSortingOrdecComponent);

            if (entity.HasBuffer<Child>())
            {
                foreach (var item in entity.GetBuffer<Child>())
                {
                    ecb.AddComponent(item.Value, setSortingOrdecComponent);
                }
            }

        }).WithoutBurst().Run();

        UpdateECB();

    }
}
public partial class SortingOrderSetSystem : MySystemBase
{

    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        Entities.WithNone<DestroyTag, SleepTag, SortingLayerOffsetComponent>().ForEach((Entity entity, SpriteRenderer spriteRenderer, ref SetSortingOrderComponent setSortingOrderComponent) =>
        {
            ecb.RemoveComponent<SetSortingOrderComponent>(entity);
            spriteRenderer.sortingOrder = setSortingOrderComponent.sortingOrder;

        }).WithoutBurst().Run();
        Entities.WithNone<DestroyTag, SleepTag>().ForEach((Entity entity, SpriteRenderer spriteRenderer, ref SetSortingOrderComponent setSortingOrderComponent, ref SortingLayerOffsetComponent sortingLayerOffsetComponent) =>
        {
            ecb.RemoveComponent<SetSortingOrderComponent>(entity);
            spriteRenderer.sortingOrder = setSortingOrderComponent.sortingOrder + sortingLayerOffsetComponent.offset;

        }).WithoutBurst().Run();
        UpdateECB();

    }
}

public partial class CheckTransformChangeSystem : MySystemBase
{
    private static EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        m_EndSimulationEcbSystem = World
        .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {

        Entities.WithAll<LocalToWorld>().WithNone<DestroyTag, SleepTag, PositionChangedTag>().ForEach((Entity entity, LocalToWorld localToWorld, ref LastLocalToWorld lastLocalToWorld) =>
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (lastLocalToWorld.Value[x][y] != localToWorld.Value[x][y])
                    {

                        lastLocalToWorld.Value = localToWorld.Value;
                        lastLocalToWorld.changed = true;
                        break;
                    }

                }
            }

        }).ScheduleParallel();

    }
}
[UpdateBefore(typeof(CheckTransformChangeSystem))]
public partial class RegisterPositionChangeSystem : MySystemBase
{

    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBufferParallel();
        Entities.ForEach((int entityInQueryIndex, Entity entity, ref LastLocalToWorld lastLocalToWorld) =>
        {
            if (lastLocalToWorld.changed)
            {
                lastLocalToWorld.changed = false;
                ecb.AddComponent(entityInQueryIndex, entity, new PositionChangedTag());
            }

        }).WithBurst().ScheduleParallel();

        UpdateECB();

    }
}

public struct UpdateSortingOrderTag : IComponentData
{

}
public struct PositionChangedTag : IComponentData
{

}
public struct SortingLayerOffsetComponent : IComponentData
{
    public int offset;

    public SortingLayerOffsetComponent(int offset)
    {
        this.offset = offset;
    }
}
public struct SetSortingOrderComponent : IComponentData
{
    public int sortingOrder;

    public SetSortingOrderComponent(int sortingOrder)
    {
        this.sortingOrder = sortingOrder;
    }
}
public struct LastLocalToWorld : IComponentData
{
    public float4x4 Value;
    public bool changed;
}

