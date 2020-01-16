using UnityEngine;

public abstract class StretchProjectileTower : Tower
{
    protected void ScaleProjectile(GameObject projectile, Vector3 fireDirection)
    {
        ColliderDistance2D projectileLength = closestHaltingHit(fireDirection);
        projectileLength.distance += (projectileLength.pointB - (Vector2)this.transform.position).magnitude;
        Vector3 currentScale = projectile.transform.localScale;
        currentScale.y = currentScale.y * projectileLength.distance;
        projectile.transform.localScale = currentScale;
    }

    protected override GameObject CreateProjectile(UnityEngine.Vector2 fireDirection)
    {
        GameObject instProj = base.CreateProjectile(fireDirection);
        ScaleProjectile(instProj, fireDirection);
        return instProj;
    }

    protected ColliderDistance2D closestHaltingHit(Vector2 fireDirection)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, fireDirection);
        ColliderDistance2D closestDist = new ColliderDistance2D();
        closestDist.distance = 100f;
        foreach (RaycastHit2D hit in hits)
        {
            if (!isHaltingObject(hit.transform.gameObject))
            {
                continue;
            }

            ColliderDistance2D distance = hit.transform.gameObject.GetComponent<Collider2D>().Distance(this.GetComponent<Collider2D>());
            if (distance.distance < closestDist.distance)
            {
                closestDist = distance;
            }
        }
        return closestDist;
    }
}