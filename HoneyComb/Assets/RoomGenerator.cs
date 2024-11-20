using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
	/* �� ���� */
	public GameObject hpUpDoor;         // ü�� ���� ��
	public GameObject atkUpDoor;        // ���ݷ� ���� ��
	public GameObject emotionRoundDoor; // ���� ���� ȹ�� ��
	public GameObject MoneyUpDoor;      // ��ȭ ȹ�� ��

	/* ���� ������ ��ǥ ��ġ */
	[Header("���� ������ ��ǥ ��ġ")]
	public Transform[] Pos;

	private List<DoorType> doorType;    // �� ���� Ÿ���� ����Ʈ

	// ������ ������ ������ ������ ����Ʈ
	private List<GameObject> generatedDoors = new List<GameObject>();

	void Start()
	{
		//RandomDoorGenerate(); // ���� �� ����
	}

	// key 0�� : �⺻ ���� ��
	// key 1�� : ü�¹�
	// key 2�� : ���ݹ�
	// key 3�� : ��ȭ��
	// key 4�� : ������
	public void RandomDoorGenerate(int key) // ���� �� ����
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
			selectedDoors.Add(doorType[RandomNumber]);            // ������ ���� Ÿ���� ���õ� ����Ʈ�� �߰�
			doorType.RemoveAt(RandomNumber);                    // ����Ʈ�� �߰��� ���� ���� ����Ʈ���� ����
		}

		key = key * 4;

		// ������ ���� ����
		foreach (var door in selectedDoors)
		{
			GameObject createdDoor = null;
			switch (door)
			{
				case DoorType.hpUP:
					createdDoor = Instantiate(hpUpDoor, Pos[key].position, Quaternion.identity);
					break;
				case DoorType.atkUP:
					createdDoor = Instantiate(atkUpDoor, Pos[key+1].position, Quaternion.identity);
					break;
				case DoorType.MoneyUp:
					createdDoor = Instantiate(MoneyUpDoor, Pos[key+2].position, Quaternion.identity);
					break;
				case DoorType.EmotionRound:
					createdDoor = Instantiate(emotionRoundDoor, Pos[key+3].position, Quaternion.identity);
					break;
			}
			if (createdDoor != null)
			{
				generatedDoors.Add(createdDoor); // ������ �� ������ ����Ʈ�� ����
			}
		}
	}

	// �� �ʱ�ȭ (������)
	public void DestroyDoor()
	{
		foreach (GameObject door in generatedDoors)
		{
			Destroy(door); // ������ �� ����
		}
		generatedDoors.Clear(); // ���� ����Ʈ �ʱ�ȭ
	}
}
