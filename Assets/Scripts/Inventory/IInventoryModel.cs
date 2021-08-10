using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory
{
    public interface IInventoryModel
    {
        event Action<InventorySlotData> UpdateInventorySlot;
        //для обновления всех слотов, пришли новые данные с бека и тд тп
        event Action<List<InventorySlotData>> UpdateInventory;
        void LoadInventoryElements();
        void MoveToAnotherPanel(InventorySlotData inventorySlotData);
    }
}