using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
	//�÷��̾��� ��ǥ ���� �ε�?
	public Transform player;
	private Player playerScript;
	public GameObject Dev_Btn;

	//��ǥ ���� �ε�?
	public Transform[] Pos;
	//Pos[0] �� ��ǥ
	//Pos[1] room1 ��ǥ
	//Pos[2] room2 ��ǥ
	//Pos[3] room3 ��ǥ...

	private void Start()
	{
		playerScript = FindObjectOfType<Player>();
	}
	private void Update()
	{
		if (Dev_Btn.activeSelf)
		{
			//Debug.Log("Dev_Btn�� Ȱ��ȭ�� �����Դϴ�.");

			if (Input.GetKeyDown(KeyCode.H))
			{
				//�÷��̾� ��ǥ�� -> �� ��ǥ
				player.position = Pos[0].position;
			}

			if (Input.GetKeyDown(KeyCode.K))
			{
				//��õ�ϼ̽��ϴ�.
				playerScript.Dead();
			}
		}
		else
		{
			//Debug.Log("Dev_Btn�� ��Ȱ��ȭ�� �����Դϴ�.");
		}


		////����Ű = ���ȣ
		//if (Input.GetKeyDown(KeyCode.Alpha1))
		//	player.position = Pos[1].position;

		//if (Input.GetKeyDown(KeyCode.Alpha2))
		//	player.position = Pos[2].position;

		//if (Input.GetKeyDown(KeyCode.Alpha3))
		//	player.position = Pos[3].position;

		//if (Input.GetKeyDown(KeyCode.Alpha4))
		//	player.position = Pos[4].position;

		//if (Input.GetKeyDown(KeyCode.Alpha5))
		//	player.position = Pos[5].position;

		//if (Input.GetKeyDown(KeyCode.Alpha6))
		//	player.position = Pos[6].position;
	}
}
