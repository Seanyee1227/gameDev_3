using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WeaponAssualtRifle _weapon;

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI _weaponName; // 무기 이름
    [SerializeField]
    private Image _weaponIcon; // 무기 아이콘
    [SerializeField]
    private Sprite[] _weponIconSprite; // 무기 아이콘 스프라이트 배열

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI _Ammo; // 탄창 수

    [Header("Magazine")]
    [SerializeField]
    private GameObject _magazinPrefab; // 탄약 프리팹
    [SerializeField]
    private Transform _magazineParnel; // 탄약 UI 패널
    private List<GameObject> _magazineList; // 탄약 UI 리스트

    private void Awake()
    {
        SetupWeapon();
        SetupMagazine();

        _weapon.onAmmoEvent.AddListener(UpadateAmmoUI);
        _weapon.onMagazinEvent.AddListener(UpdateMagazineUI);
    }

    private void SetupWeapon()
    {
        _weaponName.text = _weapon.weaponName.ToString();
        _weaponIcon.sprite = _weponIconSprite[(int)_weapon.weaponName];
    }

    private void SetupMagazine()
    {
        _magazineList = new List<GameObject>();

        for (int i = 0; i < _weapon.currentMagazine; ++i)
        {
            GameObject _clone = Instantiate(_magazinPrefab);
            _clone.transform.SetParent(_magazineParnel);
            _clone.SetActive(false);

            _magazineList.Add(_clone);
        }

        for (int i = 0; i < _weapon.currentMagazine; ++i)
        {
            _magazineList[i].SetActive(true);
        }
    }

    private void UpdateMagazineUI(int _currentMagazine)
    {
        // 지금 탄약 수 만큼 활성하
        for (int i = 0; i < _magazineList.Count; ++i)
        {
            _magazineList[i].SetActive(false);
        }
        for (int i = 0; i < _currentMagazine; ++i)
        {
            _magazineList[i].SetActive(true);
        }
    }

    private void UpadateAmmoUI(int _currntAmmo, int _maxAmmo)
    {
        _Ammo.text = $"<size=40>{_currntAmmo}/</size>{_maxAmmo}";   
    }
}
