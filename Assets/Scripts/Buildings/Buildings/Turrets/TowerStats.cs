public class TowerStats : BuildingStats
{
    public int Damage;
    public float CriticalHitChance;
    public float CriticalHitDamage;
    public float FireCooldown;
    public float Range;
    public float Inaccuracy;
    public float ExplosionRadius;
    public int Pierce;
    public float ProjectileMovementSpeed;
    public float ProjectileLifespan;

    public string FieldsToString()
    {
        return $"Health\nDamage\nCriticalHitChance\nCriticalHitDamage\nFireCooldown\nRange\nInaccuracy\nExplosionRadius\nPierce";
    }

    public override string ToString()
    {
        return $"{Health}\n{Damage}\n{CriticalHitChance}\n{CriticalHitDamage}\n{FireCooldown}\n{Range}\n{Inaccuracy}\n{ExplosionRadius}\n{Pierce}";
    }
}