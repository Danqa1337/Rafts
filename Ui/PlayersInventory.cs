using System.Linq;
using UnityEngine;

public class PlayersInventory : Singleton<PlayersInventory>
{
    public InventorySlot[] slots;
    private void Awake()
    {
        slots = GetComponentsInChildren<InventorySlot>();
    }
    public static int GetEmptySlotsCount()
    {
        var count = 0;
        foreach (var item in instance.slots)
        {
            if (item.itemName == ItemName.Null) count++;
        }
        return count;
    }
    public static InventorySlot GetSlotWithItem(ItemName itemName)
    {
        if (instance.slots.Any(s => s.itemName == itemName))
        {
            return instance.slots.First(s => s.itemName == itemName);
        }
        else
        {
            return null;
        }
    }
    public static bool PlaceToEmptySlot(ItemName itemName, int number = 1)
    {
        var slotWithItem = GetSlotWithItem(itemName);

        if (slotWithItem != null)
        {
            slotWithItem.PlaceItem(itemName, number);
            return true;
        }
        else
        {

            if (GetEmptySlotsCount() > 0)
            {
                var emptySlot = GetSlotWithItem(ItemName.Null);
                emptySlot.PlaceItem(itemName, number);
                return true;

            }
            else
            {
                Debug.Log("Inventory is full!");
                return false;
            }
        }
    }
}
