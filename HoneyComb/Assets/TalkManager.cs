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

	string talker = "";
	string dialog = "";
	public int tempId = 0;
	public int tempDeadCount = 0;

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
		Talker.text = talker.ToString();
		Dialog.text = dialog.ToString();
	}

	public int i = 0;
	void Talk(int id, bool isNPC)
	{
		//죽음 횟수가 바뀌었다?
		if(tempDeadCount != 0 && tempDeadCount != player.DeadCount)
		{
			i = 0;
		}

		//다른사람 대화했다가 저사람 대화했다가 순번 꼬이는 일 방지
		if(tempId != 0&& tempId != id)
		{
			i = 0;
		}

		tempDeadCount = player.DeadCount;
		if (isOverIndex == false)
		{
			int DialogNum = player.DeadCount * 10000 + id + i;//대화번호
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
