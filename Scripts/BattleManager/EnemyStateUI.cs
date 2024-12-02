using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyStateUI : MonoBehaviour
{
    public Player player;
    //클릭한 적의 체력 변동ui를 표시
    //1, 2, 3번 항목에 적절한 text 내용 부여
    [Header("적 상태")]
    public GameObject enemy_State;//적이 있는 방에서 true
    public TMP_Text hp_Text;

    [Header("배틀 스크린에서 적이 제공하는 정보")]
    public TMP_Text[] accuracyTexts;
    public TMP_Text[] damageMultiplierTexts;
    public TMP_Text[] guiltValueTexts;

    public void UpdateEnemyState(EnemyState enemyState, float currentHealth)
    {
        // 적의 현재 체력을 표시
        hp_Text.text = FormatText(currentHealth) + "/" + enemyState.enemy_Maxhp; // 적의 현재 체력 표시
                                                                     
        // 각 공격 옵션에 따른 값들을 UI에 업데이트
        for (int i = 0; i < enemyState.attackOptions.Length; i++)
        {
            if (i < accuracyTexts.Length && i < damageMultiplierTexts.Length && i < guiltValueTexts.Length)
            {
                accuracyTexts[i].text = "명중률 : " + 
                    (enemyState.attackOptions[i].accuracy +
                    player.AccuracyRate) + "%";

                damageMultiplierTexts[i].text = "데미지 : " +
                    FormatText(player.MinAttackCapability * enemyState.attackOptions[i].damageMultiplier) + " - " +
                    FormatText(player.MaxAttackCapability * enemyState.attackOptions[i].damageMultiplier);

                guiltValueTexts[i].text = "죄책감 : " + enemyState.attackOptions[i].guiltValue;
            }
        }
        string FormatText(float value)
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