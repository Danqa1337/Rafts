using UnityEngine;

public class AddItemToInventoryExecutable : ExecutableWithTarget
{
    public ItemName itemName;

    public override void Execute(GameObject target)
    {
        if (target.GetComponent<PlayersRaft>())
        {
            if (PlayersInventory.PlaceToEmptySlot(itemName))
            {
                base.Execute(target);
            }
        }

    }

}
