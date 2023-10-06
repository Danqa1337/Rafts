using Unity.Entities;
using UnityEngine;
public partial class HealthSystem : MySystemBase
{

    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBufferParallel();
        Entities.WithAll<HealthChangedTag>().ForEach((int entityInQueryIndex, Entity entity, DynamicBuffer<HealthChangedElement> changeElements, ref HealthComponent healthComponent) =>
        {
            ecb.RemoveComponent<HealthChangedTag>(entityInQueryIndex, entity);

            foreach (var item in changeElements)
            {
                healthComponent.CurrentHealth = Mathf.Clamp(healthComponent.CurrentHealth + item.change, 0, healthComponent.MaxHealth);
            }

            if (healthComponent.CurrentHealth == 0)
            {
                ecb.AddComponent(entityInQueryIndex, entity, new DieTag());
            }
            ecb.SetBuffer<HealthChangedElement>(entityInQueryIndex, entity);

        }).WithBurst().ScheduleParallel();
        UpdateECB();

    }
}
public struct HealthChangedElement : IBufferElementData
{
    public int change;

    public HealthChangedElement(int change)
    {
        this.change = change;
    }
}
public struct HealthChangedTag : IComponentData
{

}

