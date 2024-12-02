using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAni : MonoBehaviour
{
    // 카드 회전 속도
    public float rotationSpeed = 1f;

    // 카드를 회전시키는 코루틴 함수
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

    // 클릭한 카드를 회전시키고 새 카드를 활성화하는 메서드
    public IEnumerator FlipCard(GameObject oldCard, GameObject newCard)
    {
        // 클릭한 카드를 90도 회전
        yield return RotateCard(oldCard, 90, 0.5f);
        oldCard.SetActive(false);

        // 새 카드를 활성화하고 -90에서 0도로 회전
        newCard.SetActive(true);
        newCard.transform.eulerAngles = new Vector3(newCard.transform.eulerAngles.x, 90, newCard.transform.eulerAngles.z);
        yield return RotateCard(newCard, 0, 0.5f);
    }
}
