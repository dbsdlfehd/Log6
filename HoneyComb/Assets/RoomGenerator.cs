using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
	/* �� ���� */
	//public GameObject hpUpDoor;			// ü�� ���� ��
	//public GameObject atkUpDoor;		// ���ݷ� ���� ��
	//public GameObject emotionRoundDoor; // ���� ���� ȹ�� ��
	//public GameObject MoneyUpDoor;		// ��ȭ ȹ�� ��

	/* ���� ������ ��ǥ ��ġ */
	public Transform[] Pos;

	private List<DoorType> doorType;    // �� ���� Ÿ���� ����Ʈ

	void Start()
	{
		RandomDoorGenerate(); // ���� �� ����
	}
	void RandomDoorGenerate() // ���� �� ����
	{
		// �� Ÿ�Ե��� ����Ʈ�� �߰�
		doorType = new List<DoorType> { DoorType.hpUP, DoorType.atkUP, DoorType.MoneyUp, DoorType.EmotionRound };

		// �� 2���� �������� �̱��� �� 2�� ������ ���� ����Ʈ
		List<DoorType> selectedDoors = new List<DoorType>();

        // �� 2���� �������� �̱�
        for (int i = 0; i < 2; i++)
        {
			// ���� ���� ���� ���� ���� �߿��� �ϳ��� ����
			int RandomNumber = Random.Range(0, doorType.Count); // 0 ~ (������ Ÿ�� ����) ���� ���� ���� ����
			selectedDoors.Add(doorType[RandomNumber]);			// ������ ���� Ÿ���� ���õ� ����Ʈ�� �߰�
			doorType.RemoveAt(RandomNumber);					// ����Ʈ�� �߰��� ���� ���� ����Ʈ���� ����
		}

		// ������ ���� ����
   //     foreach (var door in selectedDoors)
   //     {
   //         switch (door)
			//{
			//	case DoorType.hpUP:
			//		Instantiate(hpUpDoor, Pos[0].position, Quaternion.identity);
			//		break;
			//	case DoorType.atkUP:
			//		Instantiate(atkUpDoor, Pos[1].position, Quaternion.identity);
			//		break;
			//	case DoorType.MoneyUp:
			//		Instantiate(MoneyUpDoor, Pos[2].position, Quaternion.identity);
			//		break;
			//	case DoorType.EmotionRound:
			//		Instantiate(emotionRoundDoor, Pos[3].position, Quaternion.identity);
			//		break;
			//}
   //     }
    }
}
