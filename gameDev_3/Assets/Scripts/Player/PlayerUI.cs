using UnityEngine.UI;
using UnityEngine;
using TMPro;

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
    private TextMeshProUGUI _Ammo;

    private void Awake()
    {
        SetupWeapon();

        _weapon.onAmmoEvent.AddListener(UpadateAmmoUI);
    }

    private void SetupWeapon()
    {
        _weaponName.text = _weapon.weaponName.ToString();
        _weaponIcon.sprite = _weponIconSprite[(int)_weapon.weaponName];
    }

    private void UpadateAmmoUI(int _currntAmmo, int _maxAmmo)
    {
        _Ammo.text = $"<size=40>{_currntAmmo}/</size>{_maxAmmo}";
    }
}
