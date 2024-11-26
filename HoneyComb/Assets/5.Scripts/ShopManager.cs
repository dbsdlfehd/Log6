using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    Player player;
    ItemManager itemManager;    // ������ �Ŵ���
    public int ItemID;          // ������ ��ȣ
    public int ItemMoney;       // ��ȭ
    public Text ItemMoneyTxt;
    public Text ItemNameTxt;

    private void Awake()
    {
        player = FindObjectOfType<Player>(); // ������ ����ߵ� (�ʱ�ȭ)
		itemManager = FindAnyObjectByType<ItemManager>();
	}

    private void Update()
    {
        ItemMoneyTxt.text = ItemMoney.ToString();
    }

    public void Buy()
    {
        if (Player.Money >= ItemMoney)
        {
            Player.Money = Player.Money - ItemMoney;
            GetComponent<Button>().interactable = false;
			string ItemNametext = ItemNameTxt.text;

            itemManager.Signal(ItemNametext, ItemID);

            // Debug.Log($"{ItemNametext}�� �����ϼ̽��ϴ�.");
        }

		else if (Player.Money < ItemMoney)
        {
            Debug.Log("�� ����.");
        }
    }
}
