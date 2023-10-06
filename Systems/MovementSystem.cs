using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class MovementSystem : MySystemBase
{

    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        Entities.WithAll<SailorAIComponent>().WithNone<DestroyTag>().ForEach((Entity entity, Transform transform, ref MoveComponent moveComponent) =>
        {
            var sqDistance = (moveComponent.newLocalPosition - transform.localPosition.ToFloat2()).SqrMagnitude();

            if (sqDistance > 0.1f)
            {
                var newPos = (transform.position + ((moveComponent.newLocalPosition.ToVector3() - transform.localPosition).normalized * moveComponent.speedMultipler * Time.DeltaTime));

                transform.position = newPos;

            }
            else
            {
                if (entity.HasComponent<Animator>())
                {
                    entity.GetComponentObject<Animator>().SetTrigger("StopWalk");
                }
                ecb.RemoveComponent<MoveComponent>(entity);
            }

        }).WithoutBurst().Run();
        UpdateECB();
    }
}
public struct MoveComponent : IComponentData
{
    public float2 newLocalPosition;
    public float speedMultipler;
    public MoveComponent(float2 newLocalPosition)
    {
        this.newLocalPosition = newLocalPosition;
        this.speedMultipler = 1;
    }

    public MoveComponent(float2 newLocalPosition, float speedMultipler) : this(newLocalPosition)
    {
        this.speedMultipler = speedMultipler;
    }
}
