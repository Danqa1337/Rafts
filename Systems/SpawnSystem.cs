using Unity.Entities;
public partial class SpawnSystem : MySystemBase
{

    public void Spawn()
    {
        EntityManager.Instantiate(GetSingleton<PrefabComponent>().Value);
    }

    protected override void OnUpdate()
    {

    }
}
