using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;//TextMeshPro용 namespace
using UnityEngine;

public class TalkManager : MonoBehaviour
{
	/*TalkManger란 대화 관련된 것을 처리하는 스크립트입니다.*/

	[Header("NPC 스캔")]
	public GameObject ScanObject;

	[Header("Player")]
	public Player player;

	[Header("파씽")]
	public ParsingManager parsingManager;

	[Header("DialogSet")]
	public TextMeshProUGUI Talker;
	public TextMeshProUGUI Dialog;
	string talker = "";
	string dialog = "";

	[Header("테스트")]
	public TextMeshProUGUI TalkCountNum;
	string text = "";
	string NPCname;//테스트용 NPC 이름 변수

	int tempId = 0;//임시 NPC 주민등록번호
	//int tempDeadCount = 0;//임시 Player죽음 횟수

	public bool isOverIndex = false;//대화 흐름수 EX. 10100~10107

	public void DialogAction(GameObject scannedObject)//진우가 한번 스캔시 작동
	{
		ScanObject = scannedObject;//진우가 스캔한 NPC

		_Object obj = scannedObject.GetComponent<_Object>();//NPC 정보

		Talk(obj.id, scannedObject);
		tempId = obj.id;
		NPCname = obj.name;
		text = NPCname + "의 회차별 대화 횟수 : " + obj.EachTalkCount.ToString();//test
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
			Debug.Log("B 나는 1 늘어나");
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

		if (tempId != 0 && tempId != id)//다른사람 대화했다가 저사람 대화했다가 순번 꼬이는 일 방지
		{
			i = 0;
		}

		obj.tempPlayerDead = player.DeadCount;//현재 죽음 횟수 임시 저장

		if (isOverIndex == false)//대화 순번 index를 넘지 않을 때
		{
			int DialogNum = obj.EachTalkCount * 10000 + id + i;//대화번호
			Debug.Log(DialogNum);//테스트
			i++;
			string[] texts = parsingManager.GetDialogPlz(DialogNum);
			talker = texts[1];
			dialog = texts[0];
		}
		else if (isOverIndex == true)//대화 순번 index를 넘을 때
		{
			i = 0;
			isOverIndex = false;
		}
	}
}
