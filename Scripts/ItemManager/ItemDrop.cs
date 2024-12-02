using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ClickInputSystem clickInputSystem;  // Ŭ�� �Է� �ý��� ����
    public ItemProbability itemProbability;   // ������ Ȯ�� ���� ��ũ��Ʈ ����
    public ItemAni itemAni;                   // ������ �ִϸ��̼� ��ũ��Ʈ ����
    public PlayerMovement playerMovement;     // �÷��̾� �̵� ��ũ��Ʈ ����
    public InventoryManager inventoryManager; // �κ��丮 ���� ��ũ��Ʈ ����

    private void OnEnable()
    {
        clickInputSystem.OnItemClicked += HandleItemClicked;  // �̺�Ʈ ����
    }

    private void OnDisable()
    {
        clickInputSystem.OnItemClicked -= HandleItemClicked;  // �̺�Ʈ ���� ����
    }

    private void HandleItemClicked(GameObject item)
    {
        RoomInfo itemRoomInfo = item.GetComponentInParent<RoomInfo>();  // �������� ���� ���� ����
        RoomInfo playerRoomInfo = playerMovement.GetCurrentRoomInfo();
        if (itemRoomInfo == playerRoomInfo)
        {
            item.SetActive(false);  // Ŭ���� �������� ��Ȱ��ȭ
            GameObject newItem = itemProbability.SpawnItem(item.transform.position);  // �� ������ ����
            ItemPreInfo newItemInfo = newItem.GetComponent<ItemPreInfo>(); // �� �������� ItemPreInfo ������Ʈ

            if (newItemInfo != null && newItemInfo.item != null)
            {
                Debug.Log($"Adding new item: {newItemInfo.item.itemName} with ID {newItemInfo.item.itemID}");
                InventoryManager.Instance.AddItem(newItemInfo.item); // �κ��丮�� �� ������ �߰�
            }

            itemAni.MoveItem(newItem);  // �� �������� �̵���Ű�� �޼��� ȣ��
            Debug.Log(newItem.name);  // �� �������� �̸� �α� ���
        }
    }
}
