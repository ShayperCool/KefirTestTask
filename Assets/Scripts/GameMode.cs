using System;
using Inventory;
using UnityEngine;
using UnityEngine.Serialization;

public class GameMode : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    private IInventoryPresenter _inventoryPresenter;
    private IInventoryModel _inventoryModel;
    private void Start()
    {
        _inventoryModel= new LocalInventoryModel();
        _inventoryPresenter = new InventoryPresenter(inventoryView, _inventoryModel);
        inventoryView.Init();
        _inventoryModel.LoadInventoryElements();
    }
}