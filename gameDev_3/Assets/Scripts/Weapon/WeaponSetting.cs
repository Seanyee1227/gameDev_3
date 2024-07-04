// 무기 종류가 여러 종류일 때 공용으로 사용하는 변수를 구조체로 묶어서 정리하면
// 변수의 추가 / 삭제될 때 구조체에 선언하기 때문에 추가 / 삭제 관리가 용이하다.

[System.Serializable]
public struct WeaponSetting
{
    public float attackRate;
    public float attackDistane;
    public bool isAutoAttack;
}

