using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public partial class ProjectilesSystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        Entities.ForEach((Entity entity, Transform transform, Projectile projectile, ref ProjectileComponent projectileComponent) =>
        {
            var displacement = (projectileComponent.direction * Time.DeltaTime * projectileComponent.speed);

            var newPosition = transform.position + displacement.ToVector3();
            var sqrMagnitude = (projectileComponent.destination - transform.position.ToFloat2()).SqrMagnitude();

            if (sqrMagnitude > 0.2)
            {
                var rendererTransform = transform.GetChild(0).transform;
                var shadowTransform = transform.GetChild(1).transform;
                var parabolicDispalcement = new float2(0, AnimationCurves.instance.parabola.Evaluate(sqrMagnitude / projectileComponent.sqrDistance) * projectileComponent.distance);

                var parabolicDirection = transform.position + parabolicDispalcement.ToVector3() + displacement.ToVector3() - (rendererTransform.position);
                var scale = 1 + parabolicDispalcement.y / projectileComponent.distance;

                float angle = Mathf.Atan2(parabolicDirection.y, parabolicDirection.x) * Mathf.Rad2Deg - 90;

                transform.position += displacement.ToVector3();
                rendererTransform.position = transform.position + parabolicDispalcement.ToVector3();
                rendererTransform.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                rendererTransform.localScale = new Vector3(scale, scale, scale);
                shadowTransform.localScale = new Vector3(scale, scale, scale) / 2;
            }
            else
            {
                var sailors = Physics2D.OverlapCircleAll(projectileComponent.destination, projectile.damageRadius, LayerMask.GetMask("Sailors"));

                if (sailors.Length > 0)
                {
                    foreach (var sailor in sailors)
                    {

                        if (sailor != null && (!projectileComponent.sender.Exists() || projectileComponent.sender.HasComponent<DestroyTag>() || (sailor.transform.root != projectileComponent.sender.GetComponentObject<Transform>().root)))
                        {
                            projectile.Execute(sailor.gameObject);

                            var link = sailor.GetComponent<EntityLink>();
                            ecb.AppendToBuffer(link.entity, new HealthChangedElement(-1));
                            ecb.AddComponent(link.entity, new HealthChangedTag());

                        }

                        if (!projectile.damageMultipleTargets)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    var raft = Physics2D.OverlapCircle(projectileComponent.destination, 0.5f, LayerMask.GetMask("RaftFloors"));
                    if (raft != null)
                    {
                        Pooler.Take(projectile.particlesOnHit, transform.position);
                    }
                    else
                    {
                        Pooler.Take(projectile.particlesOnSplash, transform.position);
                    }

                }
                ecb.DestroyEntity(entity);
                Pooler.PutObjectBackToPool(transform.gameObject);

            }

        }).WithoutBurst().Run();
        UpdateECB();
    }

}
public struct ProjectileComponent : IComponentData
{
    public Entity sender;
    public float2 start;
    public float2 destination;
    public float2 direction;
    public float sqrDistance;
    public float distance;
    public float speed;

    public ProjectileComponent(Entity entity, float2 start, float2 destination, float2 direction, float speed)
    {
        this.sender = entity;
        this.start = start;
        this.destination = destination;
        this.direction = direction;
        this.speed = speed;
        this.sqrDistance = (destination - start).SqrMagnitude();
        this.distance = (destination - start).Magnitude();
    }
}
