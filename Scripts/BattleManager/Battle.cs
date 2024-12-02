using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{//적 클릭 시 배틀 스크린이 나오게 하는 스크립트
    
    [Header("필요한 오브젝트 할당")]
    public GameObject battleWindow;            // 전투 창 오브젝트
    public GameObject TransparentWall; //투명 창
    public GameObject enemy_State_Obj;
    public GameObject bagbtn;

    [Header("참조 스크립트")]
    public ClickInputSystem clickInputSystem;  // 클릭 입력 시스템 참조
    public EnemyStateUI enemyStateUI;          // 적 상태 UI 스크립트 참조
    public PlayerMovement playerMovement;
    public Player player;

    private void OnEnable()
    {
        clickInputSystem.OnEnemyClicked += HandleEnemyClicked;
    }

    private void OnDisable()
    {
        clickInputSystem.OnEnemyClicked -= HandleEnemyClicked;
    }

    private void HandleEnemyClicked(GameObject enemy)
    {
        RoomInfo playerRoomInfo = playerMovement.GetCurrentRoomInfo();
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        EnemyAni enemyAni = enemy.GetComponent<EnemyAni>();
        
        if (enemyAni != null && playerRoomInfo.RoomNumber == enemyAni.roomNum)
        {
            // 여기서 적이 같은 방에 있을 때 처리
            BattleUIManager.Instance.HitTextPosition(playerMovement.isPoint1);
            if (enemyHealth != null && enemyHealth.alive)
            {
                TransparentWall.SetActive(true);
                //enemyStateUI.UpdateEnemyState(enemyHealth.enemyState, enemyHealth.currentHealth);
                BattleManager.Instance.SetCurrentEnemy(enemyHealth.enemyState, enemyHealth, enemyAni);
                battleWindow.SetActive(true);
                enemy_State_Obj.SetActive(true);
                player.isInCombat = true; 
                bagbtn.SetActive(false);
                Debug.Log("전투 시작! 적: " + enemy.name);
            }
        }
    }
}