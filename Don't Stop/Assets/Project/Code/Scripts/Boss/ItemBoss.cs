using UnityEngine;
using UnityEngine.UI;


public enum EltemType
{
    Weapon,
    Gear
}
public class ItemBoss : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;

    [SerializeField] EltemType itemType;
    [SerializeField] UWeaponData weaponData;
    [SerializeField] UGearData gearData;

    [SerializeField, ReadOnly] WeaponBoss weapon;
    [SerializeField, ReadOnly] GearBoss gear;

    void Start()
    {
        if (itemType == EltemType.Weapon)
        {
            var weaponDataBoss = GameStateBoss.Get().GetEquipmentComponent().WeaponData;
            if (weaponDataBoss is not null && weaponData.WeaponType == weaponDataBoss.WeaponType)
                weaponData = weaponDataBoss;
        }
    }

    public void OnClick()
    {
        switch (itemType)
        {
            case EltemType.Weapon:
                GameObject weaponObject = new GameObject(weaponData.DisplayName);
                weapon = weaponObject.AddComponent<WeaponBoss>();
                weapon.Init(weaponData);
                GameStateBoss.Get().GetEquipmentComponentBoss().AddWeapon(weapon);
                break;
            case EltemType.Gear:
                gearData.Level = gearData.Level;
                GameObject gearObject = new GameObject(gearData.DisplayName);
                gear = gearObject.AddComponent<GearBoss>();
                gear.Init(gearData);
                GameStateBoss.Get().GetEquipmentComponentBoss().AddGear(gear);
                break;
        }
    }
}
