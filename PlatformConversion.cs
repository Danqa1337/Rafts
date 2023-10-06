using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
[RequireComponent(typeof(EntityLink))]

public class PlatformConversion : MonoBehaviour
{
    public Entity mechanismEntity;
    public virtual Entity Convert()
    {
        var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
        var bounds = GetComponentInChildren<PlatformBounds>().GetBounds();

        if (GetComponent<PlayersRaft>() != null)
        {
            name = "Players Raft";
            entity.SetZeroSizedTagComponentData(new PlayerTag());
        }

        entity.SetName(name);
        GetComponent<EntityLink>().entity = entity;
        entity.AddComponentObject(transform);
        entity.AddComponentData(new LocalToWorld());
        entity.AddComponentData(new BoundsComponent(bounds));
        var mechanism = transform.GetComponentInChildren<Mortar>();
        if (mechanism != null)
        {
            mechanismEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            mechanismEntity.SetName(mechanism.name);
            mechanismEntity.AddComponentObject(mechanism.transform);
            mechanismEntity.AddComponentObject(mechanism);
            mechanismEntity.AddComponentData(new LocalToWorld());
            mechanismEntity.AddComponentData(new LocalToParent());
            mechanismEntity.AddComponentData(new Parent() { Value = entity });
            mechanismEntity.AddComponentData(new CooldownComponent());
        }

        return entity;
    }
}
public struct BoundsComponent : IComponentData
{
    public Bounds bounds;

    public BoundsComponent(Bounds bounds)
    {
        this.bounds = bounds;
    }
}
