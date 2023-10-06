using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Selectable
{
    public int number;
    public Image icon;
    public Label numberCounter;
    public ItemName itemName;

    protected override void Awake()
    {
        ReDraw();

    }
    public void ReDraw()
    {

        numberCounter.SetFloatValue = number;
        if (number > 0)
        {
            numberCounter.gameObject.SetActive(true);
            numberCounter.SetIntValue = number;
            if (itemName != ItemName.Null)
            {
                icon.sprite = ItemDatabase.GetItem(itemName).sprite;
                icon.color = Color.white;
            }
        }
        else
        {
            Clear();

        }
    }
    public void PlaceItem(ItemName newitemName, int newNumber = 1)
    {
        number += newNumber;
        itemName = newitemName;
        ReDraw();

    }
    public void Clear()
    {
        number = 0;
        itemName = ItemName.Null;
        icon.color = Color.clear;
        icon.sprite = null;
        numberCounter.gameObject.SetActive(false);

    }

    private void ApplyItem()
    {
        if (itemName != ItemName.Null)
        {
            ItemApplyer.ApplyItem(itemName);
            number--;
            ReDraw();

        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ApplyItem();
        base.OnPointerDown(eventData);

    }

}
