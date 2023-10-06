using Unity.Entities;
using Unity.Mathematics;

public enum Order
{
    Push,
    Attack,
}
[UpdateBefore(typeof(AbilitySystem))]
public partial class OrderExecuteSystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBufferParallel();

        Entities.WithAll<OrderComponent, ReadyTag>().WithNone<WaitingForSyncTag>().ForEach((int entityInQueryIndex, Entity entity, ref SailorAIComponent sailorAIComponent, ref OrderComponent orderComponent) =>
        {

            var abilityComponent = new AbilityComponent();

            switch (orderComponent.order)
            {
                case Order.Push:

                    abilityComponent = new AbilityComponent(Ability.WaitForSyncPush, orderComponent.vector);

                    break;
                case Order.Attack:

                    var allowedAbilities = entity.GetBuffer<AllowedAbilityElement>().Reinterpret<Ability>();
                    if (allowedAbilities.Contans(Ability.Trust))
                    {
                        abilityComponent = new AbilityComponent(Ability.Trust, orderComponent.vector);
                    }
                    else
                    {
                        abilityComponent = new AbilityComponent(Ability.Throw, orderComponent.vector);

                    }

                    break;
                default:
                    break;
            }
            ecb.AddComponent(entityInQueryIndex, entity, abilityComponent);
            ecb.RemoveComponent<ReadyTag>(entityInQueryIndex, entity);
            ecb.RemoveComponent<OrderComponent>(entityInQueryIndex, entity);

        }).WithoutBurst().ScheduleParallel();
        UpdateECB();
    }

}
public struct OrderComponent : IComponentData
{
    public Order order;
    public float2 vector;

    public OrderComponent(Order order, float2 vector)
    {
        this.order = order;
        this.vector = vector;
    }
}
