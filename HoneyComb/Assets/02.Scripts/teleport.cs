using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    //�̵��� ��ġ�� public���� �����Ͽ� Unity �����Ϳ��� ������ �� �ְ� ��
    [Header("��ǥ")]
	public Transform [] Pos;// �÷��̾ ������ ���� ������Ʈ ��ǥ

	//Ʈ���ſ� �浹���� �� ȣ��Ǵ� �Լ�
	private void OnTriggerEnter2D(Collider2D other)//���� ����ü�� �Ű������� �ҷ���
    {
        if (Pos.Length>=2)
        {
			//�÷��̾ �浹�ߴ��� Ȯ�� (�±׷� �÷��̾ ����)
			if (other.CompareTag("Player"))//���� ����ü�� �±װ� Player�� ���
			{
				int RANDOM_NUMBER = Random.Range(0, 2);

				Debug.Log(RANDOM_NUMBER + "�� ������ ��ǲ���~");

				other.transform.position = Pos[RANDOM_NUMBER].position;
				//���� ����ü�� �÷��̾� ��ǥ�� ������ ���� ������Ʈ ��ǥ�� �̵�
			}
		}
		else
		{
			other.transform.position = Pos[0].position;
		}
        
    }
}