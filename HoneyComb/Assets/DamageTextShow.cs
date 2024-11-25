using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextShow : MonoBehaviour
{
	[Header("예는 안 갖다 붙여도 됨")]
	public TextMeshProUGUI damage_text;

	[Header("데미지 텍스트 프리펩")]
	public GameObject damage_text_prf;

	public void ShowDamage(int damage)
	{
		StartCoroutine(ShowDamageText(damage));
	}

	// 대미지 텍스트 생성 및 1초 후 삭제
	IEnumerator ShowDamageText(int damage)
	{
		// 텍스트 프리팹 생성
		GameObject dmgText = Instantiate(damage_text_prf, GameObject.Find("Canvas").transform);

		// TextMeshProUGUI와 RectTransform 가져오기
		TextMeshProUGUI dmgTextComponent = dmgText.GetComponent<TextMeshProUGUI>();
		RectTransform rectTransform = dmgText.GetComponent<RectTransform>();

		// 텍스트 내용 설정
		dmgTextComponent.text = damage.ToString();

		// RectTransform 설정 (pivot 수정)
		rectTransform.pivot = new Vector2(1.9f, 0.5f);

		// 화면 좌표에 따라 위치 설정
		dmgText.transform.position = Camera.main.WorldToScreenPoint(transform.position);

		// 0.2초 대기 후 삭제
		yield return new WaitForSeconds(0.2f);
		Destroy(dmgText);
	}

}
