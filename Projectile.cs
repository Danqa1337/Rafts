using Unity.Entities;
using UnityEngine;
public class Projectile : ExecutableWithTarget
{
    public float speed;
    public float damageRadius;
    public bool damageMultipleTargets;
    public PoolableItem particlesOnHit;
    public PoolableItem particlesOnSplash;
    public void Launch(Entity sender, Vector2 from, Vector2 to, Transform excludedRoot)
    {

        var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();

        entity.SetName("Projectile");
        entity.AddComponentObject(transform);

        var direction = (to - from).normalized;

        entity.AddComponentData(new ProjectileComponent(sender, from, to, direction, speed));
        entity.AddComponentObject(this);
    }

}
