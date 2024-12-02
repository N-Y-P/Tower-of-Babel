using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    public ClickInputSystem clickInputSystem;  // 클릭 입력 시스템 참조
    public CardProbability cardProbability;   // 카드 확률 관리 스크립트 참조
    public CardManager cardManager;
    public CardAni cardAni;
    private void OnEnable()
    {
        clickInputSystem.OnCardClicked += HandleCardClicked;
    }

    private void OnDisable()
    {
        clickInputSystem.OnCardClicked -= HandleCardClicked;
    }
    private void HandleCardClicked(GameObject card)
    {
        // 이미 카드가 선택된 경우 추가 클릭 무시
        if (cardManager.cardSelected) return;

        Debug.Log("클릭된 카드: " + card.name);

        // 클릭된 카드 이름과 일치하는 카테고리 찾기
        CardCategory category = cardProbability.CardCategories
            .FirstOrDefault(cat => cat.name == card.name);

        if (category != null && category.Cards.Count > 0)
        {
            int randomIndex = Random.Range(0, category.Cards.Count);
            GameObject selectedCard = category.Cards[randomIndex];

            // 애니메이션 시작 전에 새 카드를 생성하지만 비활성화 상태로 둔다.
            GameObject newCard = Instantiate(selectedCard, category.CardPosition.position, Quaternion.Euler(0, -90, 0), category.CardPosition);
            newCard.SetActive(false);

            // 카드 회전 애니메이션 실행 후 새 카드 활성화
            StartCoroutine(cardAni.FlipCard(card, newCard));
            // 카드 데이터에서 카드 정보 가져오기
            CardData cardData = selectedCard.GetComponent<CardData>();
            cardManager.ApplyCardEffect(cardData.CardInfo);

            // 카드 이벤트 완료 처리
            cardManager.cardSelected = true;  // 다른 카드 선택 방지
            // 3초 후에 카드 이벤트를 리셋
            StartCoroutine(ResetCardEventAfterDelay(3));
            Debug.Log("생성된 카드: " + selectedCard.name);
        }
        else
        {
            Debug.Log("적절한 카테고리 또는 카드가 없습니다.");
        }
    }
    IEnumerator ResetCardEventAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cardManager.ResetCardEvent();
    }
}
