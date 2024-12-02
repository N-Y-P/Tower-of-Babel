using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance { get; private set; }

    public PlayerMovement playerMovement;
    public GameObject HitTextPrefab;
    [Header("���� ���� UI Text ����")]
    public TMP_Text enemyText;
    public TMP_Text playerText_1;
    public TMP_Text playerText_2;
    public TMP_Text expText;
    public float textSpeed = 10.0f;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void DisplayEnemyDamage(float damage)//�÷��̾ ���� ����
    {
        StartCoroutine(AnimateText(enemyText, $"-{damage:F1}"));
    }

    public void DisplayPlayerDamage(float damage)//���� �÷��̾ ����
    {
        if(playerMovement.isPoint1)
        {
            StartCoroutine(AnimateText(playerText_1, $"-{damage:F1}"));
        }
        else
        {
            StartCoroutine(AnimateText(playerText_2, $"-{damage:F1}"));
        }
        
    }

    public void DisplayExperienceGained(int exp)
    {
        StartCoroutine(AnimateText(expText, $"+{exp} ����"));
    }
    public void DisplayMissDamage()
    {
        StartCoroutine(AnimateText(enemyText, "��ħ"));
    }
    public void DisplayEnemyMissDamage()
    {
        if (playerMovement.isPoint1)
        {
            StartCoroutine(AnimateText(playerText_1, "��ħ"));
        }
        else
        {
            StartCoroutine(AnimateText(playerText_2, "��ħ"));
        }
    }
    public void ClearCombatTexts()
    {
        enemyText.text = "";
        playerText_1.text = "";
        expText.text = "";
    }
    IEnumerator AnimateText(TMP_Text textComponent, string message)
    {
        textComponent.text = message;
        Vector3 originalPosition = textComponent.rectTransform.localPosition;
        Vector3 targetPosition = originalPosition + new Vector3(0, 20f, 0); // y������ 20 ���� ���

        float elapsedTime = 0;
        while (elapsedTime < textSpeed)
        {
            textComponent.rectTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / textSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textComponent.rectTransform.localPosition = targetPosition;
        yield return new WaitForSeconds(0.0f); // �ؽ�Ʈ�� ������ 2�ʰ� �ӹ���

        textComponent.text = ""; // �ؽ�Ʈ ������ �����
        textComponent.rectTransform.localPosition = originalPosition; // ���� ��ġ�� ���ƿ�
    }
    public void HitTextPosition(bool isPoint1)
    {
        if (isPoint1)
        {
            HitTextPrefab.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            HitTextPrefab.transform.localPosition = new Vector3(320, 0, 0);
        }
    }
}
