using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
public class RaftConversion : PlatformConversion
{
    public override Entity Convert()
    {
        var entity = base.Convert();

        if (GetComponent<PlayersRaft>() != null)
        {
            name = "Players Raft";
            entity.SetZeroSizedTagComponentData(new PlayerTag());
        }
        entity.SetName(name);
        GetComponent<EntityLink>().entity = entity;
        entity.AddComponentObject(GetComponent<Raft>());
        entity.AddComponentObject(GetComponent<Rigidbody2D>());
        entity.AddComponentObject(GetComponentInChildren<NavMeshAgent>());
        entity.AddComponentData(new RaftComponent());
        entity.AddBuffer<CrewElement>();
        entity.AddBuffer<PushElement>();
        entity.AddBuffer<EffectElement>();

        return entity;
    }

}
public struct CrewElement : IBufferElementData
{
    public Entity entity;

    public CrewElement(Entity entity)
    {
        this.entity = entity;
    }
}

