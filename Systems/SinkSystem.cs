using Unity.Entities;
using UnityEngine;

public partial class SinkSystem : MySystemBase
{
    protected override void OnUpdate()
    {
        var ecb = CreateEntityCommandBuffer();
        Entities.WithAll<SinkTag>().ForEach((Entity entity, Raft raft) =>
        {
            ecb.AddComponent(entity, new DestroyTag());

            foreach (var item in raft.sailors)
            {
                ecb.AddComponent(item, new DestroyTag());
            }

            if (!entity.HasComponent<PlayerTag>())
            {
                DropSpawner.SpawnDrop(raft.transform.position, raft.dropChanceMultipler);

            }
            else
            {
                PlayersRaft.instance.Die();
            }

            var debris = Pooler.Take("SinkingDebris", raft.transform.position);
            debris.GetComponentInChildren<SpriteRenderer>().sprite = raft.GetComponentInChildren<SpriteRenderer>().sprite;
            debris.GetComponent<Animation>().Play();

            raft.GetComponent<DestroySelfExecutable>().Execute();

            Debug.Log("raft destroyed");
        }).WithoutBurst().Run();
        UpdateECB();
    }

}
public struct SinkTag : IComponentData
{

}
