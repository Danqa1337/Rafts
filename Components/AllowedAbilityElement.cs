using Unity.Entities;
[GenerateAuthoringComponent]
public struct AllowedAbilityElement : IBufferElementData
{
    public Ability ability;

    public AllowedAbilityElement(Ability ability)
    {
        this.ability = ability;
    }
}
