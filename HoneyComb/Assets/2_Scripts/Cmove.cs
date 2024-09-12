using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cmove : MonoBehaviour
{
    /*�÷��̾� ��ǥ���� ī�޶� ��ǥ�� ���� �� �� ����ŭ�� ���� ���󰡴� �ڵ�*/

    //ī�޶� �ӵ� ������ ����
    public float cameraSpeed = 5.0f;

    //�ܺ� ���̾ƶ�Ű â�� �÷��̾� �����ͼ� player ������ ���
    public GameObject player;

    private void Update()
    {
        //dir = �÷��̾� ��ǥ - ī�޶� ��ǥ
        Vector3 dir = player.transform.position - this.transform.position;

        //moveVector = dir�� �Էµ� x��ǥ, y��ǥ�� �ӵ��� �ð��� ���ؼ� �Ѱ��ش�.
        Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);

        //�� ��ũ��Ʈ�� �޸� ��ü�� moveVector���� ���� ��ȯ�Ѵ�.
        this.transform.Translate(moveVector);
    }
}