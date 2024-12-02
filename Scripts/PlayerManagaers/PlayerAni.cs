using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAni : MonoBehaviour
{
    public GameObject player;
    public float speed = 5f; // 이동 속도
    private SkeletonAnimation skeletonAnimation;
    public bool Moveable = true;//연속이동 시 오류 생성 방지용
    [Header("효과음")]
    public bool isfoodSound = false;

    void Start()
    {
        skeletonAnimation = player.GetComponent<SkeletonAnimation>();
    }

    // targetPosition을 인자로 받아 움직임을 처리
    public void MoveToPosition(Vector3 targetPosition)
    {
        StartCoroutine(MovePlayer(targetPosition));
    }

    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        // 애니메이션을 'walking'으로 변경
        skeletonAnimation.AnimationName = "walking";
        Moveable = false;//walking 애니메이션 출력될 때 다른 방 이동 불가
        isfoodSound = true;//발소리 시작
        while (Vector3.Distance(player.transform.position, targetPosition) > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, 
                targetPosition, speed * Time.deltaTime);
            yield return null;  // 다음 프레임까지 기다림
        }

        // 위치 이동 후 애니메이션을 'idle'로 변경
        skeletonAnimation.AnimationName = "idle";  // 원하는 대기 애니메이션 이름으로 변경
        Moveable = true;//이제 다른 방 클릭해서 이동 가능
        isfoodSound = false;//발소리 끝
    }
    
    public IEnumerator MoveAlongPath(Vector3[] positions, float[] rotations, Action onComplete)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            // 걷는 애니메이션 실행
            skeletonAnimation.AnimationName = "walking";
            Moveable = false;
            isfoodSound = true;
            // 플레이어를 목표 위치로 이동
            while (Vector3.Distance(player.transform.position, positions[i]) > 0.01f)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, 
                    positions[i], 
                    speed * Time.deltaTime);
                yield return null;
            }

            // 플레이어가 위치에 도달한 후 회전과 카메라 업데이트
            player.transform.eulerAngles = new Vector3(0, rotations[i], 0);
        }

        // 이동 완료 후 대기 애니메이션으로 변경
        skeletonAnimation.AnimationName = "idle";
        Moveable = true;
        isfoodSound = false;//발소리 끝
        onComplete(); // 콜백 함수 호출
    }
    public IEnumerator PerformAttackAnimation()//BattleManager스크립트에서 사용
    {
        skeletonAnimation.AnimationName = "attack"; // 공격 애니메이션
        yield return new WaitForSeconds(1.0f); // 1초간 지속
        skeletonAnimation.AnimationName = "idle"; // 공격 완료 후 idle 상태로 복귀
    }
    
}
