using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Transform canvasTransform; // Canvas받기
    public GameObject TransparentWall;

    // 현재 활성화된 창을 비활성화하는 메소드

    //x버튼 
    //GameBtnEvent에서 가져감
    public void CloseActiveWindow()
    {
        // Canvas의 모든 자식을 검사, 활성화된 창을 찾고 비활성화
        foreach (Transform child in canvasTransform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
                break; // 첫 번째 활성화된 창을 비활성화 한 후 루프 종료
            }
        }
    }
}
