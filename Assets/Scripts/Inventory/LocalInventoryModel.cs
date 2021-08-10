using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Inventory
{
    public class LocalInventoryModel : IInventoryModel
    {
        private static string SaveLoadPath;
        private const int EMPTY_ID = -1;

        private int[] _userPanelSlotsIds = Enumerable.Repeat(EMPTY_ID, 10).ToArray();
        private int[] _targetPanelSlotsIds = Enumerable.Repeat(EMPTY_ID, 10).ToArray();

        private List<InventorySlotData> _inventorySlotDatas;

        public event Action<InventorySlotData> UpdateInventorySlot;
        public event Action<List<InventorySlotData>> UpdateInventory;

        public LocalInventoryModel()
        {
            SaveLoadPath = Path.Combine(Application.persistentDataPath, "inventory_data.json");
            Application.quitting += Save;
        }

        public void LoadInventoryElements()
        {
            if (File.Exists(SaveLoadPath))
            {
                Task.Run(() => File.ReadAllText(SaveLoadPath))
                    .ContinueWith(task => JsonConvert.DeserializeObject<List<InventorySlotData>>(task.Result),
                        TaskScheduler.FromCurrentSynchronizationContext())
                    .ContinueWith(task =>
                    {
                        _inventorySlotDatas = task.Result;
                        FillArrays(_inventorySlotDatas);
                        UpdateInventory?.Invoke(_inventorySlotDatas);
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                _inventorySlotDatas = Enumerable.Range(0, 10).Select(index =>
                    new InventorySlotData
                    {
                        Id = index,
                        PanelIndex = 0,
                        InventoryIndex = index,
                        ResourcePath = $"icons/Sprites_{index}",
                    }
                ).ToList();
                FillArrays(_inventorySlotDatas);
                UpdateInventory?.Invoke(_inventorySlotDatas);
            }
        }

        private void FillArrays(List<InventorySlotData> inventorySlotDatas)
        {
            foreach (var inventorySlotData in inventorySlotDatas)
            {
                var targetArray = inventorySlotData.PanelIndex == 0 ? _userPanelSlotsIds : _targetPanelSlotsIds;
                targetArray[inventorySlotData.InventoryIndex] = inventorySlotData.Id;
            }
        }

        public void MoveToAnotherPanel(InventorySlotData inventorySlotData)
        {
            var currentPanel = inventorySlotData.PanelIndex == 0 ? _userPanelSlotsIds : _targetPanelSlotsIds;
            currentPanel[inventorySlotData.InventoryIndex] = EMPTY_ID;

            var panelToMove = inventorySlotData.PanelIndex == 0 ? _targetPanelSlotsIds : _userPanelSlotsIds;

            var firstFreeIndex = Array.FindIndex(panelToMove, 0, slotId => slotId == EMPTY_ID);

            panelToMove[firstFreeIndex] = inventorySlotData.Id;

            inventorySlotData.InventoryIndex = firstFreeIndex;
            inventorySlotData.PanelIndex = inventorySlotData.PanelIndex == 0 ? 1 : 0;
            UpdateInventorySlot?.Invoke(inventorySlotData);
        }

        private void Save()
        {
            var jsonText = JsonConvert.SerializeObject(_inventorySlotDatas);
            File.WriteAllText(SaveLoadPath, jsonText);
        }
    }
}