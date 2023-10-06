using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
public partial class DestructionSystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        Entities.WithAll<DestroyTag>().ForEach((Entity entity, Transform transform) =>
        {
            ecb.DestroyEntity(entity);
            if (entity.HasComponent<SailorAIComponent>())
            {
                foreach (var item in entity.GetBuffer<Child>())
                {
                    ecb.DestroyEntity(item.Value);

                }
                var explosion = Pooler.Take("Explosion", transform.position);
                MonoBehaviour.Destroy(transform.gameObject);

            }
        }).WithoutBurst().Run();
    }
}

public struct DestroyTag : IComponentData
{

}