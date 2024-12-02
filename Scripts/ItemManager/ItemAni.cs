using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAni : MonoBehaviour
{
    public Transform targetPosition; // �������� �̵��� ��ǥ ��ġ
    public float UpmoveSpeed = 3.0f; //y������ �ö󰡴� �̵� �ӵ�
    public float BagmoveSpeed = 10.0f;   // ������ �̵� �ӵ�(�������������)

    // �������� �̵���Ű�� ���� �޼���
    public void MoveItem(GameObject item)
    {
        StartCoroutine(MoveItemToPosition(item));
    }

    // ������ �������� �̵���Ű�� �ڷ�ƾ
    private IEnumerator MoveItemToPosition(GameObject item)
    {
        // y������ 0.7 �̵�
        Vector3 endPosition = new Vector3(item.transform.position.x, item.transform.position.y + 0.7f, item.transform.position.z);

        while (item.transform.position != endPosition)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, endPosition, UpmoveSpeed * Time.deltaTime);
            yield return null;
        }

        // ���� ��ǥ ��ġ�� �̵�
        while (item.transform.position != targetPosition.position)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPosition.position, BagmoveSpeed * Time.deltaTime);
            yield return null;
        }

        // ���� �� ������ ����
        Destroy(item);
    }
}
