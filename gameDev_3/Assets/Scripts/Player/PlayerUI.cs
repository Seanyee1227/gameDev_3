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
    private TextMeshProUGUI _weaponName; // ���� �̸�
    [SerializeField]
    private Image _weaponIcon; // ���� ������
    [SerializeField]
    private Sprite[] _weponIconSprite; // ���� ������ ��������Ʈ �迭

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI _Ammo; // źâ ��

    [Header("Magazine")]
    [SerializeField]
    private GameObject _magazinPrefab; // ź�� ������
    [SerializeField]
    private Transform _magazineParnel; // ź�� UI �г�
    private List<GameObject> _magazineList; // ź�� UI ����Ʈ

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
        // ���� ź�� �� ��ŭ Ȱ����
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
