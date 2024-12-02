using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomClicked : MonoBehaviour
{
    public ClickInputSystem clickInputSystem;  // 클릭 입력 시스템 참조
    public PlayerMovement playerMovement;
    public PlayerAni playerAni;
    public Player player;
    public GameObject bagbtn;
    private GameObject currentRoom = null; // 현재 선택된 방
    private EnemyHealth currentEnemyHealth; // 현재 방에 있는 적의 Health 컴포넌트
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
        if (playerAni.Moveable)//움직임이 가능할때(현재 플레이어가 걷는 애니메이션을 하고 있지 않음)
        {
            
            player.isInCombat = false;
            RoomInfo clickedRoomInfo = room.GetComponent<RoomInfo>();

            // 이전에 클릭된 방의 Collider를 다시 활성화
            if (currentRoom != null)
            {
                currentRoom.GetComponent<BoxCollider2D>().enabled = true;
            }

            // 첫 이동은 1번 방으로만 이동 가능
            if (currentRoom == null && clickedRoomInfo.RoomNumber == 1 && clickedRoomInfo.CurrenFloor == 1)
            {
                currentRoom = room; // 현재 방 업데이트
                currentEnemyHealth = room.GetComponentInChildren<EnemyHealth>(); // 적 정보 업데이트
                RealRoomClick(room, clickedRoomInfo); // 첫 방 이동
                
            }
            else if (currentRoom != null)
            {
                RoomInfo currentRoomInfo = currentRoom.GetComponent<RoomInfo>();
                
                // 방 이동 (현재 방 기준 앞뒤 방만 이동 가능)
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
                        RealRoomClick(room, clickedRoomInfo); // 적이 없거나 죽어있으면 바로 이동
                    }
                }
            }
            
        }
    }

    private IEnumerator EnemyCounterAndMove(GameObject room, RoomInfo roomInfo)
    {
        // 적의 반격 실행
        if (currentEnemyHealth != null && currentEnemyHealth.alive)
        {
            BattleManager.Instance.PerformRoomEnemyCounterAttack(currentEnemyHealth);
            playerAni.Moveable = false;
            yield return new WaitForSeconds(1.0f); // 반격 대기
            playerAni.isfoodSound = false;
            
        }

        // 반격 후 방 이동
        RealRoomClick(room, roomInfo);
    }
    private void RealRoomClick(GameObject room, RoomInfo clickedRoomInfo)
    {

        playerMovement.MovePlayerToRoom(room); // 해당 방으로 이동
        currentRoom = room; // 현재 방 업데이트
        bagbtn.SetActive(true);
        currentEnemyHealth = room.GetComponentInChildren<EnemyHealth>();
        room.GetComponent<BoxCollider2D>().enabled = false; // 새로운 방의 Collider 비활성화

        // 플레이어 스크립트에 현재 방과 층 정보 업데이트
        player.CurrentRoom = clickedRoomInfo.RoomNumber;
        player.CurrentFloor = clickedRoomInfo.CurrenFloor;
        // 천사방과 적의 방 상태를 업데이트
        player.IsAngelRoom = TowerGenerator.AngelRoomNumbers.Contains(clickedRoomInfo.RoomNumber);
        player.IsEnemyRoom = currentEnemyHealth != null && currentEnemyHealth.alive;

        Transform hideTransform = room.transform.Find("Hide"); // 'Hide' 오브젝트 찾기
        Transform itemsTransform = room.transform.Find("Items"); // 'Items' 오브젝트 찾기
        if (hideTransform != null) // 만약 hide가 있으면
        {
            SpriteRenderer spriteRenderer = hideTransform.GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
            if (spriteRenderer != null) // 스프라이트가 있으면
            {
                Color newColor = spriteRenderer.color;
                newColor.a = 0;  // 투명도를 0으로 조정
                spriteRenderer.color = newColor;  // 색상 변경 적용
                if(itemsTransform != null)//만약 아이템이 있으면
                {
                    itemsTransform.gameObject.SetActive(true); // 아이템 활성화
                }
            }
        }
        
    }
}
