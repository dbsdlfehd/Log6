using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
	[Header("��� �Ȱ��� �ٿ��� ��")]
	public GameObject ScanObject;

	[Header("�÷��̾� ��ũ��Ʈ")]
	public Player player;

	[Header("�ľŸŴ���")]
	public ParsingManager parsingManager;

	[Header("��ȭâ")]
	public TextMeshProUGUI Talker;
	public TextMeshProUGUI Dialog;

	[Header("ȸ���� ��ȭ Ƚ��")]
	public TextMeshProUGUI TalkCountNum;

	string talker = "";
	string dialog = "";
	public int tempId = 0;
	public int tempDeadCount = 0;
	public int TalkCount = 0;
	bool isDeadUp = false;
	bool isUpGraded = false;

	public bool isOverIndex = false;
	//��ȭ �׼� �Լ�
	public void DialogAction(GameObject scannedObject)
    {
		//��ĵ�� ������Ʈ
		ScanObject = scannedObject;
		
		//��ĵ�� ������Ʈ�� ������Ʈ�� �����ɴϴ�.
		_Object obj = scannedObject.GetComponent<_Object>();
		
		//��ĵ�� ������Ʈ�� ���̵�� NPC���θ� Ȯ���մϴ�.
		//Debug.Log(obj.id + " " + obj.isNPC);

		//��ĵ�� ������Ʈ�� ���̵�� NPC���θ� Talk�Լ��� �����ϴ�.
		Talk(obj.id, obj.isNPC);
		tempId = obj.id;
	}

	private void Update()
	{
		//UI�� ��Ÿ����
		Talker.text = talker.ToString();
		Dialog.text = dialog.ToString();
		TalkCountNum.text = "ȸ���� ��ȭ Ƚ�� : " + TalkCount.ToString();
	}

	public int i = 0;
	void Talk(int id, bool isNPC)
	{
		//���� Ƚ���� �ٲ����?
		if (tempDeadCount != 0 && tempDeadCount != player.DeadCount)
		{
			i = 0;
			isDeadUp = true;//
		}

		//���� ��ȭ�� Ŭ���ߴٸ�?
		if (player.DeadCount > TalkCount && isUpGraded == false)
		{
			isUpGraded = true;
			TalkCount++;
		}

		//���� ���� Ƚ������ ȸ���� ��ȭ Ƚ���� ���ٸ�
		if (player.DeadCount > TalkCount && isDeadUp)
		{
			TalkCount++;
			//Debug.Log($"ȸ���� ��ȭ Ƚ�� : {TalkCount}");
			isDeadUp = false;
		}

		//�ٸ���� ��ȭ�ߴٰ� ����� ��ȭ�ߴٰ� ���� ���̴� �� ����
		if(tempId != 0&& tempId != id)
		{
			i = 0;
		}

		//���� ���� Ƚ�� �ӽ� ����
		tempDeadCount = player.DeadCount;

		//�߿�!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//����� ��ȭ ����߿� �ش� ��ȭ ��ȣ�� ��ȭ�� ��������
		if (isOverIndex == false)
		{
			int DialogNum = TalkCount * 10000 + id + i;//��ȭ��ȣ
			i++;
			string[] texts = parsingManager.GetDialogPlz(DialogNum);
			talker = texts[1];
			dialog = texts[0];
		}
		else if(isOverIndex == true)
		{
			i = 0;
			isOverIndex = false;
		}
	}
}
