using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Object : MonoBehaviour
{
    public int id;//NPC��ȣ

    //public bool isNPC;//NPC���� Ȯ���ϴ� �ǵ� �ʿ� ���µ�

    //NPC�� Ƚ���� ��ȭ Ƚ��
    public int EachTalkCount = 0;
    public int TempEachTalkCount = 0;

    //�׽�Ʈ�� �̸� ����
    public new string name;

    //NPC�� ������ ��ȭ Ȯ�ο� bool�� ����
	public bool isDeadUp = false;
	public bool isUpGraded = false;

    public int PlayerDead;
    public int tempPlayerDead = 0;
}
