using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home_teleport : MonoBehaviour
{
	//�̵��� ��ġ�� public���� �����Ͽ� Unity �����Ϳ��� ������ �� �ְ� ��
	public Transform Home;// �÷��̾ ������ ���� ������Ʈ ��ǥ

	//Ʈ���ſ� �浹���� �� ȣ��Ǵ� �Լ�
	private void OnTriggerEnter2D(Collider2D other)//���� ����ü�� �Ű������� �ҷ���
	{
		//�÷��̾ �浹�ߴ��� Ȯ�� (�±׷� �÷��̾ ����)
		if (other.CompareTag("Player"))//���� ����ü�� �±װ� Player�� ���
		{
			Debug.Log("������ ��ǲ���~");

			other.transform.position = Home.position;
			//���� ����ü�� �÷��̾� ��ǥ�� ������ ���� ������Ʈ ��ǥ�� �̵�
		}
	}
}
