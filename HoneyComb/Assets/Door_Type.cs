using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Type : MonoBehaviour
{
    void Start()
    {
        Debug.Log("���� �� ���� �� ���� �غ��~"); // �峭���� �ẽ����
    }
}


public enum DoorType // �� ���� ���� ex. ü�¹�, ���������� ���
{
    hpUP,            // �ִ� ü�� ���� ��
    atkUP,           // ���ݷ� ���� ��
    EmotionRound,    // ���� ���� ȹ�� ��
    MoneyUp          // ��ȭ ȹ�� ��
}
