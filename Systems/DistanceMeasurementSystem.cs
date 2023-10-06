using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public partial class DistanceMeasurementSystem : MySystemBase
{
    private static float2 playerPos;
    protected override void OnUpdate()
    {
        playerPos = PlayersRaft.instance != null ? PlayersRaft.instance.transform.position.ToFloat2() : CameraFolow.instance.transform.position.ToFloat2();

        Entities.ForEach((Entity entity, ref DistanceComponent distanceComponent, ref LocalToWorld localToWorld) =>
        {
            distanceComponent.DistanceToPlayer = (playerPos - localToWorld.Position.ToFloat2()).Magnitude();

        }).WithoutBurst().ScheduleParallel();

    }
}

public partial class WakeUpSystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        var playerPos = PlayersRaft.instance != null ? PlayersRaft.instance.transform.position.ToFloat2() : float2.zero;

        Entities.ForEach((Entity entity, Animator animator, ref DistanceComponent distanceComponent) =>
        {

            if (distanceComponent.DistanceToPlayer < 50)
            {
                if (entity.HasComponent<SleepTag>())
                {
                    animator.enabled = true;
                    ecb.RemoveComponent<SleepTag>(entity);
                }
            }
            else
            {
                if (!entity.HasComponent<SleepTag>())
                {
                    if (entity.HasComponent<Animator>())
                    {
                        animator.enabled = false;
                    }
                    ecb.AddComponent(entity, new SleepTag());
                }
            }

        }).WithoutBurst().Run();

        UpdateECB();
    }
}

public struct SleepTag : IComponentData
{

}
public struct DistanceComponent : IComponentData
{
    public float DistanceToPlayer;
}
