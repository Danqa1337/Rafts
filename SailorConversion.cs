using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class SailorConversion : MonoBehaviour
{
    bool converted = false;
    public int xp;
    public async void OnEnable()
    {

        if (!converted)
        {
            converted = true;
            await System.Threading.Tasks.Task.Delay(500);
            if (Utills.Chance(50)) transform.localScale = new Vector3(-1, 1, 1);

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var sailorEntity = entityManager.CreateEntity();
            GetComponent<EntityLink>().entity = sailorEntity;

            sailorEntity.SetName(name);

            sailorEntity.AddComponentObject(transform);

            sailorEntity.AddComponentObject(GetComponent<SpriteRenderer>());
            sailorEntity.AddComponentObject(GetComponent<Animator>());
            sailorEntity.AddComponentObject(GetComponent<BoxCollider2D>());

            sailorEntity.AddComponentData(new LocalToWorld());
            sailorEntity.AddComponentData(new LocalToParent());
            sailorEntity.AddComponentData(new CopyTransformFromGameObject());
            sailorEntity.AddComponentData(new DistanceComponent());
            sailorEntity.AddComponentData(new LastLocalToWorld());
            sailorEntity.AddComponentData(new XpComponent(xp));
            sailorEntity.AddComponentData(new CooldownComponent());

            sailorEntity.SetZeroSizedTagComponentData(new UpdateSortingOrderTag());
            sailorEntity.AddBuffer<HealthChangedElement>();
            sailorEntity.AddBuffer<EffectElement>();

            var properties = GetComponent<SailorProperties>();
            sailorEntity.AddComponentData(properties.sailorAIComponent);
            sailorEntity.AddComponentData(properties.healthComponent);

            sailorEntity.AddBuffer<AllowedAbilityElement>();
            sailorEntity.AddBuffer<Child>();

            foreach (var item in properties.abilities)
            {
                sailorEntity.AddBufferElement(new AllowedAbilityElement(item));
            }

            var headTransform = transform.GetChild(0);
            var weaponTransform = transform.GetChild(1);

            await System.Threading.Tasks.Task.Delay(500);

            var headEntity = entityManager.CreateEntity();
            var weaponEntity = entityManager.CreateEntity();

            headEntity.SetName("Head");
            weaponEntity.SetName("Weapon");

            headEntity.AddComponentData(new LocalToWorld());
            headEntity.AddComponentData(new LocalToParent());
            headEntity.AddComponentData(new Parent() { Value = sailorEntity });
            headEntity.AddComponentObject(headTransform.transform.GetComponent<SpriteRenderer>());
            headEntity.AddComponentData(new SortingLayerOffsetComponent(1));

            weaponEntity.AddComponentData(new LocalToWorld());
            weaponEntity.AddComponentData(new LocalToParent());
            weaponEntity.AddComponentData(new Parent() { Value = sailorEntity });
            weaponEntity.AddComponentObject(weaponTransform.transform.GetComponent<SpriteRenderer>());

            converted = true;

            var platform = GetComponentInParent<PlatformConversion>();

            if (platform != null)
            {

                var platformEntity = platform.GetComponent<EntityLink>().entity;

                if (platformEntity == Entity.Null)
                {
                    platformEntity = platform.GetComponent<PlatformConversion>().Convert();
                }

                platformEntity.AddBufferElement(new Child() { Value = sailorEntity });
                sailorEntity.AddComponentData(new OnPlatformEntityComponent(platformEntity));
                if (platform.mechanismEntity != Entity.Null)
                {
                    sailorEntity.AddComponentData(new MaintainingMechanismComponent(platform.mechanismEntity));
                }
                var raft = GetComponentInParent<Raft>();

                if (raft != null)
                {
                    if (!raft.sailors.Contains(sailorEntity))
                    {
                        raft.sailors.Add(sailorEntity);
                        raft.sailorsAlive++;
                    }

                    sailorEntity.AddComponentObject(raft);

                    sailorEntity.AddComponentData(new Parent { Value = platformEntity });
                    sailorEntity.AddComponentData(new LocalToParent());
                    platformEntity.AddBufferElement(new CrewElement(sailorEntity));
                    if (raft.GetComponent<PlayersRaft>() != null)
                    {
                        sailorEntity.AddComponentData(new PlayerTag());
                        ViewModel.instance.SailorsCount++;
                    }

                }
            }

        }
    }
}
public struct SailorSharedComponentData : ISharedComponentData
{

}
public struct SailorChunkComponentData : IComponentData
{
    public int id;
}
public struct OnPlatformEntityComponent : IComponentData
{
    public Entity platform;

    public OnPlatformEntityComponent(Entity raft)
    {
        this.platform = raft;
    }
}
public struct MaintainingMechanismComponent : IComponentData
{
    public Entity mechanism;

    public MaintainingMechanismComponent(Entity mechanism)
    {
        this.mechanism = mechanism;
    }
}

