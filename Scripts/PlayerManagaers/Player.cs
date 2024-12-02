using JetBrains.Annotations;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /***�������ΰ�***/
    [Header("���� ��")]
    public bool isInCombat = false;

    /***���� ��, �� �ѹ�***/
    [Header("��, �� ����")]
    [SerializeField] private int current_room = 0;//���� ��
    [SerializeField] private int current_floor = 0;// ���� ��
    [SerializeField] private bool isAngelRoom = false;//���� õ�������
    [SerializeField] private bool isEnemyRoom = false;
    [Space(10)]

    /***ü��***/
    [Header("ü��")]
    [SerializeField] private int max_Hp = 100;//ü�� �ִ�ġ
    [SerializeField] private float current_hp = 100;//���� ü��
    [Space(10)]

    /***���ŷ�***/
    [Header("���ŷ�")]
    [SerializeField] private int max_Mental = 100;//���ŷ� �ִ�ġ
    [SerializeField] private float current_mental = 0;//���� ���ŷ�
    [Space(10)]

    /***����ġ***/
    [Header("����ġ")]
    [SerializeField] private float exp = 50;//ī�� �̺�Ʈ���� �ʿ��� ����ġ
    [SerializeField] private int current_exp = 0;//���� ����ġ

    /***���߷�***/
    //ī����� �� ���. �⺻�� 0
    [Header("���߷�")]
    [SerializeField] private int accuracy_Rate = 0;

    /***�⺻���ݷ�***/
    //ī�� ���� �� ���. �⺻�� 0
    [Header("���� ���ݷ�")]
    [SerializeField] private int basic_Capability = 0;

    /***���� ���ݷ�***/
    //���� �÷��̾� ���ݷ�(���� ���ݷ� + ���� ���ݷ�)
    [Header("���� ���ݷ�")]
    private Equipment equippedWeapon; // �÷��̾ ���� ������ ����
    //���� �ּҰ��ݷ��� ���� ���ݷ� + (���� ������ ���Ⱑ �ִٸ� ���������� �ּ� ������)
    public int MinAttackCapability => basic_Capability + (equippedWeapon != null ? equippedWeapon.minDamage : 2);
    public int MaxAttackCapability => basic_Capability + (equippedWeapon != null ? equippedWeapon.maxDamage : 3);


    // ���� ���� �޼���
    public void EquipWeapon(Equipment weapon)
    {
        equippedWeapon = weapon;
    }
    // ������Ƽ
    public int CurrentRoom
    {
        get => current_room;
        set => current_room = value;
    }

    // ���� ���� ���� public ������Ƽ
    public event Action<int> OnFloorChange;
    public int CurrentFloor
    {
        get => current_floor;
        set
        {
            if (current_floor != value)
            {
                current_floor = value;
                OnFloorChange?.Invoke(current_floor); // �� ���� �̺�Ʈ �߻�
            }
        }
    }
    public bool IsAngelRoom
    {
        get => isAngelRoom;
        set => isAngelRoom = value;
    }

    public bool IsEnemyRoom
    {
        get => isEnemyRoom;
        set => isEnemyRoom = value;
    }

    public int MaxHP
    {
        get => max_Hp;
        set => max_Hp = Mathf.Max(value, 0);
    }

    public float CurrentHP
    {
        get => current_hp;
        set => current_hp = Mathf.Clamp(value, 0, max_Hp);
    }

    public int MaxMental
    {
        get => max_Mental;
        set => max_Mental = Mathf.Max(value, 0);
    }

    public float CurrentMental
    {
        get => current_mental;
        set => current_mental = Mathf.Clamp(value, 0, max_Mental);
    }

    public float ExpRequired
    {
        get => exp;
        set => exp = Mathf.Max(value, 0);
    }

    public int CurrentExp
    {
        get => current_exp;
        set => current_exp = Mathf.Max(value, 0);
    }
    public int AccuracyRate
    {
        get => accuracy_Rate;
        set => accuracy_Rate = Mathf.Clamp(value, 0, 100);
    }

    public int BasicCapability
    {
        get => basic_Capability;
        set => basic_Capability = Mathf.Max(value, 0);
    }
}
