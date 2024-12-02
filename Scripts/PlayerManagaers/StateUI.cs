using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateUI : MonoBehaviour
{
    public Player player; // �÷��̾� ����
    [Header("�κ��丮 �÷��̾� ����")]
    public Image inven_HealthBar; // ü�¹� UI
    public Image inven_MentalBar; // ���ŷ¹� UI
    public Image inven_ExpBar; // ����ġ�� UI
    public TMP_Text inven_Hp;
    public TMP_Text inven_Mental;
    public TMP_Text inven_Exp;
    [Header("�ǽð� �÷��̾� ����")]
    public TMP_Text player_Hp;
    public TMP_Text player_Mental;
    //[Header("�� ����")]
    //�� ���� ��������
    void Update()
    {
        if (player != null)
        {
            UpdateHealth(player.CurrentHP, player.MaxHP);
            UpdateMental(player.CurrentMental, player.MaxMental);
            UpdateExperience(player.CurrentExp, player.ExpRequired);

            // ü�� �ؽ�Ʈ ������Ʈ
            inven_Hp.text = FormatHealthText(player.CurrentHP) + "/" + player.MaxHP;
            player_Hp.text = FormatHealthText(player.CurrentHP) + "/" + player.MaxHP;

            // ���ŷ� �ؽ�Ʈ ������Ʈ 
            inven_Mental.text = FormatHealthText(player.CurrentMental) + "/" + player.MaxMental;
            player_Mental.text = FormatHealthText(player.CurrentMental) + "/" + player.MaxMental;

            // ����ġ �ؽ�Ʈ ������Ʈ
            inven_Exp.text = FormatHealthText(player.CurrentExp) + "/" + player.ExpRequired;

            // ���� ���˿� ���� �Ҽ��� ǥ�� ���� ����
            string FormatHealthText(float value)
            {
                if (value % 1 == 0)  // �Ҽ��� ���ϰ� 0�̸� ������ ó��
                {
                    return value.ToString("F0"); // �Ҽ��� ����
                }
                else
                {
                    return value.ToString("F1"); // �Ҽ��� �� �ڸ�
                }
            }
        }
    }
    // �÷��̾� ���� ������Ʈ�� �޾� UI�� ������Ʈ�ϴ� �޼���
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        inven_HealthBar.fillAmount = currentHealth / maxHealth;
    }

    public void UpdateMental(float currentMental, float maxMental)
    {
        inven_MentalBar.fillAmount = currentMental / maxMental;
    }

    public void UpdateExperience(float currentExp, float requiredExp)
    {
        inven_ExpBar.fillAmount = currentExp / requiredExp;
    }
}
