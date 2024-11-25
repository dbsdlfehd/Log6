using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BtnManager : MonoBehaviour
{
	//����
	public GameObject tip;
    public GameObject shop;

    //������
    public GameObject Dev;

	//â
	public TextMeshProUGUI TipText;

	private bool isOpened = false;
	public string text = "����Ʈ";

	void Start()
	{
		// �Ҵ� Ȯ���� ���� ����� �޽���
		if (tip == null)
		{
			Debug.LogError("Tip ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
		}
		if (Dev == null)
		{
			Debug.LogError("Dev ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
		}
		if (TipText == null)
		{
			Debug.LogError("TipText ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
		}
	}

	public void OnAndOFF()
	{
		if (tip != null) // Null üũ
		{
			if (isOpened == false)
			{
				tip.SetActive(true);
				isOpened = true;
			}
			else if (isOpened == true)
			{
				tip.SetActive(false);
				isOpened = false;
			}
		}
		else
		{
			Debug.LogError("Tip ������Ʈ�� null �����Դϴ�.");
		}
	}

	public void OFF()
	{
		tip.SetActive(false);
        shop.SetActive(false);
        isOpened = false;
	}

	public void Developement()
	{
		if (TipText != null) // Null üũ
		{
			string text = "��Ű - H\n�ڰ� - K";
			TipText.text = text;
		}
		else
		{
			Debug.LogError("TipText ������Ʈ�� null �����Դϴ�.");
		}
	}

	public void ControlKeyTips()
	{
		if (TipText != null) // Null üũ
		{
			string text = "�̵� ���� - ȭ��ǥ ����Ű\r\n�⺻ ���� - ��Ŭ��\r\n��ų ���� - ��Ŭ��\r\n�ñر� - R\r\n�����̵� - ����Ʈ\r\n��ȭ - E";
			TipText.text = text;
		}
		else
		{
			Debug.LogError("TipText ������Ʈ�� null �����Դϴ�.");
		}
	}
}
