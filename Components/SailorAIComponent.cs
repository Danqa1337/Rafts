using Unity.Entities;
[GenerateAuthoringComponent]
[System.Serializable]
public struct SailorAIComponent : IComponentData
{
    public int AttackDistance;
    public int PushDistance;
    public int IddleDistance;

}

