using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInputController : MonoBehaviour
{
    //cache
    private Transform _transform;

    private int _bareHandsIndec = 4;
    private int _weaponIndec = 0;

    private Weapon _currentActiveWeapon;

    [SerializeField] private Camera _mainCamera;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        ChangeWeapon(_weaponIndec, 4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyboardControl.Instance.GetKeyByName("slot 1")))
        {
            ChangeWeapon(_weaponIndec, 0);
        }

        if (Input.GetKeyDown(KeyboardControl.Instance.GetKeyByName("slot 2")))
        {
            ChangeWeapon(_weaponIndec, 1);
        }

        if (Input.GetKeyDown(KeyboardControl.Instance.GetKeyByName("slot 3")))
        {
            ChangeWeapon(_weaponIndec, 2);
        }

        if (Input.GetKeyDown(KeyboardControl.Instance.GetKeyByName("slot 4")))
        {
            ChangeWeapon(_weaponIndec, 3);
        }

        if (Input.GetKeyDown(KeyboardControl.Instance.GetKeyByName("slot 5")))
        {
            ChangeWeapon(_weaponIndec, 4);
        }

        if (Input.GetKeyDown(KeyboardControl.Instance.GetKeyByName("fire")))
        {
            if (_currentActiveWeapon != null)
            {
                _currentActiveWeapon.StartShoot();
            }
        }

        if (Input.GetKeyUp(KeyboardControl.Instance.GetKeyByName("fire")))
        {
            if (_currentActiveWeapon != null)
            {
                _currentActiveWeapon.StopShoot();
            }
        }

        if (Input.GetKeyDown(KeyboardControl.Instance.GetKeyByName("reload")))
        {
            if (_currentActiveWeapon != null)
            {
                _currentActiveWeapon.Reload();
            }
        }

        if (_currentActiveWeapon != null)
        {
            _currentActiveWeapon.Aim();
        }

    }

    private void ChangeWeapon(int indec, int preferableWeaponIndec)
    {
        if (indec != preferableWeaponIndec)
        {
            gameObject.transform.GetChild(indec).gameObject.SetActive(false);
            gameObject.transform.GetChild(preferableWeaponIndec).gameObject.SetActive(true);
            _weaponIndec = preferableWeaponIndec;
        }
        else
        {
            gameObject.transform.GetChild(indec).gameObject.SetActive(false);
            gameObject.transform.GetChild(_bareHandsIndec).gameObject.SetActive(true);
            _weaponIndec = _bareHandsIndec;
        }

        _currentActiveWeapon = gameObject.transform.GetChild(_weaponIndec).GetComponent<Weapon>();
    }
}
