using UnityEngine;

public class DropSpawner : Singleton<DropSpawner>
{
    public PoolableItem dropHolder;
    public static void SpawnDrop(ItemName itemName, Vector3 position)
    {
        var holder = Pooler.Take(instance.dropHolder, position + UnityEngine.Random.insideUnitCircle.ToVector3() * 2);
        holder.GetComponent<AddItemToInventoryExecutable>().itemName = itemName;
        holder.GetComponentInChildren<SpriteRenderer>().sprite = ItemDatabase.GetItem(itemName).sprite;

    }

    public static void SpawnDrop(Vector3 position, float chanceMultipler = 1)
    {
        SpawnDrop(GetRandomItem().itemName, position);
        var item = GetRandomItem();
        if (Utills.Chance(item.spawnChance * chanceMultipler))
        {
            SpawnDrop(item.itemName, position);
        }

    }

    private static ItemData GetRandomItem()
    {
        return ItemDatabase.instance.items.RandomItem();
    }
}
