using Unity.Entities;
using UnityEngine;
public partial class DeathSystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        Entities.WithAll<DieTag>().WithNone<PlayerTag>().ForEach((Entity entity, ref XpComponent xpComponent) =>
        {
            ecb.RemoveComponent<XpComponent>(entity);
            XPManager.AddXP(xpComponent.XP);

        }).WithoutBurst().Run(); ;
        UpdateECB();
        ecb = CreateEntityCommandBuffer();
        Entities.WithAll<DieTag>().ForEach((Entity entity, Animator animator, BoxCollider2D collider2D, Transform transform) =>
        {

            animator.SetTrigger("Die");
            transform.GetComponent<SpriteRenderer>().sortingLayerName = "Corpses";
            collider2D.enabled = false;
            ecb.RemoveComponent<DieTag>(entity);
            ecb.AddComponent(entity, new DeadTag());
            var explosion = Pooler.Take("Explosion", transform.position);
            if (entity.HasComponent<OnPlatformEntityComponent>())
            {

                if (entity.GetComponentData<OnPlatformEntityComponent>().platform.HasComponent<Raft>())
                {
                    var raft = entity.GetComponentData<OnPlatformEntityComponent>().platform.GetComponentObject<Raft>();
                    raft.sailorsAlive--;
                    if (raft.sailorsAlive == 0)
                    {
                        ecb.AddComponent(entity.GetComponentData<OnPlatformEntityComponent>().platform, new SinkTag());
                        if (entity.HasComponent<PlayerTag>()) ViewModel.instance.SailorsCount--;

                    }

                }
            }

        }).WithoutBurst().Run();
        UpdateECB();
    }
}

public struct XpComponent : IComponentData
{
    public int XP;

    public XpComponent(int xP)
    {
        XP = xP;
    }
}

public struct DieTag : IComponentData
{

}
public struct DeadTag : IComponentData
{

}
