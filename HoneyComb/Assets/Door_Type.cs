using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Type : MonoBehaviour
{
    void Start()
    {
    }
}


public enum DoorType // �� ���� ���� ex. ü�¹�, ���������� ���
{
    hpUP,            // �ִ� ü�� ���� ��
    atkUP,           // ���ݷ� ���� ��
    EmotionRound,    // ���� ���� ȹ�� ��
    MoneyUp          // ��ȭ ȹ�� ��
}
