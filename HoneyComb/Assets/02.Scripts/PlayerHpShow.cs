using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpShow : MonoBehaviour
{
    public GameObject prefabHP_bar;//ü�¹�(������) ����ġ��
    public GameObject canvas;//canvas����ġ��
	public Player player;//�÷��̾�

	//RectTransform hpBar;//hp�� ��ȯ�ϱ� ���� ��¼��
	public RectTransform hpBar;//hp�� ��ȯ�ϱ� ���� ��¼��

	public Image nowHpBar;//���� ü�¹�

    public float height = 1.7f;//��

	void Start()
	{
		//player = FindObjectOfType<Player>();//������ ����ߵ� (�ʱ�ȭ)
		//hpBar = Instantiate(prefabHP_bar, canvas.transform).GetComponent<RectTransform>();//ü�¹� canvas���ٰ� ������ ��?
		//nowHpBar = hpBar.transform.GetChild(0).GetComponent<Image>();//ü�¹� ���� �κи� �������� ���ϵ�?
	}

	void Update()
	{
		//if(hpBar != null)
		//{
		//	Vector3 hpBarPos =
		//	Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
		//	hpBar.position = hpBarPos;
		//	nowHpBar.fillAmount = (float)player.nowHP / (float)player.maxHP;

		//	if (player.nowHP == 0)
		//	{
		//		//Destroy(hpBar.gameObject);
		//	}
		//}

		nowHpBar.fillAmount = (float)player.nowHP / (float)player.maxHP;
	}
}
