using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//파씽
using UnityEngine.Networking;

public class ParsingManager : MonoBehaviour
{
	const string URL = "https://docs.google.com/spreadsheets/d/1tEMmdWn9jWmPBLcppi0xkukl86hoj3C0Jo3x9Y8utwU/export?format=tsv&range=I2:J19";
	string SHEET;

	//파씽 스타트
	IEnumerator Start()
	{
		using (UnityWebRequest online_sheet = UnityWebRequest.Get(URL))
		{
			yield return online_sheet.SendWebRequest();

			if(online_sheet.isDone)
			{
				SHEET = online_sheet.downloadHandler.text;
			}
		}
		//테스트 메소드
		Add();
		//AddDialogs();
	}
	//대화번호, (대화주체:대화) 세트인 딕셔너리 생성
	Dictionary<int, string> dialog = new Dictionary<int, string>();
	//테스트 메소드
	void Add()
	{
		string[] sheet = SHEET.Split('\n');

		//string[] hang1 = sheet[0].Split("\t");
		//string[] hang2 = sheet[1].Split("\t");
		//string[] hang3 = sheet[2].Split("\t");
		//Debug.Log($"{hang1[0]} {hang1[1]}");
		//Debug.Log($"{hang2[0]} {hang2[1]}");
		//Debug.Log($"{hang3[0]} {hang3[1]}");

		//1행부터 17행까지
		for(int i = 1; i <= 17; i++)
		{
			string[] hang = sheet[i].Split("\t");
			//Debug.Log($"대화넘버{hang[0]} 대화주체자{hang[1]}");
			int talkNum = int.Parse(hang[0]);
			string dialogAndTalker = hang[1];
			dialog.Add(talkNum, dialogAndTalker);
		}
	}

	public TalkManager talkManager;

	//private void Start()
	//{
	//	AddDialogs();
	//}

	//void AddDialogs()
	//{
	//	//죽음 회차

	//	//1회차
	//	dialog.Add(10000 + 100 + 0, "이봐:진우");
	//	dialog.Add(10000 + 100 + 1, "야옹:고양이");
	//	dialog.Add(10000 + 100 + 2, "너 누구냐:진우");
	//	dialog.Add(10000 + 100 + 3, "야옹야옹:고양이");
	//	dialog.Add(10000 + 100 + 4, "어..:진우");
	//	dialog.Add(10000 + 100 + 5, "그렇구나:진우");

	//	dialog.Add(10000 + 200 + 0, "아저씨는 누구에요?:진우");
	//	dialog.Add(10000 + 200 + 1, "알아서 뭐해:아저씨");

	//	//2회차
	//	dialog.Add(20000 + 100 + 0, "오늘 저녁 뭐먹지?:진우");
	//	dialog.Add(20000 + 100 + 1, "야아아옹:고양이");
	//	dialog.Add(20000 + 100 + 2, "그 그래..:진우");

	//	dialog.Add(20000 + 200 + 0, "어이 자네:아저씨");
	//	dialog.Add(20000 + 200 + 1, "네:진우");
	//	dialog.Add(20000 + 200 + 2, "혹시 전설을 믿나?:아저씨");
	//	dialog.Add(20000 + 200 + 3, "아니요:진우");
	//	dialog.Add(20000 + 200 + 4, "근데 왜 물어봤어?:아저씨");
	//	dialog.Add(20000 + 200 + 5, "?:진우");
	//}

	public string[] GetDialogPlz(int DialogNum)
	{
		if (dialog.ContainsKey(DialogNum))
		{
			string[] parts = dialog[DialogNum].Split(':');
			if (parts.Length == 2)
			{
				string Talker = parts[1];
				string Dialog = parts[0];
			}
			return parts;
		}
		else
		{
			talkManager.i = 0;
			string[] arr = {"...","..." };
			return arr;
		}
	}
}
