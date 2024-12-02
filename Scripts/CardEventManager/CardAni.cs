using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAni : MonoBehaviour
{
    // ī�� ȸ�� �ӵ�
    public float rotationSpeed = 1f;

    // ī�带 ȸ����Ű�� �ڷ�ƾ �Լ�
    public IEnumerator RotateCard(GameObject card, float targetAngle, float duration)
    {
        float startRotation = card.transform.eulerAngles.y;
        float endRotation = targetAngle;
        float time = 0;

        while (time < duration)
        {
            float angle = Mathf.Lerp(startRotation, endRotation, time / duration);
            card.transform.eulerAngles = new Vector3(card.transform.eulerAngles.x, angle, card.transform.eulerAngles.z);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        card.transform.eulerAngles = new Vector3(card.transform.eulerAngles.x, endRotation, card.transform.eulerAngles.z);
    }

    // Ŭ���� ī�带 ȸ����Ű�� �� ī�带 Ȱ��ȭ�ϴ� �޼���
    public IEnumerator FlipCard(GameObject oldCard, GameObject newCard)
    {
        // Ŭ���� ī�带 90�� ȸ��
        yield return RotateCard(oldCard, 90, 0.5f);
        oldCard.SetActive(false);

        // �� ī�带 Ȱ��ȭ�ϰ� -90���� 0���� ȸ��
        newCard.SetActive(true);
        newCard.transform.eulerAngles = new Vector3(newCard.transform.eulerAngles.x, 90, newCard.transform.eulerAngles.z);
        yield return RotateCard(newCard, 0, 0.5f);
    }
}
