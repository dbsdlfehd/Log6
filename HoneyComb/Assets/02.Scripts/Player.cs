using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    //Animator anim;
    PlayerHpShow PlayerHpShow;


    [Header("宋精 判呪")]
    public int DeadCount = 0;
	public TextMeshProUGUI DeadCount_UI;

	//巴傾戚嬢 是帖
	public Transform player;

    //増 是帖
	public Transform Home;

    //走秦 是帖
    public Transform DeadPoint;

	public bool isDead = false;
    public GameObject Dead_set;//宋製 尻窒
    //public void PlayAnimation(int atkNum)
    //{
    //    anim.SetFloat("Blend", atkNum);
    //    anim.SetTrigger("Atk");

    //}

    [Header("端径")]
    public float maxHP;
    public float nowHP;
	public TextMeshProUGUI HP_UI;

	[Header("因維")]
	public int Atk;
    //public Slider slider;
	public TextMeshProUGUI Atk_UI;

	[Header("紗亀")]
    public int speed;
    public float minPos;//戚惟 更希虞?
    public float maxPos;//設 乞牽畏製 焼獣澗 歳 爽汐岨 せせ
    public RectTransform pass;//剰亀 せせせせせ
    public int atkNum;//爪左? 腰硲析牛

	private void Start()
	{
		//anim = GetComponent<Animator>();
		nowHP = maxHP;
		PlayerHpShow = GetComponent<PlayerHpShow>();
	}

	//public void SetAtk()
 //   {
 //       //slider.value = 0;
 //       minPos = pass.anchoredPosition.x;
 //       maxPos = pass.sizeDelta.x + minPos;
 //       StartCoroutine(ComboAtk());
 //   }

    IEnumerator ComboAtk()
    {
        yield return null;
        //while (!(Input.GetKeyDown(KeyCode.Space) || slider.value == slider.maxValue))
        //{
        //    slider.value += Time.deltaTime * speed;
        //    yield return null;
        //}
        //if (slider.value >= minPos && slider.value <= maxPos)
        //{
        //    //PlayAnimation(atkNum++);
        //    if (atkNum < 4)  //  因維乞芝戚 4鯵戚奄凶庚
        //        SetAtk();
        //    else
        //    {
        //        atkNum = 0;
        //        isAtk = false;
        //    }
        //}
        //else
        //{
        //    //PlayAnimation(0);
        //    isAtk = false;
        //    atkNum = 0;
        //}
        //slider.value = 0;
    }
    //bool isAtk = false;


    //穣汽戚闘 五社球
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !isAtk)
        //{
        //    isAtk = true;
        //    //SetAtk();
        //}

        //幻鉦 宋精 雌殿拭辞 什凪戚什郊研 刊献陥檎?
        if(isDead == true && Input.GetKeyDown(KeyCode.Space)) 
        {
			//巴傾戚嬢 左戚惟 馬奄
			//gameObject.SetActive(true);

			//端径 幻競
			nowHP = maxHP;

            //照宋精 雌殿稽 郊嘩
            isDead = false;

			//増 是帖稽 亜奄
			player.position = Home.position;

			//宋製 尻窒 左戚澗 暗 蒸蕉奄
			Dead_set.SetActive(false);
		}


		//宋精 雌殿析 凶澗 井送
		if (isDead == true)
			player.position = DeadPoint.position;

        //UI
		//端径
		HP_UI.text = "HP : " + nowHP.ToString();

        //因維径
		Atk_UI.text = Atk.ToString();

        //宋精 判呪
		DeadCount_UI.text = "宋精 判呪 : " + DeadCount.ToString();
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

		nowHP = nowHP - collision.GetComponent<FarATK>().damage;

        //薄仙 端径戚 0左陥 拙暗蟹 旭生檎
        if (nowHP < 0)
        {
            //宋製 敗呪 叔楳
			Dead();
		}
    }

    //宋製 敗呪
    public void Dead()
    {
        //宋精 判呪 蓄亜馬奄
        DeadCount++;

		isDead = true;
        Dead_set.SetActive(true);

		//走秦 亜奄
		player.position = DeadPoint.position;

		//gameObject.SetActive(false);//巴傾戚嬢 神崎詮闘 照左戚奄 坦軒
	}
}
