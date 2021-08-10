using System.Collections.Generic;

namespace Inventory
{
    public interface IInventoryPresenter
    {
        void OnMoveClick(InventorySlotData inventorySlotData);
        void OnInventoryUpdated(List<InventorySlotData> inventorySlotDatas);
        void OnInventorySlotUpdated(InventorySlotData inventorySlotData);
    }
}