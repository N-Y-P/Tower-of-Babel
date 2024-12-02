using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    public ClickInputSystem clickInputSystem;  // Ŭ�� �Է� �ý��� ����
    public CardProbability cardProbability;   // ī�� Ȯ�� ���� ��ũ��Ʈ ����
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
        // �̹� ī�尡 ���õ� ��� �߰� Ŭ�� ����
        if (cardManager.cardSelected) return;

        Debug.Log("Ŭ���� ī��: " + card.name);

        // Ŭ���� ī�� �̸��� ��ġ�ϴ� ī�װ� ã��
        CardCategory category = cardProbability.CardCategories
            .FirstOrDefault(cat => cat.name == card.name);

        if (category != null && category.Cards.Count > 0)
        {
            int randomIndex = Random.Range(0, category.Cards.Count);
            GameObject selectedCard = category.Cards[randomIndex];

            // �ִϸ��̼� ���� ���� �� ī�带 ���������� ��Ȱ��ȭ ���·� �д�.
            GameObject newCard = Instantiate(selectedCard, category.CardPosition.position, Quaternion.Euler(0, -90, 0), category.CardPosition);
            newCard.SetActive(false);

            // ī�� ȸ�� �ִϸ��̼� ���� �� �� ī�� Ȱ��ȭ
            StartCoroutine(cardAni.FlipCard(card, newCard));
            // ī�� �����Ϳ��� ī�� ���� ��������
            CardData cardData = selectedCard.GetComponent<CardData>();
            cardManager.ApplyCardEffect(cardData.CardInfo);

            // ī�� �̺�Ʈ �Ϸ� ó��
            cardManager.cardSelected = true;  // �ٸ� ī�� ���� ����
            // 3�� �Ŀ� ī�� �̺�Ʈ�� ����
            StartCoroutine(ResetCardEventAfterDelay(3));
            Debug.Log("������ ī��: " + selectedCard.name);
        }
        else
        {
            Debug.Log("������ ī�װ� �Ǵ� ī�尡 �����ϴ�.");
        }
    }
    IEnumerator ResetCardEventAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cardManager.ResetCardEvent();
    }
}
