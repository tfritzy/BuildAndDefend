using UnityEngine;

public class RapidFireProjectile : Projectile
{
    protected override BuildingType TowerType => BuildingType.RapidFire;

    protected override bool IsHaltingObject(Collider2D collision)
    {
        return false;
    }
}