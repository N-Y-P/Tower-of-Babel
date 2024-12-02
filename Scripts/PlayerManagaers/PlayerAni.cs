using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAni : MonoBehaviour
{
    public GameObject player;
    public float speed = 5f; // �̵� �ӵ�
    private SkeletonAnimation skeletonAnimation;
    public bool Moveable = true;//�����̵� �� ���� ���� ������
    [Header("ȿ����")]
    public bool isfoodSound = false;

    void Start()
    {
        skeletonAnimation = player.GetComponent<SkeletonAnimation>();
    }

    // targetPosition�� ���ڷ� �޾� �������� ó��
    public void MoveToPosition(Vector3 targetPosition)
    {
        StartCoroutine(MovePlayer(targetPosition));
    }

    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        // �ִϸ��̼��� 'walking'���� ����
        skeletonAnimation.AnimationName = "walking";
        Moveable = false;//walking �ִϸ��̼� ��µ� �� �ٸ� �� �̵� �Ұ�
        isfoodSound = true;//�߼Ҹ� ����
        while (Vector3.Distance(player.transform.position, targetPosition) > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, 
                targetPosition, speed * Time.deltaTime);
            yield return null;  // ���� �����ӱ��� ��ٸ�
        }

        // ��ġ �̵� �� �ִϸ��̼��� 'idle'�� ����
        skeletonAnimation.AnimationName = "idle";  // ���ϴ� ��� �ִϸ��̼� �̸����� ����
        Moveable = true;//���� �ٸ� �� Ŭ���ؼ� �̵� ����
        isfoodSound = false;//�߼Ҹ� ��
    }
    
    public IEnumerator MoveAlongPath(Vector3[] positions, float[] rotations, Action onComplete)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            // �ȴ� �ִϸ��̼� ����
            skeletonAnimation.AnimationName = "walking";
            Moveable = false;
            isfoodSound = true;
            // �÷��̾ ��ǥ ��ġ�� �̵�
            while (Vector3.Distance(player.transform.position, positions[i]) > 0.01f)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, 
                    positions[i], 
                    speed * Time.deltaTime);
                yield return null;
            }

            // �÷��̾ ��ġ�� ������ �� ȸ���� ī�޶� ������Ʈ
            player.transform.eulerAngles = new Vector3(0, rotations[i], 0);
        }

        // �̵� �Ϸ� �� ��� �ִϸ��̼����� ����
        skeletonAnimation.AnimationName = "idle";
        Moveable = true;
        isfoodSound = false;//�߼Ҹ� ��
        onComplete(); // �ݹ� �Լ� ȣ��
    }
    public IEnumerator PerformAttackAnimation()//BattleManager��ũ��Ʈ���� ���
    {
        skeletonAnimation.AnimationName = "attack"; // ���� �ִϸ��̼�
        yield return new WaitForSeconds(1.0f); // 1�ʰ� ����
        skeletonAnimation.AnimationName = "idle"; // ���� �Ϸ� �� idle ���·� ����
    }
    
}
