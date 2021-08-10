using System;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlotData
    {
        public int Id { get; set; } 
        public int InventoryIndex { get; set; }
        public int PanelIndex { get; set; }
        public string ResourcePath { get; set; }
    }
}