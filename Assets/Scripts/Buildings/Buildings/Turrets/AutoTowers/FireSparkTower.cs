using UnityEngine;

public class FireSparkTower : Tower
{
    public override TowerType Type => TowerType.FireSpark;

    protected override InputController inputController
    {
        get
        {
            if (this._inputController == null)
            {
                this._inputController = new VectorAutoInput(this);
            }
            return this._inputController;
        }
    }

    public override Vector2Int Size => new Vector2Int(0, 0);

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 5;
        this.inaccuracy = .05f;
        this.ProjectileMovementSpeed = 10;
        this.FireCooldown = 0.3f;
        this.ProjectileLifespan = 1f;
        this.Range = 1f;
    }
}