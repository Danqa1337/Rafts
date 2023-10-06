using Unity.Entities;
[GenerateAuthoringComponent]
[System.Serializable]
public struct HealthComponent : IComponentData
{
    public int MaxHealth;
    public int CurrentHealth;

    public HealthComponent(int maxHealth, int currentHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
    }
}