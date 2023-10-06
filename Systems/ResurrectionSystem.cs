using Unity.Entities;
using UnityEngine;
public partial class ResurrectionSystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        Entities.WithAll<ResurrectTag>().ForEach((Entity entity, Animator animator, BoxCollider2D collider2D, Transform transform, ref CooldownComponent cooldownComponent) =>
        {
            ecb.RemoveComponent<ResurrectTag>(entity);
            ecb.RemoveComponent<DeadTag>(entity);

            animator.SetTrigger("Resurrect");
            transform.GetComponent<SpriteRenderer>().sortingLayerName = "Sailors";
            collider2D.enabled = true;
            cooldownComponent.cooldown = 3;
            ecb.AddComponent(entity, new HealthChangedTag());
            ecb.AppendToBuffer(entity, new HealthChangedElement(999));

            if (entity.HasComponent<Raft>())
            {
                entity.GetComponentObject<Raft>().sailorsAlive++;
                if (entity.HasComponent<PlayerTag>()) ViewModel.instance.SailorsCount++;
            }

        }).WithoutBurst().Run();
        UpdateECB();
    }
}

public struct ResurrectTag : IComponentData
{

}

