using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    Player player;
    public int ItemMoney; // ¿Á»≠
    public Text ItemMoneyTxt;

    private void Awake()
    {
        player = FindObjectOfType<Player>(); // π´¡∂∞« «ÿ¡‡æﬂµ  (√ ±‚»≠)
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
        }
        else if (Player.Money < ItemMoney)
        {
            Debug.Log("µ∑ æ¯¥Ÿ.");
        }
    }
}
