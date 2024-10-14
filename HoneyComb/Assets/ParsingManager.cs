using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ľ�
using UnityEngine.Networking;

public class ParsingManager : MonoBehaviour
{
	const string URL = "https://docs.google.com/spreadsheets/d/1tEMmdWn9jWmPBLcppi0xkukl86hoj3C0Jo3x9Y8utwU/export?format=tsv&range=I2:J19";
	string SHEET;

	//�ľ� ��ŸƮ
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
		//�׽�Ʈ �޼ҵ�
		Add();
		//AddDialogs();
	}
	//��ȭ��ȣ, (��ȭ��ü:��ȭ) ��Ʈ�� ��ųʸ� ����
	Dictionary<int, string> dialog = new Dictionary<int, string>();
	//�׽�Ʈ �޼ҵ�
	void Add()
	{
		string[] sheet = SHEET.Split('\n');

		//string[] hang1 = sheet[0].Split("\t");
		//string[] hang2 = sheet[1].Split("\t");
		//string[] hang3 = sheet[2].Split("\t");
		//Debug.Log($"{hang1[0]} {hang1[1]}");
		//Debug.Log($"{hang2[0]} {hang2[1]}");
		//Debug.Log($"{hang3[0]} {hang3[1]}");

		//1����� 17�����
		for(int i = 1; i <= 17; i++)
		{
			string[] hang = sheet[i].Split("\t");
			//Debug.Log($"��ȭ�ѹ�{hang[0]} ��ȭ��ü��{hang[1]}");
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
	//	//���� ȸ��

	//	//1ȸ��
	//	dialog.Add(10000 + 100 + 0, "�̺�:����");
	//	dialog.Add(10000 + 100 + 1, "�߿�:�����");
	//	dialog.Add(10000 + 100 + 2, "�� ������:����");
	//	dialog.Add(10000 + 100 + 3, "�߿˾߿�:�����");
	//	dialog.Add(10000 + 100 + 4, "��..:����");
	//	dialog.Add(10000 + 100 + 5, "�׷�����:����");

	//	dialog.Add(10000 + 200 + 0, "�������� ��������?:����");
	//	dialog.Add(10000 + 200 + 1, "�˾Ƽ� ����:������");

	//	//2ȸ��
	//	dialog.Add(20000 + 100 + 0, "���� ���� ������?:����");
	//	dialog.Add(20000 + 100 + 1, "�߾ƾƿ�:�����");
	//	dialog.Add(20000 + 100 + 2, "�� �׷�..:����");

	//	dialog.Add(20000 + 200 + 0, "���� �ڳ�:������");
	//	dialog.Add(20000 + 200 + 1, "��:����");
	//	dialog.Add(20000 + 200 + 2, "Ȥ�� ������ �ϳ�?:������");
	//	dialog.Add(20000 + 200 + 3, "�ƴϿ�:����");
	//	dialog.Add(20000 + 200 + 4, "�ٵ� �� ����þ�?:������");
	//	dialog.Add(20000 + 200 + 5, "?:����");
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
