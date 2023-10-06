using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public partial class RaftSystem : MySystemBase
{
    protected override void OnUpdate()
    {

        Entities.WithAll<RaftComponent>().WithNone<DestroyTag>().ForEach((Entity entity, ref RaftComponent raftComponent, ref DynamicBuffer<CrewElement> crew) =>
        {
            raftComponent.velocity = entity.GetComponentObject<Rigidbody2D>().velocity;

            if (crew.Length > 0)
            {
                var commonCooldown = 0f;

                foreach (var item in crew)
                {
                    if (!item.entity.HasComponent<DeadTag>())
                    {
                        commonCooldown += item.entity.GetComponentData<CooldownComponent>().cooldown;

                    }
                }
                var avgCooldown = commonCooldown / crew.Length;
                raftComponent.avgCrewCooldown = avgCooldown;
                if (entity.HasComponent<PlayerTag>())
                {
                    ViewModel.instance.AvgCooldown = (Mathf.Max(0, (float)Math.Round(avgCooldown, 1)), 2);
                }
            }

        }).WithoutBurst().Run();

        var ecb = CreateEntityCommandBufferParallel();
        Entities.WithAll<RaftComponent, HasPushElementTag>().ForEach((int entityInQueryIndex, Entity entity, DynamicBuffer<PushElement> pushElements, ref RaftComponent raftComponent) =>
        {
            ecb.SetBuffer<PushElement>(entityInQueryIndex, entity);
            ecb.RemoveComponent<HasPushElementTag>(entityInQueryIndex, entity);
            var force = float2.zero;
            var raft = entity.GetComponentObject<Raft>();

            for (int i = 0; i < pushElements.Length; i++)
            {
                if ((force + pushElements[i].force + raftComponent.velocity).SqrMagnitude() < raft.maxVelocity * raft.maxVelocity)
                {
                    force += pushElements[i].force;
                }

            }

            var targetVelocity = (force + raftComponent.velocity).SqrMagnitude();
            if (targetVelocity < raft.minVelocity * raft.minVelocity)
            {
                force *= raft.minVelocity / targetVelocity;
            }
            raft.rb.AddForce(force * raft.pushMultipler);

            raft.WaveEmitter.transform.rotation = Quaternion.LookRotation(Vector3.forward, force.Normalize().ToVector3() * -1);
            raft.WaveEmitter.Play();
        }).WithoutBurst().Run();
        UpdateECB();
    }

}
public struct PushElement : IBufferElementData
{
    public float2 force;

    public PushElement(float2 force)
    {
        this.force = force;
    }
}
public struct HasPushElementTag : IComponentData
{

}

