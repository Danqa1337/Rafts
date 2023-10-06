using System.Collections.Generic;
using UnityEngine;
public enum ItemName
{
    Null,
    Medkit,
    Smoke,
    Epicenter,
}
[CreateAssetMenu(fileName = "New ItemDatabase", menuName = "ItemDatabase")]
public class ItemDatabase : SingletonScriptableObject<ItemDatabase>
{
    public ItemTable itemTable;
    [SerializeField] public List<ItemData> items = new List<ItemData>();

    public static Dictionary<ItemName, ItemData> itemsByName;

    public static ItemData GetItem(ItemName itemName)
    {
        if (itemsByName != null)
        {
            return itemsByName[itemName];
        }
        else
        {
            throw new System.NullReferenceException("dictionary is not created");
        }
    }
    public static void StartUp()
    {
        itemsByName = new Dictionary<ItemName, ItemData>();
        foreach (var item in instance.items)
        {
            itemsByName.Add(item.itemName, item);
        }
    }
    [ContextMenu("Reimport")]
    public void Reimport()
    {
        instance.items = new List<ItemData>();
        foreach (var param in instance.itemTable.sheets[0].list)
        {
            var newItem = new ItemData();
            newItem.itemName = param.ItemName.DecodeCharSeparatedEnumsAndGetFirst<ItemName>();
            newItem.spawnChance = param.SpawnChance;
            newItem.sprite = Resources.Load<Sprite>("ItemIcons/" + param.ItemName + "Icon");
            instance.items.Add(newItem);
        }
    }

}

[System.Serializable]
public class ItemData
{
    public ItemName itemName;
    public Sprite sprite;
    public float spawnChance;
}