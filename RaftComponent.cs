using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
public struct RaftComponent : IComponentData
{
    public float avgCrewCooldown;
    public float2 velocity;
}

