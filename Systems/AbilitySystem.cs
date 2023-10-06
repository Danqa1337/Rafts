using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public enum Ability
{
    Wait,
    Throw,
    Move,
    Push,
    Trust,
    Heal,
    WaitForSyncPush,
    Explode,
    WaveHands,
    Jump,
    Sit,
    ShootWithMortar,
    Evade,
}

[UpdateAfter(typeof(OrderExecuteSystem))]

public partial class AbilitySystem : MySystemBase
{
    private EntityCommandBuffer ecb;
    protected override void OnUpdate()
    {
        ecb = CreateEntityCommandBuffer();
        Entities.ForEach((Entity entity, ref AbilityComponent abilityComponent) =>
        {
            this.GetType().GetMethod(abilityComponent.ability.ToString()).Invoke(this, parameters: new object[] { entity });
            ecb.RemoveComponent<AbilityComponent>(entity);

        }).WithoutBurst().Run();
        UpdateECB();
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<RaftSystem>().Update();

    }
    public void Wait(Entity entity)
    {
        SetCooldown(entity, 1f);

    }
    public void HealAll(Entity entity)
    {
        SetCooldown(entity, 1f);
    }
    public async void Throw(Entity entity)
    {
        var abilityComponent = entity.GetComponentData<AbilityComponent>();
        var animator = entity.GetComponentObject<Animator>();
        var transform = entity.GetComponentObject<Transform>();
        animator.Play("Throw");
        SetCooldown(entity, 2f, 3f);

        var transformPatent = entity.GetComponentObject<Transform>().transform.root;

        await System.Threading.Tasks.Task.Delay(200);
        if (transform != null)
        {
            var spear = Pooler.Take("Spear", transform.position);
            var startPosition = transform.position;
            var targetPosition = abilityComponent.vector.ToVector3() + UnityEngine.Random.insideUnitCircle.ToVector3();
            if (PlayersRaft.instance != null && EffectSystem.HasEffect(PlayersRaft.instance.entity, EffectName.SmokeScreen))
            {
                targetPosition += UnityEngine.Random.insideUnitSphere * 10;
            }
            spear.GetComponent<Projectile>().Launch(entity, startPosition, targetPosition, transformPatent);
        }
    }
    public void Move(Entity entity)
    {
        entity.GetComponentObject<Animator>().SetTrigger("Walk");
        var abilityComponent = entity.GetComponentData<AbilityComponent>();

        ecb.AddComponent(entity, new MoveComponent(abilityComponent.vector));
        SetCooldown(entity, 2f, 3f);

    }
    public async void Push(Entity entity)
    {

        var abilityComponent = entity.GetComponentData<AbilityComponent>();
        var transform = entity.GetComponentObject<Transform>();
        var displacement = UnityEngine.Random.insideUnitCircle.ToFloat2() * 0.5f;
        var animator = entity.GetComponentObject<Animator>();

        SetCooldown(entity, 1f, 2f);
        animator.Play("Jump");

        if (entity.GetComponentData<OnPlatformEntityComponent>().platform.GetComponentData<BoundsComponent>().bounds.Contains(transform.localPosition.ToFloat2() + displacement))
        {
            ecb.AddComponent(entity, new MoveComponent(transform.localPosition.ToFloat2() + displacement));

        }
        await Task.Delay(200);

        if (entity.Exists())
        {
            var raft = entity.GetComponentData<OnPlatformEntityComponent>().platform;
            if (raft.HasBuffer<PushElement>())
            {
                raft.AddBufferElement(new PushElement(abilityComponent.vector.Normalize()));
                raft.AddComponentData(new HasPushElementTag());

            }
        }
    }

