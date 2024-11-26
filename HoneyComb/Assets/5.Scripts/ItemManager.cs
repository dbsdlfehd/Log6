using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ScriptableObjectTest;

// ������ ������ �����ϴ� ���̺�
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/ItemDatabase")]
public class ItemTable : ScriptableObject
{
	// ������ ������ ����ü
	[System.Serializable]
	public class ItemData
	{
		public string Name;	  // ������ �̸�
		public int num1;      // ����1 
		public float num2;    // ����2
		public float num3;	  // ����3

		public ItemData(string name, int Num1, float Num2, float Num3)
		{
			Name = name;
			num1 = Num1;
			num2 = Num2;
			num3 = Num3;
		}
	}

	// ������ ���
	static public Dictionary<int, ItemData> itemDictionary;

	static ItemTable()
	{
		itemDictionary = new Dictionary<int, ItemData>();

		// ������ ������ ���
		itemDictionary.Add(1, new ItemData("��� ��ġ", 250, 0, 0));
		itemDictionary.Add(2, new ItemData("��� ���", 1, 0, 0)); // ���� ������
		itemDictionary.Add(3, new ItemData("������ ��", -1, 0.1f, 0.02f)); // ���� ����, �̼� ����, ���� ����
	}

	// ������ ������ ��������
	//public ItemData GetItemData(int itemID)
	//{
	//	if (itemDictionary.ContainsKey(itemID))
	//	{
	//		return itemDictionary[itemID];
	//	}
	//	Debug.LogWarning($"Item ID {itemID} not found in ItemTable.");
	//	return null;
	//}
}

public class ItemManager : MonoBehaviour
{
	public ScriptableObjectTest[] itemData;  // ScriptableObjectTest ���� �ʵ�

	public Player player;               // �÷��̾� ��ũ��Ʈ
    public PlayerAction playerAction;   // �÷��̾� �׼� ��ũ��Ʈ

    public bool isHammerBuffing;        // �ظ� ���ݷ� ��� ���� ���� ����
	public bool isHammerAtkUsed;        // 1ȸ �ظ� ���� ���� ��?

	public bool isCrunchMode;           // ���� ������ �� ���ΰ�?
	public GameObject CrunchEye;		// ������ �� ���� �̹��� 

	public bool isNextRoundHpUp = false;// HpUpCheck
	public GameObject HpUPSet;

	public GameObject HammerBuffSet;	// ��Ӹ�ġ ���� ǥ�ÿ� �̹���
	public TextMeshProUGUI HammerCount; // ���� ���� Ƚ��

	public void Signal(string itemText, int itemID) // ���� ������ ��ȣ �޾ƿ´�.
    {
        switch (itemID)
        {
            case 1:
                ItemPrint(itemText, itemID);
                Hammer();
                break;
            case 2:
                ItemPrint(itemText, itemID);
				NextRoundHpUp();
				break;
            case 3:
                ItemPrint(itemText, itemID);
				CrunchModeEyeStarter();
				CrunchModeEye();
				break;

        }
    }

    private void ItemPrint(string itemText, int itemID) // �׽�Ʈ
    {
		//Debug.Log(itemID + "�� ������ :" + itemText + "�� ����");
	}
	
    private void Hammer()// ��� ��ġ ������ �Լ�
	{
        player.HammerBuffedRoundCount = 3;		// 3 ���常 ���ؼ� ���� �ο� (���� �̵��� 1 �Ҹ�)
		isHammerBuffing = true;
		HammerBuffSet.SetActive(true);          // ���� ��Ʈ ���
		HammerCount.text = player.HammerBuffedRoundCount.ToString(); // ���� Ƚ�� ���
	}

	public void HammerBuff()// �ظ� ���ݷ� ��� (��, ������ �϶� && 1ȸ�� ���ؼ�)
	{
		if (isHammerBuffing == true && isHammerAtkUsed == false)
		{
			player.Atk += itemData[0].Num1;
		}
	}

	public void HammerDeBuff()// �ظ� ���ݷ� �϶� (��, ������ �϶� && 1ȸ�� ���ؼ�)
	{
		if (isHammerBuffing == true && isHammerAtkUsed == false)
		{
			player.Atk -= itemData[0].Num1;
			isHammerAtkUsed = true; // 1ȸ �ظ� ���� ��±� �����

			if (player.HammerBuffedRoundCount == 0) // �ظӹ��� ���� Ƚ���� 0 �Ͻ�
			{
				GoodByeHammerBuff();    // �ظӹ��� �߰�
			}
		}
	}

	public void RoundUp()
	{
		if(isHammerBuffing == true) // ���� ��ȸ 1�Ҹ� (��, ������ �϶�)
		{
			player.HammerBuffedRoundCount--;
			isHammerAtkUsed = false; // 1ȸ �ظ� ���� ��±� ����
			HammerCount.text = player.HammerBuffedRoundCount.ToString(); // ���� Ƚ�� ���
		}

		if (player.HammerBuffedRoundCount < 0)
		{
			GoodByeHammerBuff();	// �ظӹ��� �߰�
		}
	}

	public void GoodByeHammerBuff()
	{
		isHammerBuffing = false;                 // ���� ���� �ʰ� �ع������Ƿ� (��ü���� �ظ�)������ ��Ż�մϴ�.
		HammerBuffSet.SetActive(false);          // ���� ��Ʈ �ٽ� �Ⱥ��̰�
	} // �ظ� ���� ���� �Լ�

	public void NextRoundHpUp() // ��� ��� �Լ�
	{
		isNextRoundHpUp = true;
		HpUPSet.SetActive(true);
	}

	public void DebuffHpUp() // ��� ��� ���� ���� �Լ�
	{
		HpUPSet.SetActive(false);
	}

	public void HpUP() // ��� ��� ���� ������ ����
	{
		if(player.nowHP == player.maxHP)
		{

		}
		else if(player.nowHP <= player.maxHP)
		{
			player.nowHP += itemData[1].Num1;// hp �÷���
		}
	}

	public void CrunchModeEyeStarter()
	{
		isCrunchMode = true;
		CrunchEye.SetActive(true);
	}

	public void OFFCrunchMode()
	{
		isCrunchMode = false;
		CrunchEye.SetActive(false);
	}

	public void CrunchModeEye() // ������ ��
	{
		// ���� ���� (��Ȱ�� �ʱ�ȭ �ؾߵɵ�)
		playerAction.jabCooldown -= itemData[2].Num3;

		// �̼� ���� (��Ȱ�� �ʱ�ȭ �ؾߵɵ�)
		playerAction.walkSpeed += itemData[2].Num2;

		// ���� ���� (��Ȱ�� �ʱ�ȭ �ؾߵɵ�)
		player.AR += itemData[2].Num1;
	}
}
