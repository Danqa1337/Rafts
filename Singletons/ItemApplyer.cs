using System.Linq;
using UnityEngine;

public class ItemApplyer : Singleton<ItemApplyer>
{

    public static void ApplyItem(ItemName itemName)
    {
        instance.GetType().GetMethod("Use" + itemName.ToString()).Invoke(instance, null);
        Debug.Log("Used " + itemName);
    }

    public void UseMedkit()
    {
        foreach (var item in PlayersRaft.instance.raft.sailors.Where(s => s.HasComponent<DeadTag>()))
        {
            item.SetZeroSizedTagComponentData(new ResurrectTag());
        }
    }
    public void UseSmoke()
    {
        var screen = Pooler.Take("SmokeScreen", PlayersRaft.instance.transform.position);
        screen.transform.SetParent(PlayersRaft.instance.transform);
        EffectSystem.AddEffect(PlayersRaft.instance.entity, EffectName.SmokeScreen, 10);
    }

    public void UseEpicenter()
    {
        var center = PlayersRaft.instance.transform.position;
        var rafts = Physics2D.OverlapCircleAll(center, 10).Where(o => o.GetComponent<Raft>() != null && o.gameObject != PlayersRaft.instance.gameObject);
        Pooler.Take("EpicenterWave", center);
        foreach (var item in rafts)
        {
            var vector = (item.transform.position - center);
            var distance = vector.magnitude;
            var force = (vector.normalized * 1000);

            item.GetComponent<Rigidbody2D>().AddForce(force);

        }
    }

}
