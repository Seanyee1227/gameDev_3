// ���� ������ ���� ������ �� �������� ����ϴ� ������ ����ü�� ��� �����ϸ�
// ������ �߰� / ������ �� ����ü�� �����ϱ� ������ �߰� / ���� ������ �����ϴ�.

public enum WeaponName { AssaultRifle = 0 }

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName; // ���� �̸�
    public int currentAmmo; // ���� ź�� ��
    public int maxAmmo; // �ִ� ź�� ��
    public float attackRate; // ��� �ӵ�
    public float attackDistane; // ��� �Ÿ�
    public bool isAutoAttack; // ���� o / x
}

