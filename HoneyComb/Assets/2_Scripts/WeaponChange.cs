using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    public List<GameObject> weapons; // ���� ����Ʈ
    private int selectedWeapon = 0;  // ���� ���õ� ���� �ε���


    void Start()
    {
        SelectWeapon();  // ������ �� �⺻ ���� ����
    }

    void Update()
    {
        // ���� Ű�� ���� ��ü
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
            SelectWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count >= 2)
        {
            selectedWeapon = 1;
            SelectWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Count >= 3)
        {
            selectedWeapon = 2;
            SelectWeapon();
        }

        // ���콺 �ٷ� ���� ��ü
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            selectedWeapon = (selectedWeapon + 1) % weapons.Count;
            SelectWeapon();
        }
        else if (scroll < 0f)
        {
            selectedWeapon--;
            if (selectedWeapon < 0) selectedWeapon = weapons.Count - 1;
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        // ��� ���⸦ ��Ȱ��ȭ�ϰ�, ���õ� ���⸸ Ȱ��ȭ
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == selectedWeapon);
        }
    }
}
