using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomClicked : MonoBehaviour
{
    public ClickInputSystem clickInputSystem;  // Ŭ�� �Է� �ý��� ����
    public PlayerMovement playerMovement;
    public PlayerAni playerAni;
    public Player player;
    public GameObject bagbtn;
    private GameObject currentRoom = null; // ���� ���õ� ��
    private EnemyHealth currentEnemyHealth; // ���� �濡 �ִ� ���� Health ������Ʈ
    private void OnEnable()
    {
        clickInputSystem.OnRoomClicked += HandleRoomClicked;
    }

    private void OnDisable()
    {
        clickInputSystem.OnRoomClicked -= HandleRoomClicked;
    }
    private void HandleRoomClicked(GameObject room)
    {
        if (playerAni.Moveable)//�������� �����Ҷ�(���� �÷��̾ �ȴ� �ִϸ��̼��� �ϰ� ���� ����)
        {
            
            player.isInCombat = false;
            RoomInfo clickedRoomInfo = room.GetComponent<RoomInfo>();

            // ������ Ŭ���� ���� Collider�� �ٽ� Ȱ��ȭ
            if (currentRoom != null)
            {
                currentRoom.GetComponent<BoxCollider2D>().enabled = true;
            }

            // ù �̵��� 1�� �����θ� �̵� ����
            if (currentRoom == null && clickedRoomInfo.RoomNumber == 1 && clickedRoomInfo.CurrenFloor == 1)
            {
                currentRoom = room; // ���� �� ������Ʈ
                currentEnemyHealth = room.GetComponentInChildren<EnemyHealth>(); // �� ���� ������Ʈ
                RealRoomClick(room, clickedRoomInfo); // ù �� �̵�
                
            }
            else if (currentRoom != null)
            {
                RoomInfo currentRoomInfo = currentRoom.GetComponent<RoomInfo>();
                
                // �� �̵� (���� �� ���� �յ� �游 �̵� ����)
                if (clickedRoomInfo != null && currentRoomInfo != null &&
                    (clickedRoomInfo.RoomNumber == currentRoomInfo.RoomNumber + 1 ||
                     clickedRoomInfo.RoomNumber == currentRoomInfo.RoomNumber - 1))
                {
                    //BattleManager.Instance.SearchEnemy(clickedRoomInfo);
                    if (currentEnemyHealth != null && currentEnemyHealth.alive)
                    {
                        BattleUIManager.Instance.HitTextPosition(playerMovement.isPoint1);
                        StartCoroutine(EnemyCounterAndMove(room, clickedRoomInfo));
                    }
                    else
                    {
                        RealRoomClick(room, clickedRoomInfo); // ���� ���ų� �׾������� �ٷ� �̵�
                    }
                }
            }
            
        }
    }

    private IEnumerator EnemyCounterAndMove(GameObject room, RoomInfo roomInfo)
    {
        // ���� �ݰ� ����
        if (currentEnemyHealth != null && currentEnemyHealth.alive)
        {
            BattleManager.Instance.PerformRoomEnemyCounterAttack(currentEnemyHealth);
            playerAni.Moveable = false;
            yield return new WaitForSeconds(1.0f); // �ݰ� ���
            playerAni.isfoodSound = false;
            
        }

        // �ݰ� �� �� �̵�
        RealRoomClick(room, roomInfo);
    }
    private void RealRoomClick(GameObject room, RoomInfo clickedRoomInfo)
    {

        playerMovement.MovePlayerToRoom(room); // �ش� ������ �̵�
        currentRoom = room; // ���� �� ������Ʈ
        bagbtn.SetActive(true);
        currentEnemyHealth = room.GetComponentInChildren<EnemyHealth>();
        room.GetComponent<BoxCollider2D>().enabled = false; // ���ο� ���� Collider ��Ȱ��ȭ

        // �÷��̾� ��ũ��Ʈ�� ���� ��� �� ���� ������Ʈ
        player.CurrentRoom = clickedRoomInfo.RoomNumber;
        player.CurrentFloor = clickedRoomInfo.CurrenFloor;
        // õ���� ���� �� ���¸� ������Ʈ
        player.IsAngelRoom = TowerGenerator.AngelRoomNumbers.Contains(clickedRoomInfo.RoomNumber);
        player.IsEnemyRoom = currentEnemyHealth != null && currentEnemyHealth.alive;

        Transform hideTransform = room.transform.Find("Hide"); // 'Hide' ������Ʈ ã��
        Transform itemsTransform = room.transform.Find("Items"); // 'Items' ������Ʈ ã��
        if (hideTransform != null) // ���� hide�� ������
        {
            SpriteRenderer spriteRenderer = hideTransform.GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
            if (spriteRenderer != null) // ��������Ʈ�� ������
            {
                Color newColor = spriteRenderer.color;
                newColor.a = 0;  // ������ 0���� ����
                spriteRenderer.color = newColor;  // ���� ���� ����
                if(itemsTransform != null)//���� �������� ������
                {
                    itemsTransform.gameObject.SetActive(true); // ������ Ȱ��ȭ
                }
            }
        }
        
    }
}
