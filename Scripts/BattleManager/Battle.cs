using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{//�� Ŭ�� �� ��Ʋ ��ũ���� ������ �ϴ� ��ũ��Ʈ
    
    [Header("�ʿ��� ������Ʈ �Ҵ�")]
    public GameObject battleWindow;            // ���� â ������Ʈ
    public GameObject TransparentWall; //���� â
    public GameObject enemy_State_Obj;
    public GameObject bagbtn;

    [Header("���� ��ũ��Ʈ")]
    public ClickInputSystem clickInputSystem;  // Ŭ�� �Է� �ý��� ����
    public EnemyStateUI enemyStateUI;          // �� ���� UI ��ũ��Ʈ ����
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
            // ���⼭ ���� ���� �濡 ���� �� ó��
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
                Debug.Log("���� ����! ��: " + enemy.name);
            }
        }
    }
}