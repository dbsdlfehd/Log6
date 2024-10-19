using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
	[Header("얘는 안갖다 붙여도 됨")]
	public GameObject ScanObject;

	[Header("플레이어 스크립트")]
	public Player player;

	[Header("파씽매니저")]
	public ParsingManager parsingManager;

	[Header("대화창")]
	public TextMeshProUGUI Talker;
	public TextMeshProUGUI Dialog;

	[Header("회차별 대화 횟수")]
	public TextMeshProUGUI TalkCountNum;

	string talker = "";
	string dialog = "";
	public int tempId = 0;
	public int tempDeadCount = 0;
	public int TalkCount = 0;
	bool isDeadUp = false;
	bool isUpGraded = false;

	public bool isOverIndex = false;
	//대화 액션 함수
	public void DialogAction(GameObject scannedObject)
    {
		//스캔된 오브젝트
		ScanObject = scannedObject;
		
		//스캔된 오브젝트를 컴포넌트에 가져옵니다.
		_Object obj = scannedObject.GetComponent<_Object>();
		
		//스캔된 오브젝트의 아이디와 NPC여부를 확인합니다.
		//Debug.Log(obj.id + " " + obj.isNPC);

		//스캔된 오브젝트의 아이디와 NPC여부를 Talk함수에 보냅니다.
		Talk(obj.id, obj.isNPC);
		tempId = obj.id;
	}

	private void Update()
	{
		//UI에 나타내기
		Talker.text = talker.ToString();
		Dialog.text = dialog.ToString();
		TalkCountNum.text = "회차별 대화 횟수 : " + TalkCount.ToString();
	}

	public int i = 0;
	void Talk(int id, bool isNPC)
	{
		//죽음 횟수가 바뀌었다?
		if (tempDeadCount != 0 && tempDeadCount != player.DeadCount)
		{
			i = 0;
			isDeadUp = true;//
		}

		//만일 대화를 클릭했다면?
		if (player.DeadCount > TalkCount && isUpGraded == false)
		{
			isUpGraded = true;
			TalkCount++;
		}

		//만약 죽음 횟수보다 회차별 대화 횟수가 적다면
		if (player.DeadCount > TalkCount && isDeadUp)
		{
			TalkCount++;
			//Debug.Log($"회차별 대화 횟수 : {TalkCount}");
			isDeadUp = false;
		}

		//다른사람 대화했다가 저사람 대화했다가 순번 꼬이는 일 방지
		if(tempId != 0&& tempId != id)
		{
			i = 0;
		}

		//현재 죽음 횟수 임시 저장
		tempDeadCount = player.DeadCount;

		//중요!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//저장된 대화 목록중에 해당 대화 번호의 대화말 가져오기
		if (isOverIndex == false)
		{
			int DialogNum = TalkCount * 10000 + id + i;//대화번호
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
