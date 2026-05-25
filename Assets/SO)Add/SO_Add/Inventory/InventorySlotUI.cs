using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image iconImage;
    public TMP_Text countText;

    private InventoryUI inventoryUI;
    private List<InventoryItem> itemList;
    private int index;

    public void SetSlot(InventoryUI inventoryUI, List<InventoryItem> itemList, int index)
    {
        this.inventoryUI = inventoryUI;
        this.itemList = itemList;
        this.index = index;
        // 현재 슬롯의 아이콘과 개수 텍스트 갱신
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 아이템이 없으면 return, 아이콘 raycast 끄기
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 이동량만큼 아이콘 이동
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 아이콘을 원위치하고 raycast 다시 켜기
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 드래그 시작 슬롯을 찾아 PlayerInventory.MoveItem() 호출
    }
}
