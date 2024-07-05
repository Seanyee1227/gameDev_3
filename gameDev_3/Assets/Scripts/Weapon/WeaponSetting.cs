// 무기 종류가 여러 종류일 때 공용으로 사용하는 변수를 구조체로 묶어서 정리하면
// 변수의 추가 / 삭제될 때 구조체에 선언하기 때문에 추가 / 삭제 관리가 용이하다.

public enum WeaponName { AssaultRifle = 0 }

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName; // 무기 이름
    public int currentAmmo; // 현재 탄약 수
    public int maxAmmo; // 최대 탄약 수
    public float attackRate; // 사격 속도
    public float attackDistane; // 사격 거리
    public bool isAutoAttack; // 오토 o / x
}

