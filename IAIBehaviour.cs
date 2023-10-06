using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
public interface IAIBehaviour
{
    public Ability ability { get; }
    public float Evaluate(AIBehaviourData data);

    public AbilityComponent GetAbilityComponent(AIBehaviourData data);
}

public struct AIBehaviourData
{
    public bool IsOnRaft;
    public bool IsPlayersSailor;
    public float2 targetPosition;
    public float2 worldPosition;
    public float2 localPosition;
    public float2 platformPosition;
    public float distanceToPlayer;
    public Bounds bounds;
    public float mainCaliberCooldown;
    public NavMeshAgent navMeshAgent;

    public SailorAIComponent aIComponent;
}

public struct WaitBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Wait;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        return new AbilityComponent(global::Ability.Wait, float2.zero);

    }

    public float Evaluate(AIBehaviourData data)
    {

        return 0f;
    }

}
public struct RoamBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Move;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        var displacement = UnityEngine.Random.insideUnitCircle.ToFloat2();

        if (data.bounds.Contains(data.localPosition + displacement))
        {
            return new AbilityComponent(global::Ability.Move, data.localPosition + displacement);
        }
        else
        {

        }

        return new WaitBehaviour().GetAbilityComponent(data);
    }

    public float Evaluate(AIBehaviourData data)
    {

        return 0.1f;
    }

}
public struct PushBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Push;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {

        data.navMeshAgent.SetDestination(data.targetPosition.ToVector3());


        if (data.navMeshAgent.path.corners.Length > 1)
        {

            var nextPathNode = data.navMeshAgent.path.corners[1].ToFloat2();
            return new AbilityComponent(global::Ability.Push, nextPathNode - data.worldPosition);

        }

        return new AbilityComponent(global::Ability.Wait, float2.zero);
    }

    public float Evaluate(AIBehaviourData data)
    {
        if (!data.IsPlayersSailor && data.IsOnRaft && data.distanceToPlayer < data.aIComponent.PushDistance)
        {
            return 0.5f;

        }

        return 0;
    }

}
public struct ThrowBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Throw;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        var playersCrew = new List<Entity>();

        if (PlayersRaft.instance != null && PlayersRaft.instance.entity.Exists())
        {
            foreach (var item in PlayersRaft.instance.entity.GetBuffer<CrewElement>().Reinterpret<Entity>())
            {
                if (!item.HasComponent<DeadTag>())
                {
                    playersCrew.Add(item);
                }
            }
        }
        if (playersCrew.Count > 0)
        {
            return new AbilityComponent(global::Ability.Throw, playersCrew.RandomItem().GetComponentObject<Transform>().position.ToFloat2());
        }
        return new WaitBehaviour().GetAbilityComponent(data);
    }

    public float Evaluate(AIBehaviourData data)
    {

        if (!data.IsPlayersSailor && data.distanceToPlayer <= data.aIComponent.AttackDistance)
        {
            return 1;
        }

        return 0;
    }

}
public struct TrustBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Trust;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {

        return new AbilityComponent(global::Ability.Trust, (data.targetPosition));
    }

    public float Evaluate(AIBehaviourData data)
    {
        if (!data.IsPlayersSailor && data.distanceToPlayer <= data.aIComponent.AttackDistance)
        {
            return 1;

        }
        return 0;
    }

}
public struct HealBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Heal;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {

        return new AbilityComponent(global::Ability.Heal, (data.targetPosition));
    }

    public float Evaluate(AIBehaviourData data)
    {
        if (!data.IsPlayersSailor && data.distanceToPlayer <= data.aIComponent.AttackDistance)
        {
            return 1;

        }
        Debug.Log(data.distanceToPlayer);
        return 0;
    }

}
public struct ExplodeBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Explode;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        return new AbilityComponent(global::Ability.Explode, (data.targetPosition));
    }

    public float Evaluate(AIBehaviourData data)
    {

        if (!data.IsPlayersSailor && data.distanceToPlayer <= data.aIComponent.AttackDistance)
        {
            return 99;

        }
        return 0;
    }

}
public struct WaveHandsBehaviour : IAIBehaviour
{
    public Ability ability => Ability.WaveHands;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        return new AbilityComponent(global::Ability.WaveHands, (data.targetPosition));
    }

    public float Evaluate(AIBehaviourData data)
    {

        if (!data.IsPlayersSailor && data.distanceToPlayer <= data.aIComponent.AttackDistance)
        {
            return UnityEngine.Random.Range(1, 2);

        }
        return 0;
    }

}
public struct JumpBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Jump;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        return new AbilityComponent(global::Ability.Jump, (data.targetPosition));
    }

    public float Evaluate(AIBehaviourData data)
    {

        if (!data.IsPlayersSailor && data.distanceToPlayer <= data.aIComponent.AttackDistance)
        {
            return UnityEngine.Random.Range(1, 2);

        }
        return 0;
    }

}
public struct SitBehaviour : IAIBehaviour
{
    public Ability ability => Ability.Sit;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        return new AbilityComponent(global::Ability.Sit, (data.targetPosition));
    }

    public float Evaluate(AIBehaviourData data)
    {

        if (!data.IsPlayersSailor)
        {
            return UnityEngine.Random.Range(0, 2);

        }
        return 0;
    }

}
public struct ShootWithMortarBehaviour : IAIBehaviour
{
    public Ability ability => Ability.ShootWithMortar;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        var playersCrew = new List<Entity>();

        if (PlayersRaft.instance != null && PlayersRaft.instance.entity.Exists())
        {
            foreach (var item in PlayersRaft.instance.entity.GetBuffer<CrewElement>().Reinterpret<Entity>())
            {
                if (!item.HasComponent<DeadTag>())
                {
                    playersCrew.Add(item);
                }
            }
        }
        if (playersCrew.Count > 0)
        {
            return new AbilityComponent(global::Ability.ShootWithMortar, playersCrew.RandomItem().GetComponentObject<Transform>().position.ToFloat2());
        }
        return new WaitBehaviour().GetAbilityComponent(data);
    }

    public float Evaluate(AIBehaviourData data)
    {

        if (!data.IsPlayersSailor && data.distanceToPlayer <= data.aIComponent.AttackDistance && data.mainCaliberCooldown < 0)
        {
            return 1;
        }

        return 0;
    }

}
public struct EvadeBehaviour : IAIBehaviour
{

    public Ability ability => Ability.Evade;

    public AbilityComponent GetAbilityComponent(AIBehaviourData data)
    {
        data.navMeshAgent.SetDestination(data.targetPosition.ToVector3());
        return new AbilityComponent(Ability.Push, UnityEngine.Random.insideUnitCircle.ToFloat2());
    }

    public float Evaluate(AIBehaviourData data)
    {
        if (!data.IsPlayersSailor && data.IsOnRaft && data.distanceToPlayer < data.aIComponent.AttackDistance && Utills.Chance(20))
        {
            return 2f;

        }

        return 0;
    }

}

