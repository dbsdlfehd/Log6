using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;//TextMeshPro�� namespace
using UnityEngine;

public class TalkManager : MonoBehaviour
{
	/*TalkManger�� ��ȭ ���õ� ���� ó���ϴ� ��ũ��Ʈ�Դϴ�.*/

	[Header("NPC ��ĵ")]
	public GameObject ScanObject;

	[Header("Player")]
	public Player player;

	[Header("�ľ�")]
	public ParsingManager parsingManager;

	[Header("DialogSet")]
	public TextMeshProUGUI Talker;
	public TextMeshProUGUI Dialog;
	string talker = "";
	string dialog = "";

	[Header("�׽�Ʈ")]
	public TextMeshProUGUI TalkCountNum;
	string text = "";
	string NPCname;//�׽�Ʈ�� NPC �̸� ����

	int tempId = 0;//�ӽ� NPC �ֹε�Ϲ�ȣ
	//int tempDeadCount = 0;//�ӽ� Player���� Ƚ��

	public bool isOverIndex = false;//��ȭ �帧�� EX. 10100~10107

	public void DialogAction(GameObject scannedObject)//���찡 �ѹ� ��ĵ�� �۵�
	{
		ScanObject = scannedObject;//���찡 ��ĵ�� NPC

		_Object obj = scannedObject.GetComponent<_Object>();//NPC ����

		Talk(obj.id, scannedObject);
		tempId = obj.id;
		NPCname = obj.name;
		text = NPCname + "�� ȸ���� ��ȭ Ƚ�� : " + obj.EachTalkCount.ToString();//test
	}

	private void Update()
	{
		Talker.text = talker.ToString();//DialogSet
		Dialog.text = dialog.ToString();//DialogSet
		TalkCountNum.text = text;//test
	}

	public int i = 0;
	void Talk(int id, GameObject scannedObject)
	{
		_Object obj = scannedObject.GetComponent<_Object>();

		if(tempId != id)
		{
			Debug.Log("B ���� 1 �þ");
		}

		if (obj.tempPlayerDead != player.DeadCount)
		{
			i = 0;
			obj.isDeadUp = true;
		}

		if (player.DeadCount > obj.EachTalkCount && obj.isDeadUp == true)
		{
			obj.EachTalkCount++;
			obj.isDeadUp = false;
		}

		if (tempId != 0 && tempId != id)//�ٸ���� ��ȭ�ߴٰ� ����� ��ȭ�ߴٰ� ���� ���̴� �� ����
		{
			i = 0;
		}

		obj.tempPlayerDead = player.DeadCount;//���� ���� Ƚ�� �ӽ� ����

		if (isOverIndex == false)//��ȭ ���� index�� ���� ���� ��
		{
			int DialogNum = obj.EachTalkCount * 10000 + id + i;//��ȭ��ȣ
			Debug.Log(DialogNum);//�׽�Ʈ
			i++;
			string[] texts = parsingManager.GetDialogPlz(DialogNum);
			talker = texts[1];
			dialog = texts[0];
		}
		else if (isOverIndex == true)//��ȭ ���� index�� ���� ��
		{
			i = 0;
			isOverIndex = false;
		}
	}
}
