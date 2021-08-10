using System;
using System.Collections.Generic;

namespace Inventory
{
    public interface IInventoryView
    {
        event Action<InventorySlotData> OnMoveClick;
        void UpdateInventory(List<InventorySlotData> inventoryCellDatas);
        void UpdateInventorySlot(InventorySlotData inventorySlotData);
    }
}