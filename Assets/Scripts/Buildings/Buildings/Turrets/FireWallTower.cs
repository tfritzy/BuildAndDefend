using System.Collections.Generic;
using UnityEngine;

public class FireWallTower : DragSelectTower
{
    public override BuildingType Type => BuildingType.FireWall;
    protected override string projectilePrefabName => "FireWall";
    public int MaxFireSegments;
    public float FireDamageTickGapInSeconds;

    public override void SetTowerParameters() {
        this.Health = 100;
        this.projectileDamage = 1;
        this.fireCooldown = 5f;
        this.projectileLifespan = 3f;
        this.MaxFireSegments = 6;
        this.FireDamageTickGapInSeconds = 1f;
    }

    protected override void CreateProjectile()
    {
        List<GameObject> tilesInBetween = new List<GameObject>(); 
        RaycastHit2D[] hits = Physics2D.LinecastAll(this.dragStartPos, this.dragEndPos);
        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.gameObject.CompareTag(Tags.Terrain)){
                continue;
            }

            if (!hit.collider.gameObject.GetComponent<EnvironmentTile>().CanBeBuiltUpon){
                continue;
            }

            tilesInBetween.Add(hit.collider.gameObject);
        }

        // TODO Replace resource load with gameobject pooling.
        GameObject fireWallSegment = Resources.Load<GameObject>($"{FilePaths.Projectiles}/{this.projectilePrefabName}");
        for (int i = 0; i < Mathf.Min(tilesInBetween.Count, MaxFireSegments); i++){
            GameObject tile = tilesInBetween[i];
            GameObject fireSegmentInst = Instantiate(fireWallSegment, tile.transform.position, new Quaternion(), null);
            SetProjectileValues(fireSegmentInst.GetComponent<Projectile>());
        }
        lastFireTime = Time.time;
    }

    protected override void SetProjectileValues(Projectile p) {
        p.GetComponent<ConstantDamageProjectile>().SetParameters(
            this.projectileDamage,
            this.projectileLifespan,
            this.projectilePierce,
            this,
            this.FireDamageTickGapInSeconds);
    }
}