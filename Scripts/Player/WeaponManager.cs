using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private int selectedWeapon = 0;

    [SerializeField] private Shotgun weaponWithCurrentAnimation;

    //Transform.GetChild

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        Debug.Log(weaponWithCurrentAnimation.ShotgunFireAnimationFinished());

        // Mouse wheel weapon switching
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) //&& weaponWithCurrentAnimation.ShotgunFireAnimationFinished())
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f) //&& weaponWithCurrentAnimation.ShotgunFireAnimationFinished())
        {
            if(selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        // 0-9 weapon switching
/*        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			selectedWeapon = 1;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			selectedWeapon = 2;
		}*/


		if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapons();
        }
    }

    void SelectWeapons()
    {
        sbyte index = 0;
        foreach(Transform weapon in transform)
        {
            if(index == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                weaponWithCurrentAnimation = weapon.GetComponentInChildren<Shotgun>();
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            index++;
        }
    }

}
