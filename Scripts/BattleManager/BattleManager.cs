using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemyState;

public class BattleManager : MonoBehaviour
{
    [Header("�ʿ��� ������Ʈ �Ҵ�")]
    public GameObject enemy_State_Obj;//�� ���� ������Ʈ(�� ��� �� ��Ȱ ����)
    public GameObject battleWindow; // ���� â ������Ʈ
    public GameObject TransparentWall;//���� â

    [Header("���� ��ũ��Ʈ")]
    public Player player; // �÷��̾� ����
    public EnemyState currentEnemy; // ���� ���� ���� ��
    public EnemyStateUI currentEnemyUI;
    public EnemyHealth currentEnemyHealth;
    public EnemyAni currentEnemyAni;
    public PlayerAni playerAni;
    public PlayerMovement playerMovement;
    [Header("ȿ����")]
    public bool missAttack;

    public static BattleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if(!player.isInCombat)
        {
            SearchEnemy();
        }
    }
    
    public void SearchEnemy()
    {
        RoomInfo playerRoomInfo = playerMovement.GetCurrentRoomInfo();
        if (playerRoomInfo == null) return;

        bool enemyFound = false;

        // �� ����Ʈ���� ���� �� ��ȣ�� ��ġ�ϴ� ���� ã�� ȸ����Ű��
        foreach (var enemy in FindObjectsOfType<EnemyAni>()) // ��� ���� �˻�
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemy.roomNum == playerRoomInfo.RoomNumber && enemyHealth.alive) 
                // �÷��̾ �ִ� �� ��ȣ�� ���� �� ��ȣ�� ��ġ�� ���
            {
                enemyFound = true;
                if (playerMovement.isPoint1)
                {
                    enemy.SetRotationYToZero(); // �÷��̾ point1�� ������ ���� 0���� ȸ��
                }
                else
                {
                    enemy.SetRotationYToOneEighty(); // �÷��̾ point2�� ������ ���� 180���� ȸ��
                }
                // �� ���� UI Ȱ��ȭ �� ���� ������Ʈ
                currentEnemyUI.enemy_State.SetActive(true);
                currentEnemyUI.UpdateEnemyState(enemyHealth.enemyState, enemyHealth.currentHealth);
                break;
            }
        }
        if (!enemyFound)
        {
            // �÷��̾ �ִ� �濡 ���� ���ų� ��� ���� ���� ���
            currentEnemyUI.enemy_State.SetActive(false);
            player.isInCombat = false;

        }
    }
    /*
    public void SearchEnemy(RoomInfo playerRoomInfo)
    {
        if (playerRoomInfo == null) return;

        bool enemyFound = false;

        foreach (var enemy in FindObjectsOfType<EnemyAni>()) // ��� ���� �˻�
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemy.roomNum == playerRoomInfo.RoomNumber && enemyHealth.alive) // �÷��̾ �ִ� �� ��ȣ�� ���� �� ��ȣ�� ��ġ�� ���
            {
                enemyFound = true;
                if (playerMovement.isPoint1)
                {
                    enemy.SetRotationYToZero(); // �÷��̾ point1�� ������ ���� 0���� ȸ��
                }
                else
                {
                    enemy.SetRotationYToOneEighty(); // �÷��̾ point2�� ������ ���� 180���� ȸ��
                }
                // �� ���� UI Ȱ��ȭ �� ���� ������Ʈ
                currentEnemyUI.enemy_State.SetActive(true);
                currentEnemyUI.UpdateEnemyState(enemyHealth.enemyState, enemyHealth.currentHealth);
                break;
            }
        }

        if (!enemyFound)
        {
            // �÷��̾ �ִ� �濡 ���� ���ų� ��� ���� ���� ���
            currentEnemyUI.enemy_State.SetActive(false);
            player.isInCombat = false;
        }
    }*/
    // �� �޼ҵ�� �ܺο��� ���� ���� ���� ���� EnemyState�� EnemyHealth�� �����ϱ� ���� ���
    public void SetCurrentEnemy(EnemyState enemyState, EnemyHealth enemyHealth, EnemyAni enemyAni)
    {
        currentEnemy = enemyState;
        currentEnemyHealth = enemyHealth;
        currentEnemyAni = enemyAni;
        // �� ���� ���� ��, �ʿ��ϴٸ� UI ������Ʈ ������ �߰� ����
        if (currentEnemyUI != null && currentEnemyHealth != null)
        {
            currentEnemyUI.UpdateEnemyState(currentEnemy, currentEnemyHealth.currentHealth);
        }
    }
    #region /***��ư �޼ҵ�***/
    // �� ��ư Ŭ���� ȣ��� �޼ҵ�
    public void AttackOption1()
    {
        StartCoroutine(CombatSequence(0));
    }

    public void AttackOption2()
    {
        StartCoroutine(CombatSequence(1));   
    }

    public void AttackOption3()
    {
        StartCoroutine(CombatSequence(2));
    }
    #endregion
    #region/***���ݽ���, ������, ��å�� �޼ҵ�***/
    // ���� ���� �޼ҵ�
    private void PerformAttack(int optionIndex)
    {
        if (currentEnemyHealth == null || optionIndex >= currentEnemyHealth.enemyState.attackOptions.Length)
        {
            Debug.LogError("�߸��� ���� �ɼ��̰ų� ���� �������� �ʾҽ��ϴ�.");
            return;
        }

        var attackOption = currentEnemyHealth.enemyState.attackOptions[optionIndex];

        // ���� Ȯ���� ����Ͽ� ������ ���� ���� ����
        if (Random.Range(0, 100) < attackOption.accuracy)
        {
            // ���� ���� ��
            // ���� ü�� ���� 
            float damage = CalculateDamage(attackOption.damageMultiplier);
            currentEnemyHealth.TakeDamage(damage);

            if (currentEnemyHealth.currentHealth <= 0)
            {
                enemy_State_Obj.SetActive(false);  // �� UI ��Ȱ��ȭ
            }

            // �� ü�� UI ������Ʈ
            if (currentEnemyUI != null)
            {
                currentEnemyUI.UpdateEnemyState(currentEnemyHealth.enemyState, currentEnemyHealth.currentHealth);  
                // ü�� ǥ�� ������Ʈ
            }

            // ��å�� ����
            ApplyGuilt(attackOption.guiltValue);
            
            BattleUIManager.Instance.DisplayEnemyDamage(damage);
            Debug.Log($"�� ���� �ɼ� {optionIndex + 1}�� ���. ����");
        }
        else
        {
            BattleUIManager.Instance.DisplayMissDamage();
            // ���� �̽� ��
            Debug.Log($"�� ���� �ɼ� {optionIndex + 1}�� ���. �̽�");
            missAttack = true;

        }
    }

    // ������ ��� �޼ҵ�
    private float CalculateDamage(float damageMultiplier)
    {
        // ���⿡�� �÷��̾��� ���ݷ��� �������� �������� ���
        float minDamage = player.MinAttackCapability * damageMultiplier;  // �Ҽ��� ����� �����Ͽ� �̴ϸ� ������ ���
        float maxDamage = player.MaxAttackCapability * damageMultiplier;  // �Ҽ��� ����� �����Ͽ� �ƽø� ������ ���
        float finalDamage = Random.Range(minDamage, maxDamage);  // �̴ϸذ� �ƽø� ������ ���� ���� ���� (�Ҽ��� ����)

        // �Ҽ��� ù° �ڸ����� �ݿø��Ͽ� ó��
        finalDamage = Mathf.Round(finalDamage * 10f) / 10f;

        Debug.Log($"���� �� ���� {finalDamage}");
        return finalDamage;  // �Ҽ��� ù° �ڸ�����
    }

    // ��å�� ���� �޼ҵ�
    private void ApplyGuilt(int guiltValue)
    {
        // ���⿡�� �÷��̾��� ���� ���¿� ��å���� ����
        player.CurrentMental += guiltValue;
    }
    #endregion
    #region/*** ���ݰ� �ݰ��� ó���ϴ� �ڷ�ƾ***/
    IEnumerator CombatSequence(int optionIndex)
    {
        // ��Ʋ ��ũ���� ��Ȱ��ȭ
        battleWindow.SetActive(false);
        

        yield return new WaitForSeconds(0.5f);  // ª�� ����

        // �÷��̾� ����
        StartCoroutine(playerAni.PerformAttackAnimation());
        yield return new WaitForSeconds(0.5f);
        PerformAttack(optionIndex);
        if(currentEnemyHealth.currentHealth <= 0)
        {
            //���� �׾��ٸ� ����ġ �ο� 
            StartCoroutine(currentEnemyAni.EnmeylandAnimation());
            yield return new WaitForSeconds(0.3f);
            player.CurrentExp += currentEnemyHealth.enemyState.enemy_Exp;
            BattleUIManager.Instance.DisplayExperienceGained(currentEnemyHealth.enemyState.enemy_Exp);
        }
        yield return new WaitForSeconds(1.0f);  // �÷��̾� ���� �� ���

        // �� �ݰ� 
        if (currentEnemyHealth.currentHealth > 0)
        {
            StartCoroutine(currentEnemyAni.EnemyAttackAnimation());
            yield return new WaitForSeconds(0.5f);
            EnemyCounterAttack();
            yield return new WaitForSeconds(1.0f);  // �� ���� �� ���
        }

        // ���� �� ��Ȱ��ȭ
        TransparentWall.SetActive(false);

    }
    public void EnemyCounterAttack()
    {

        // ���� �÷��̾�� ����
        /*
        float damage = currentEnemy.GetRandomDamage();
        player.CurrentHP -= damage;
        BattleUIManager.Instance.DisplayPlayerDamage(damage);
        Debug.Log($"���� {damage} ��ŭ ����");
        */
        if (Random.Range(0, 100) < currentEnemy.enemyAccuracy)
        {
            float damage = currentEnemy.GetRandomDamage();
            player.CurrentHP -= damage;
            BattleUIManager.Instance.DisplayPlayerDamage(damage);
            Debug.Log($"���� {damage} ��ŭ ����");
        }
        else
        {//���� �÷��̾�� ���� �̽�
            BattleUIManager.Instance.DisplayEnemyMissDamage();
            Debug.Log("�� ���� �̽�");
            missAttack = true;
        }
    }
    #endregion
    public void PerformRoomEnemyCounterAttack(EnemyHealth enemyHealth)
    {
        if (enemyHealth != null && enemyHealth.alive)
        {
            StartCoroutine(ExecuteRoomEnemyCounterAttack(enemyHealth));
        }
    }

    private IEnumerator ExecuteRoomEnemyCounterAttack(EnemyHealth enemyHealth)
    {
        // ���� ���� �ִϸ��̼� ����
        EnemyAni enemyAni = enemyHealth.GetComponent<EnemyAni>();
        if (enemyAni != null)
        {
            StartCoroutine(enemyAni.EnemyAttackAnimation());
            yield return new WaitForSeconds(0.5f); // �ִϸ��̼� ���
        }

        // ���� �÷��̾�� �ݰ�
        if (Random.Range(0, 100) < enemyHealth.enemyState.enemyAccuracy)
        {
            float damage = enemyHealth.enemyState.GetRandomDamage();
            player.CurrentHP -= damage;
            BattleUIManager.Instance.DisplayPlayerDamage(damage);
        }
        else
        {//���� �÷��̾�� ���� �̽�
            BattleUIManager.Instance.DisplayEnemyMissDamage();
            Debug.Log("�� ���� �̽�");
            missAttack = true;
        }

        yield return new WaitForSeconds(1.0f); // �ݰ� �� �߰� ���
    }
}
