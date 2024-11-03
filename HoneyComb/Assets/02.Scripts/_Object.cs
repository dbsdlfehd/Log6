using System;
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
    public bool isDialogged = true;

    public GameObject malPeungSeon;         // ��ǳ�� �̹���
    private Player player;                  // �÷��̾� ��ũ��Ʈ
    private float range = 2f;               // �÷��̾� ��������

	private void Start()
	{
		player = FindObjectOfType<Player>();// ������ ����ߵ� (�ʱ�ȭ)
	}

	private void FixedUpdate()
	{
        /*�÷��̾ ������ ������ ��ǳ�� ���̱�*/
		Vector2 direction = player.transform.position - transform.position;

		float X = Mathf.Abs(Mathf.Round(direction.x));
		float Y = Mathf.Abs(Mathf.Round(direction.y));

		if (isDialogged && (X<= range && Y <= range))//���𹮿��� �����ϼ�
        {
			malPeungSeon.SetActive(true);
		}
        else if (!isDialogged || (X >= range && Y >= range))//���𹮿��� �����ϼ�
		{
            malPeungSeon.SetActive(false);
        }
	}
}
