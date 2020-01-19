using UnityEngine;

public abstract class StretchProjectileTower : Tower
{
    protected void ScaleProjectile(GameObject projectile, Vector3 fireDirection)
    {
        float projectileLength = closestHaltingHit(fireDirection);
        // projectileLength.distance += Vector3.Distance(projectileLength, this.transform.position);
        Vector3 currentScale = projectile.transform.localScale;
        currentScale.y = currentScale.y * projectileLength;
        projectile.transform.localScale = currentScale;
    }

    protected override GameObject CreateProjectile(InputDAO input)
    {
        Vector2 fireDirection = CalculateProjectileTargetLocation(((VectorInputDAO)input).location.Value);

        // TODO: Have towers pool projectiles
        GameObject instProj = Instantiate(
            Resources.Load<GameObject>($"{FilePaths.Projectiles}/{this.projectilePrefabName}"),
            this.transform.position,
            new Quaternion(),
            null);

        SetProjectileRotation(instProj, fireDirection);
        ScaleProjectile(instProj, fireDirection);

        instProj.GetComponent<Rigidbody2D>().velocity = fireDirection * ProjectileMovementSpeed;
        SetProjectileValues(instProj, input);
        return instProj;
    }

    protected float closestHaltingHit(Vector2 fireDirection)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, fireDirection);
        float closestDist = 100f;
        foreach (RaycastHit2D hit in hits)
        {
            if (!isHaltingObject(hit.transform.gameObject))
            {
                continue;
            }

            float distance = Vector2.Distance(hit.point, this.transform.position);
            if (distance < closestDist)
            {
                closestDist = distance;
            }
        }
        return closestDist;
    }
}