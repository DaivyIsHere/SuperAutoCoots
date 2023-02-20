using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Coots/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    //*Static
    [Header("Stats")]
    public int damage = 1;
    public int maxDurability = 5;
    //public float size = 1f;

    [Header("Attack")]
    public float movementVelocity = 10f;//magnitude
    public float movementDuration = 0.5f;//Or attack duration //TODO not assign to attackState yet

    [Header("Knockback")] //TODO move to projectile
    [Range(1f, 3f)]
    public float knockbackForce = 1f;//velocity.magnitude multiply by this
    [Range(0f, 1f)] public float knockBackResis = 0f;

    [Header("Projectile")]
    public GameObject projectilePref;
    public Vector2 spawnOffset; //TODO will change based on facing
    public Vector3 spawnRotation; //TODO will change based on facing
    public Vector2 spawnVelocity;
    public bool worldSpaceSpawn = false;//spawn as unit's child or not

    //*dynamic
    public int weaponID;
    public int durability;

    private void OnEnable()
    {
        durability = maxDurability;
        // if(BattleManager.instance)s
        //     ability.IniAbility();
    }

    public void PerformAttack(UnitController unit)
    {
        AttckMovement(unit);
        SpawnProjectile(unit);
    }

    public virtual void AttckMovement(UnitController unit)
    {
        if (!BattleManager.instance.GetOpponentUnit(unit.side))
            return;

        Vector2 positionDiff = (BattleManager.instance.GetOpponentUnit(unit.side).transform.position - unit.transform.position).normalized;
        unit.rb2d.velocity = new Vector2(positionDiff.x, 0) * movementVelocity;
    }

    public virtual void SpawnProjectile(UnitController unit)
    {
        ProjectileController projectile = Instantiate(projectilePref, (Vector2)unit.transform.position + spawnOffset, Quaternion.Euler(spawnRotation)).GetComponent<ProjectileController>();

        if (!worldSpaceSpawn)
        {
            projectile.transform.parent = unit.transform;
            projectile.rb2d.isKinematic = true;
        }
        else
        {
            projectile.rb2d.isKinematic = false;
        }

        projectile.rb2d.velocity = spawnVelocity;
        projectile.owner = unit;
        projectile.damage = damage;
    }

}
