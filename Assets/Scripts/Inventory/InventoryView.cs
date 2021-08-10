using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        public event Action<InventorySlotData> OnMoveClick;

        [SerializeField] private InventorySlot[] userAvailableInventorySlots;
        [SerializeField] private InventorySlot[] targetAvailableInventorySlots;
        [SerializeField] private Button moveButton;
        
        private InventorySlot _selectedInventorySlot;
        private Dictionary<InventorySlotData, InventorySlot> _inventoryDataToSlotMap;

        public void Init()
        {
            foreach (var userAvailableInventorySlot in userAvailableInventorySlots)
            {
                PrepareInventorySlot(userAvailableInventorySlot);
            }

            foreach (var targetAvailableInventorySlot in targetAvailableInventorySlots)
            {
                PrepareInventorySlot(targetAvailableInventorySlot);
            }
            
            moveButton.onClick.AddListener(MoveButtonClick);
            moveButton.interactable = false;
        }

        private void PrepareInventorySlot(InventorySlot inventorySlot)
        {
            inventorySlot.OnClick += InventorySlotClick;
        }

        private void InventorySlotClick(InventorySlot inventorySlot)
        {
            if (_selectedInventorySlot == inventorySlot)
            {
                _selectedInventorySlot.Selected = false;
                _selectedInventorySlot = null;
                moveButton.interactable = false;
            }
            else
            {
                if (_selectedInventorySlot != null)
                {
                    _selectedInventorySlot.Selected = false;
                }
                _selectedInventorySlot = inventorySlot;
                _selectedInventorySlot.Selected = true;
                moveButton.interactable = true;
            }
        }

        private void MoveButtonClick()
        {
            if (_selectedInventorySlot != null)
            {
                OnMoveClick?.Invoke(_selectedInventorySlot.InventorySlotData);
            }
        }

        
        public void UpdateInventory(List<InventorySlotData> inventorySlotDatas)
        {
            if (_inventoryDataToSlotMap != null)
            {
                foreach (var inventorySlotData in inventorySlotDatas)
                {
                    if (_inventoryDataToSlotMap.TryGetValue(inventorySlotData, out var inventorySlot))
                    {
                        inventorySlot.InventorySlotData = inventorySlotData;
                    }
                    else
                    {
                        AddSlot(inventorySlotData);
                    }
                }
                Reset();
            }
            else
            {
                _inventoryDataToSlotMap = new Dictionary<InventorySlotData, InventorySlot>();
                foreach (var slotData in inventorySlotDatas)
                {
                    AddSlot(slotData);
                }
            }
        }

        private void AddSlot(InventorySlotData slotData)
        {
            var slot = GetSlotByInventorySlotData(slotData);
            slot.InventorySlotData = slotData;
            _inventoryDataToSlotMap.Add(slotData, slot);
        }

        private InventorySlot GetSlotByInventorySlotData(InventorySlotData inventorySlotData)
        {
            var targetArray = inventorySlotData.PanelIndex == 0
                ? userAvailableInventorySlots
                : targetAvailableInventorySlots;
            
            return targetArray[inventorySlotData.InventoryIndex];

        }

        public void UpdateInventorySlot(InventorySlotData inventorySlotData)
        {
            var cell = _inventoryDataToSlotMap[inventorySlotData];
            cell.Reset();
            
            var newCell = GetSlotByInventorySlotData(inventorySlotData);
            newCell.InventorySlotData = inventorySlotData;
            _inventoryDataToSlotMap[inventorySlotData] = newCell;
            
            Reset();
        }

        private void Reset()
        {
            if (_selectedInventorySlot != null)
            {
                _selectedInventorySlot.Selected = false;
            }
            moveButton.interactable = false;
        }
    }
}