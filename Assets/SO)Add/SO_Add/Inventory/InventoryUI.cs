using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public InventorySlotUI[] bagSlots;
    public InventorySlotUI[] equipSlots;

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public void Toggle()
    {
        // 패널 열기/닫기
        // 열릴 때 Refresh() 호출
    }

    public void Refresh()
    {
        // bagSlots와 equipSlots에 PlayerInventory의 리스트 연결
    }
}
