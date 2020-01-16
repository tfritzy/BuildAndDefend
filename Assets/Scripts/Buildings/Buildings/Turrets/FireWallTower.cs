using System.Collections.Generic;
using UnityEngine;

public class FireWallTower : DragSelectTower
{
    public override TowerType Type => TowerType.FireWall;
    public int MaxFireSegments;
    public float FireDamageTickGapInSeconds;

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 1;
        this.FireCooldown = 5f;
        this.ProjectileLifespan = 3f;
        this.MaxFireSegments = 6;
        this.FireDamageTickGapInSeconds = 1f;
    }

    protected override GameObject CreateProjectile()
    {
        List<GameObject> tilesInBetween = new List<GameObject>();
        RaycastHit2D[] hits = Physics2D.LinecastAll(this.dragStartPos, this.dragEndPos);
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
            GameObject fireSegmentInst = Instantiate(fireWallSegment, tile.transform.position, new Quaternion(), null);
            SetProjectileValues(fireSegmentInst);
        }
        lastFireTime = Time.time;
        return fireWallSegment;
    }

    protected override void SetProjectileValues(GameObject p)
    {
        p.GetComponent<ConstantDamageProjectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this.ProjectilePierce,
            this,
            this.FireDamageTickGapInSeconds);
    }
}