using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextShow : MonoBehaviour
{
	[Header("���� �� ���� �ٿ��� ��")]
	public TextMeshProUGUI damage_text;

	[Header("������ �ؽ�Ʈ ������")]
	public GameObject damage_text_prf;

	public void ShowDamage(int damage)
	{
		StartCoroutine(ShowDamageText(damage));
	}

	// ����� �ؽ�Ʈ ���� �� 1�� �� ����
	IEnumerator ShowDamageText(int damage)
	{
		// �ؽ�Ʈ ������ ����
		GameObject dmgText = Instantiate(damage_text_prf, GameObject.Find("Canvas").transform);

		// TextMeshProUGUI�� RectTransform ��������
		TextMeshProUGUI dmgTextComponent = dmgText.GetComponent<TextMeshProUGUI>();
		RectTransform rectTransform = dmgText.GetComponent<RectTransform>();

		// �ؽ�Ʈ ���� ����
		dmgTextComponent.text = damage.ToString();

		// RectTransform ���� (pivot ����)
		rectTransform.pivot = new Vector2(1.9f, 0.5f);

		// ȭ�� ��ǥ�� ���� ��ġ ����
		dmgText.transform.position = Camera.main.WorldToScreenPoint(transform.position);

		// 0.2�� ��� �� ����
		yield return new WaitForSeconds(0.2f);
		Destroy(dmgText);
	}

}
