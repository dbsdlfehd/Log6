using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner2 : MonoBehaviour
{
	//���Ÿ� ���ݿ� ��ũ��Ʈ
	public float scanRange;
	public LayerMask targetLayer;
	public RaycastHit2D[] targets;
	public Transform nearestTarget;

	void FixedUpdate()
	{
		targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); //�� �����ȿ� Ÿ�� ��ĵ
		nearestTarget = GetNearest();
	}

	Transform GetNearest()
	{
		Transform result = null;
		float diff = 100;

		foreach (RaycastHit2D target in targets)
		{
			Vector3 mypos = transform.position;
			Vector3 targetpos = target.transform.position;
			float curdiff = Vector3.Distance(mypos, targetpos);

			if (curdiff < diff)
			{
				diff = curdiff;
				result = target.transform;
			}
		}

		return result;
	}
}
