using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ľ�
using UnityEngine.Networking;

public class ParsingManager : MonoBehaviour
{
	public GameObject DialogSet; // ��ȭâ

	const string URL = "https://docs.google.com/spreadsheets/d/1tEMmdWn9jWmPBLcppi0xkukl86hoj3C0Jo3x9Y8utwU/export?format=tsv&range=I2:J1000";
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


	//������ ������ ��ųʸ� ������ �߰�
	void Add()
	{
		//������ �ִ� ���� �����(�ٰ���)("\n")���� �и�
		string[] sheet = SHEET.Split('\n');

		//�� ����
		int rowCount = Mathf.Min(1000, sheet.Length - 1);

		//1���� �����ϴ� ����: ���� ������ �����Ѵ�~
		for (int i = 1; i <= rowCount; i++)
		{
			//�� ���� �ִ� ��츦 ó�� �ǳʶٱ�
			if (string.IsNullOrWhiteSpace(sheet[i]))
			{
				continue;
			}

			//������ �ִ� ���� ��(��)("\t")���� �и�
			string[] hang = sheet[i].Split('\t');

			//��ȭ �ѹ��� ��ȭ ��ü�ڰ� ��� �־�� �۵���
			if (hang.Length < 2)
			{
				Debug.LogError($"�ùٸ��� ���� ������: {sheet[i]}");
				continue;
			}

			//��ȭ��ȣ 
			int talkNum;

			//��ȭ��ȣ�� �������� �Ǻ�
			if (int.TryParse(hang[0], out talkNum))
			{
				//��ȭ:��ü��
				string dialogAndTalker = hang[1];

				//�ߺ� Ű ����
				if (!dialog.ContainsKey(talkNum))
				{
					dialog.Add(talkNum, dialogAndTalker);
				}
				else
				{
					Debug.LogError($"�ߺ��� Ű: {talkNum}");
				}
			}
			else
			{
				//���н� �˸���
				Debug.LogError($"��ȭ �ѹ� ��ȯ ����: {hang[0]}");
			}
		}
		//// ���� ���Խ� ���� ��Ȳ �߰�
		//dialog.Add(40000, "���� ���� ����߿�~ ���� ���� �� ������!:����");
		//dialog.Add(40001, "������ �غ���:����");
		//dialog.Add(40002, "��..:����");
		//dialog.Add(40003, "�� �Ҽ� �ִ�!:����");
		//dialog.Add(40004, "�� �����غ���..:����");
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

	public string[] GetDialogPlz(int DialogNum,_Object obj)
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
			//Debug.Log("END");
			//talkManager.isTalking = false;
			talkManager.StopDialogSet(obj);
			talkManager.i = 0;
			string[] arr = {"...","..." };
			return arr;
		}
	}

	//����
	public string[] GetSoloDialogPlz(int DialogNum)
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
			string[] arr = { "...", "..." };
			return arr;
		}
	}
}
