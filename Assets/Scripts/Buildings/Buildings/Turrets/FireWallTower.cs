using System.Collections.Generic;
using UnityEngine;

public class FireWallTower : Tower
{
    public override BuildingType Type => BuildingType.FireWall;
    public int MaxFireSegments;
    public float FireDamageTickGapInSeconds;
    public override string Name => "Fire Wall";
    public override Faction Faction => Faction.Fire;
    public override Vector2Int Size => new Vector2Int(1, 1);
    public override ResourceDAO PowerUpCost
    {
        get
        {
            return new ResourceDAO(
                gold: 100 * this.Tier,
                wood: 40 * this.Tier,
                stone: 10 * this.Tier);
        }
    }
    protected override InputController inputController
    {
        get
        {
            if (_inputController == null)
            {
                _inputController = new DragSelectInput();
            }
            return _inputController;
        }
    }

    public override TowerStats GetTowerParameters(int level, int tier)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100 + 10 * level + 5 * tier;
        stats.Damage = 1 + level * 5 + tier * 3;
        stats.FireCooldown = 5f;
        stats.ProjectileLifespan = 3f;
        this.MaxFireSegments = 6;
        this.FireDamageTickGapInSeconds = 1f;
        return stats;
    }

    protected override GameObject CreateProjectile(InputDAO input)
    {
        List<GameObject> tilesInBetween = new List<GameObject>();
        RaycastHit2D[] hits = Physics2D.LinecastAll(
            ((DragSelectInputDAO)input).point1.Value,
            ((DragSelectInputDAO)input).point2.Value);
        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.gameObject.CompareTag(Tags.Terrain))
            {
                continue;
            }

            if (!hit.collider.gameObject.GetComponent<EnvironmentTile>().CanBeBuiltUpon)
            {
                continue;
            }

            tilesInBetween.Add(hit.collider.gameObject);
        }

        // TODO Replace resource load with gameobject pooling.
        GameObject fireWallSegment = Resources.Load<GameObject>($"{FilePaths.Projectiles}/{this.projectilePrefabName}");
        for (int i = 0; i < Mathf.Min(tilesInBetween.Count, MaxFireSegments); i++)
        {
            GameObject tile = tilesInBetween[i];
            GameObject fireSegmentInst = Instantiate(fireWallSegment, tile.transform.position + Vector3.back, new Quaternion(), null);
            SetProjectileValues(fireSegmentInst, input);
        }
        lastFireTime = Time.time;
        return fireWallSegment;
    }

    protected override void SetProjectileValues(GameObject p, InputDAO input)
    {
        ConstantDamageProjectileStatsDAO stats = new ConstantDamageProjectileStatsDAO
        {
            Damage = Stats.Damage,
            Lifespan = Stats.ProjectileLifespan,
            PierceCount = Stats.Pierce,
            Owner = this,
            DamageTickGapInSeconds = this.FireDamageTickGapInSeconds,
        };

        p.GetComponent<Projectile>().SetParameters(stats);
    }
}