using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    //�̵��� ��ġ�� public���� �����Ͽ� Unity �����Ϳ��� ������ �� �ְ� ��
    public Transform destination;// �÷��̾ ������ ���� ������Ʈ ��ǥ

    //Ʈ���ſ� �浹���� �� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter2D(Collider2D other)//���� ����ü�� �Ű������� �ҷ���
    {
        //�÷��̾ �浹�ߴ��� Ȯ�� (�±׷� �÷��̾ ����)
        if (other.CompareTag("Player"))//���� ����ü�� �±װ� Player�� ���
        {
            other.transform.position = destination.position;
            //���� ����ü�� �÷��̾� ��ǥ�� ������ ���� ������Ʈ ��ǥ�� �̵�
        }
    }
}
/*

*/