    public void WaitForSyncPush(Entity entity)
    {

        entity.GetComponentObject<Animator>().Play("Sit");
        ecb.AddComponent(entity.GetComponentData<OnPlatformEntityComponent>().platform, new SyncAbilityComponent(new AbilityComponent(Ability.Push, entity.GetComponentData<AbilityComponent>().vector)));
        ecb.AddComponent(entity, new WaitingForSyncTag());

    }
    public async void Trust(Entity entity)
    {
        var transform = entity.GetComponentObject<Transform>();
        var abilityComponent = entity.GetComponentData<AbilityComponent>();
        var weaponTransform = transform.GetChild(1);
        var animator = entity.GetComponentObject<Animator>();
        animator.Play("Trust");
        SetCooldown(entity, 4f, 6f);
        await System.Threading.Tasks.Task.Delay(100);
        if (entity.Exists())
        {
            var raft = entity.GetComponentData<OnPlatformEntityComponent>().platform;

            animator.enabled = false;
            weaponTransform.GetChild(0).gameObject.layer = 10;
            var direction = (abilityComponent.vector.ToVector3() - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x * transform.localScale.x) * Mathf.Rad2Deg - 90 + UnityEngine.Random.Range(-10, 10);

            weaponTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            var displacement = (direction).ToFloat2();

            if (raft.HasBuffer<PushElement>())
            {
                raft.AddBufferElement(new PushElement(direction.ToFloat2()));
                raft.AddComponentData(new HasPushElementTag());
            }
            if (entity.GetComponentData<OnPlatformEntityComponent>().platform.GetComponentData<BoundsComponent>().bounds.Contains(transform.localPosition.ToFloat2() + displacement))
            {
                entity.AddComponentData(new MoveComponent(transform.localPosition.ToFloat2() + displacement, 3));

            }

            await System.Threading.Tasks.Task.Delay(1000);
            if (weaponTransform != null) weaponTransform.GetChild(0).gameObject.layer = 9;
            await System.Threading.Tasks.Task.Delay(2000);
            if (animator != null) animator.enabled = true;
        }

    }
    public async void ShootWithMortar(Entity entity)
    {
        var mortarEntity = entity.GetComponentData<MaintainingMechanismComponent>().mechanism;
        if (mortarEntity.HasComponent<ReadyTag>())
        {
            var abilityComponent = entity.GetComponentData<AbilityComponent>();
            SetCooldown(entity, 1, 2);
            SetCooldown(mortarEntity, 10);
            await Task.Delay(10);
            mortarEntity.GetComponentObject<Mortar>().Shoot(abilityComponent.vector, entity);

        }
    }
    private void SetCooldown(Entity entity, float cooldownSecondsMin, float cooldownSecondsMax = 0, bool sync = false)
    {
        if (entity.Exists())
        {
            var cooldownSeconds = cooldownSecondsMin;
            if (cooldownSecondsMax != 0)
            {
                cooldownSeconds = UnityEngine.Random.Range(cooldownSecondsMin, cooldownSecondsMax);
            }
            if (sync || !entity.HasComponent<PlayerTag>())
            {
                //not implemented
            }
            if (entity.HasComponent<PlayerTag>())
            {
                cooldownSeconds /= GameManager.instance != null ? GameManager.instance.playerStatsBonus + XPManager.statsBonus : 1;
            }

            var ai = entity.GetComponentData<CooldownComponent>();

            ai.cooldown = cooldownSeconds;
            ecb.SetComponent(entity, ai);
            if (entity.HasComponent<ReadyTag>())
            {
                ecb.RemoveComponent<ReadyTag>(entity);
            }
        }
    }
    public void SuicideJump(Entity entity)
    {

    }
    public async void Explode(Entity entity)
    {
        Debug.Log("explode");
        var transform = entity.GetComponentObject<Transform>();
        entity.GetComponentObject<Animator>().Play("Sit");
        transform.GetComponentInChildren<ParticleSystem>().emissionRate = 200;
        transform.GetComponentInChildren<ParticleSystem>().startSize = 2;
        SetCooldown(entity, 99);
        await Task.Delay(500);
        if (entity.Exists())
        {

            transform.GetComponent<ExecuteInRadiysExecutable>().Execute();

        }
    }
    public void WaveHands(Entity entity)
    {
        SetCooldown(entity, 2, 4);
        entity.GetComponentObject<Animator>().Play("WaveHands");
    }
    public void Jump(Entity entity)
    {
        SetCooldown(entity, 2, 4);
        entity.GetComponentObject<Animator>().Play("Jump");
    }
    public void Sit(Entity entity)
    {
        SetCooldown(entity, 2, 4);
        if (entity.HasComponent<MoveComponent>())
        {
            ecb.RemoveComponent<MoveComponent>(entity);
        }
        entity.GetComponentObject<Animator>().Play("Sit");
    }
    public void Evade(Entity entity)
    {
        Push(entity);
    }

}
public struct AbilityComponent : IComponentData
{
    public Ability ability;
    public float2 vector;

    public AbilityComponent(Ability ability, float2 vector)
    {
        this.ability = ability;
        this.vector = vector;
    }
}
public struct SyncAbilityComponent : IComponentData
{
    public AbilityComponent abilityComponent;

    public SyncAbilityComponent(AbilityComponent abilityComponent)
    {
        this.abilityComponent = abilityComponent;
    }
}