using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateUI : MonoBehaviour
{
    public Player player; // 플레이어 참조
    [Header("인벤토리 플레이어 상태")]
    public Image inven_HealthBar; // 체력바 UI
    public Image inven_MentalBar; // 정신력바 UI
    public Image inven_ExpBar; // 경험치바 UI
    public TMP_Text inven_Hp;
    public TMP_Text inven_Mental;
    public TMP_Text inven_Exp;
    [Header("실시간 플레이어 상태")]
    public TMP_Text player_Hp;
    public TMP_Text player_Mental;
    //[Header("적 상태")]
    //적 정보 가져오기
    void Update()
    {
        if (player != null)
        {
            UpdateHealth(player.CurrentHP, player.MaxHP);
            UpdateMental(player.CurrentMental, player.MaxMental);
            UpdateExperience(player.CurrentExp, player.ExpRequired);

            // 체력 텍스트 업데이트
            inven_Hp.text = FormatHealthText(player.CurrentHP) + "/" + player.MaxHP;
            player_Hp.text = FormatHealthText(player.CurrentHP) + "/" + player.MaxHP;

            // 정신력 텍스트 업데이트 
            inven_Mental.text = FormatHealthText(player.CurrentMental) + "/" + player.MaxMental;
            player_Mental.text = FormatHealthText(player.CurrentMental) + "/" + player.MaxMental;

            // 경험치 텍스트 업데이트
            inven_Exp.text = FormatHealthText(player.CurrentExp) + "/" + player.ExpRequired;

            // 숫자 포맷에 따라 소수점 표시 유무 결정
            string FormatHealthText(float value)
            {
                if (value % 1 == 0)  // 소수점 이하가 0이면 정수로 처리
                {
                    return value.ToString("F0"); // 소수점 없음
                }
                else
                {
                    return value.ToString("F1"); // 소수점 한 자리
                }
            }
        }
    }
    // 플레이어 상태 업데이트를 받아 UI를 업데이트하는 메서드
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
