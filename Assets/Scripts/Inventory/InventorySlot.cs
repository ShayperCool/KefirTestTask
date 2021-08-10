using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        public event Action<InventorySlot> OnClick;

        private const string IconsPath = "icons";
        private static Sprite[] _iconsSprites;
        
        [SerializeField] private GameObject selectedIndicator;
        [SerializeField] private Image icon;

        public InventorySlotData InventorySlotData
        {
            get => _inventorySlotData;
            set
            {
                var iconSprite = _iconsSprites.FirstOrDefault(sprite =>
                    $"{IconsPath}/{sprite.name}" == value.ResourcePath);
                if (iconSprite != null)
                {
                    icon.gameObject.SetActive(true);
                    icon.sprite = iconSprite;
                    icon.color = Color.white;
                }

                _inventorySlotData = value;
            }
        }

        private InventorySlotData _inventorySlotData;

        public bool Selected
        {
            get => _selected;
            set
            {
                selectedIndicator.SetActive(value);
                _selected = value;
            }
        }

        private bool _selected;

        private void Awake()
        {
            if (_iconsSprites == null)
            {
                _iconsSprites = Resources.LoadAll<Sprite>(IconsPath);
            }
        }
        


        public void OnPointerClick(PointerEventData eventData)
        {
            if (InventorySlotData != null)
            {
                OnClick?.Invoke(this);
            }
        }

        public void Reset()
        {
            icon.gameObject.SetActive(false);
            icon.sprite = null;
            selectedIndicator.SetActive(false);
        }
    }
}