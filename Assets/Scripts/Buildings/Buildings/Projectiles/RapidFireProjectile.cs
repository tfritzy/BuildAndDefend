using UnityEngine;

public class RapidFireProjectile : Projectile
{
    protected override TowerType TowerType => TowerType.RapidFire;

    protected override bool IsHaltingObject(Collider2D collision)
    {
        return false;
    }
}