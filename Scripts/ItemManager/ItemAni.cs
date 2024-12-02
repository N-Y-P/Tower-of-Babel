using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAni : MonoBehaviour
{
    public Transform targetPosition; // 아이템이 이동할 목표 위치
    public float UpmoveSpeed = 3.0f; //y축으로 올라가는 이동 속도
    public float BagmoveSpeed = 10.0f;   // 아이템 이동 속도(가방아이콘으로)

    // 아이템을 이동시키는 공개 메서드
    public void MoveItem(GameObject item)
    {
        StartCoroutine(MoveItemToPosition(item));
    }

    // 실제로 아이템을 이동시키는 코루틴
    private IEnumerator MoveItemToPosition(GameObject item)
    {
        // y축으로 0.7 이동
        Vector3 endPosition = new Vector3(item.transform.position.x, item.transform.position.y + 0.7f, item.transform.position.z);

        while (item.transform.position != endPosition)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, endPosition, UpmoveSpeed * Time.deltaTime);
            yield return null;
        }

        // 최종 목표 위치로 이동
        while (item.transform.position != targetPosition.position)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPosition.position, BagmoveSpeed * Time.deltaTime);
            yield return null;
        }

        // 도착 후 아이템 삭제
        Destroy(item);
    }
}
