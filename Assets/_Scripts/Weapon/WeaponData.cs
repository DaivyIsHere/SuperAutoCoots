using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Coots/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public Sprite weaponSprite;
    public string description;
    
    [Header("Static")]
    public int damage = 1;
    public int maxDurability = 5;
    [Space]
    public float movementVelocity = 10f;//magnitude
    [Range(0f, 1f)] public float knockBackResis = 0f;
    public float size = 1f;
    public int useTurn = 3;//how many turns before switching to the next weapon

    [Header("Dynamic")]
    public int weaponID;
    public int durability;

    [Header("Projectile")]
    public ProjectileData projectileData;
    public Vector2 spawnOffset;
    public float spawnRotZ;
    public Vector2 spawnVelocity;
    public bool worldSpaceSpawn = false;//spawn as unit's child or not


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
        unit.rb2d.AddForce(new Vector2(positionDiff.x, 0) * movementVelocity, ForceMode2D.Impulse);
        //unit.rb2d.velocity = new Vector2(positionDiff.x, 0) * movementVelocity;
        unit.FaceTowards(positionDiff.x);
    }

    public virtual void SpawnProjectile(UnitController unit)
    {
        int unitFacing = unit.facing;
        Vector2 spawnPos = (Vector2)unit.transform.position + new Vector2(spawnOffset.x * unitFacing, spawnOffset.y);
        Vector2 spawnEuler = new Vector3(0, unitFacing == 1 ? 0 : 180, spawnRotZ);
        Vector2 velocity = new Vector2(spawnVelocity.x * unitFacing, spawnVelocity.y);
        GameObject projectilePref = (GameObject)Resources.Load("Prefabs/Projectile");
        ProjectileController projectile = Instantiate(projectilePref, spawnPos, Quaternion.Euler(spawnEuler)).GetComponent<ProjectileController>();

        if (!worldSpaceSpawn)
        {
            projectile.transform.parent = unit.transform;
            projectile.rb2d.isKinematic = true;
        }
        else
        {
            projectile.rb2d.isKinematic = false;
        }

        projectile.rb2d.velocity = velocity;
        projectile.owner = unit;
        projectile.projectileData = projectileData;
        projectile.damage = damage;
        projectile.facing = unitFacing;
    }

}
