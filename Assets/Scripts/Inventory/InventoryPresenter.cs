using System.Collections.Generic;

namespace Inventory
{
    public class InventoryPresenter : IInventoryPresenter
    {

        private IInventoryView _inventoryView;
        private IInventoryModel _inventoryModel;
        
        public InventoryPresenter(IInventoryView inventoryView, IInventoryModel inventoryModel)
        {
            _inventoryModel = inventoryModel;
            
            _inventoryModel.UpdateInventory += OnInventoryUpdated;
            _inventoryModel.UpdateInventorySlot += OnInventorySlotUpdated;

            _inventoryView = inventoryView;

            _inventoryView.OnMoveClick += OnMoveClick;

        }
        
        public void OnMoveClick(InventorySlotData inventorySlotData)
        {
            _inventoryModel.MoveToAnotherPanel(inventorySlotData);
        }

        public void OnInventoryUpdated(List<InventorySlotData> inventorySlotDatas)
        {
            _inventoryView.UpdateInventory(inventorySlotDatas);
        }

        public void OnInventorySlotUpdated(InventorySlotData inventorySlotData)
        {
            _inventoryView.UpdateInventorySlot(inventorySlotData);
        }
    }
}