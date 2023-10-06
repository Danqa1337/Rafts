using Unity.Entities;

public enum EffectName
{
    SmokeScreen,
}
public partial class EffectSystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var effectBufferFromEntity = GetBufferFromEntity<EffectElement>();

        Entities.WithAll<EffectElement>().ForEach((Entity entity) =>
        {
            var effectElements = effectBufferFromEntity[entity];
            for (int i = 0; i < effectElements.Length; i++)
            {
                var effectElement = effectElements[i];
                effectElement.liftime -= Time.DeltaTime;
                if (effectElement.liftime <= 0)
                {
                    effectElements.RemoveAt(i);

                }
                else
                {
                    effectElements[i] = effectElement;
                }

            }
        }).WithoutBurst().Run();
    }

    public static bool HasEffect(Entity entity, EffectName effectName)
    {
        if (entity == Entity.Null) throw new System.NullReferenceException("Entity is null");
        if (!entity.HasBuffer<EffectElement>()) throw new System.NullReferenceException("No effect buffer");

        var buffer = entity.GetBuffer<EffectElement>();
        foreach (var item in buffer)
        {
            if (item.effectName == effectName) return true;
        }
        return false;
    }

    public static void AddEffect(Entity entity, EffectName effectName, float liftime)
    {
        if (entity == Entity.Null) throw new System.NullReferenceException("Entity is null");
        if (!entity.HasBuffer<EffectElement>()) throw new System.NullReferenceException("No effect buffer");

        entity.AddBufferElement(new EffectElement(effectName, liftime));

    }
}

public struct EffectElement : IBufferElementData
{
    public EffectName effectName;
    public float liftime;

    public EffectElement(EffectName effectName, float liftime)
    {
        this.effectName = effectName;
        this.liftime = liftime;
    }
}
