public class Fireball : ExplosiveProjectile
{
    private float _explosionRadius = 1f;
    public override float ExplosionRadius
    {
        get => _explosionRadius;
        set
        {
            _explosionRadius = value;
        }
    }

    private string _explosionPrefabName = "FireballExplosion";
    protected override string explosionPrefabName
    {
        get => _explosionPrefabName;
        set
        {
            _explosionPrefabName = value;
        }
    }

}