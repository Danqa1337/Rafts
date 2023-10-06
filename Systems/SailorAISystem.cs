using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
[UpdateBefore(typeof(OrderExecuteSystem))]

public partial class SailorAISystem : MySystemBase
{
    public IAIBehaviour[] allBehaviours => new IAIBehaviour[]
    {
        new WaitBehaviour(),
        new RoamBehaviour(),
        new ThrowBehaviour(),
        new PushBehaviour(),
        new TrustBehaviour(),
        new ExplodeBehaviour(),
        new WaveHandsBehaviour(),
        new JumpBehaviour(),
        new SitBehaviour(),
        new ShootWithMortarBehaviour(),
        new EvadeBehaviour(),
    };
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBufferParallel();
        if (PlayersRaft.instance == null) return;
        var deltaTime = Time.DeltaTime;
        Entities.WithAll<CooldownComponent>().WithNone<DeadTag>().WithNone<SleepTag>().ForEach((int entityInQueryIndex, Entity entity, ref CooldownComponent cooldownComponent) =>
        {
            cooldownComponent.cooldown -= deltaTime;

            if (cooldownComponent.cooldown < 0)
            {
                ecb.AddComponent(entityInQueryIndex, entity, new ReadyTag());
            }

        }).WithBurst().Run();

        if (GameManager.currentGameState == GameState.Won || GameManager.currentGameState == GameState.Dead) return;

        UpdateECB();
        var ecb1 = CreateEntityCommandBuffer();

        Entities.WithAll<ReadyTag, OnPlatformEntityComponent>().WithNone<AbilityComponent, OrderComponent, WaitingForSyncTag>().ForEach((Entity entity, Transform transform, ref SailorAIComponent sailorAIComponent, ref DistanceComponent distanceComponent) =>
        {
            ecb1.RemoveComponent<ReadyTag>(entity);

            var data = new AIBehaviourData()
            {
                IsOnRaft = entity.GetComponentData<OnPlatformEntityComponent>().platform.HasComponent<RaftComponent>(),
                worldPosition = transform.position.ToFloat2(),
                localPosition = transform.localPosition.ToFloat2(),
                distanceToPlayer = distanceComponent.DistanceToPlayer,
                targetPosition = PlayersRaft.instance != null ? PlayersRaft.instance.transform.position.ToFloat2() : CameraFolow.instance.transform.position.ToFloat2(),
                IsPlayersSailor = entity.HasComponent<PlayerTag>(),
                platformPosition = (transform.position - transform.localPosition).ToFloat2(),
                aIComponent = sailorAIComponent,
                navMeshAgent = entity.GetComponentData<OnPlatformEntityComponent>().platform.HasComponent<RaftComponent>() ? entity.GetComponentData<OnPlatformEntityComponent>().platform.GetComponentObject<NavMeshAgent>() : null,
                bounds = entity.GetComponentData<OnPlatformEntityComponent>().platform.GetComponentData<BoundsComponent>().bounds,
                mainCaliberCooldown = entity.HasComponent<MaintainingMechanismComponent>() ? entity.GetComponentData<MaintainingMechanismComponent>().mechanism.GetComponentData<CooldownComponent>().cooldown : 999,
            };

            var topScore = 0f;
            var topBehaviourIndex = 0;
            var allowedabilities = entity.GetBuffer<AllowedAbilityElement>().Reinterpret<Ability>();
            var behaviours = new IAIBehaviour[allowedabilities.Length];
            for (int i = 0; i < behaviours.Length; i++)
            {

                for (int a = 0; a < allBehaviours.Length; a++)
                {
                    if (allowedabilities[i] == allBehaviours[a].ability)
                    {
                        behaviours[i] = allBehaviours[a];
                    }
                }
            }

            for (int i = 0; i < behaviours.Length; i++)
            {
                var score = behaviours[i].Evaluate(data);
                if (score > topScore)
                {
                    topScore = score;
                    topBehaviourIndex = i;
                }
            }

            var topBehaviour = behaviours[topBehaviourIndex];
            ecb1.AddComponent(entity, topBehaviour.GetAbilityComponent(data));

        }).WithoutBurst().Run();
        UpdateECB();
    }
}

public struct ReadyTag : IComponentData
{

}
public struct CooldownComponent : IComponentData
{
    public float cooldown;
}

