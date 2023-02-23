using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Coots/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public Sprite weaponSprite;
    public string description;

    public int damage { get { return LVdamage[level] + additionalDamage; } }
    public int maxDurability { get { return LVmaxDurability[level] + additionalDurability; } }
    public float movementVelocity { get { return LVmovementVelocity[level] + additionalVelocity; } }
    public float knockBackResis { get { return LVknockBackResis[level]; } }
    public float size { get { return LVsize[level]; } }
    public int useTurn { get { return LVuseTurn[level]; } }

    [Header("Level Stats")]
    public List<int> LVdamage = new List<int>() { 1, 1, 1 };
    public List<int> LVmaxDurability = new List<int>() { 5, 5, 5 };
    public List<float> LVmovementVelocity = new List<float>() { 10f, 10f, 10f };//magnitude
    [Range(0f, 1f)] public List<float> LVknockBackResis = new List<float>() { 0, 0, 0 };
    public List<float> LVsize = new List<float>() { 1, 1, 1 };
    public List<int> LVuseTurn = new List<int>() { 2, 2, 2 };//how many turns before switching to the next weapon

    [Header("Dynamic")]
    public int durability;
    public int totalExp = 0;
    public int level
    {
        get
        {
            if (totalExp >= lv2Exp)
                return 2;
            else if (totalExp >= lv1Exp)
                return 1;
            else if (totalExp >= lv0Exp)
                return 0;//level 0 display as lv1
            return 0;
        }
    }
    private int lv0Exp = 0;//total xp needed
    private int lv1Exp = 2;//total xp needed
    private int lv2Exp = 5;//total xp needed

    public int additionalDamage = 0;
    public int additionalDurability = 0;
    public int additionalVelocity = 0;

    [Header("Projectile")]
    public ProjectileData projectileData;
    public Vector2 spawnOffset;
    public float spawnRotZ;
    public Vector2 spawnVelocity;
    public bool worldSpaceSpawn = false;//spawn as unit's child or not


    private void OnValidate()
    {
        durability = maxDurability;
        CheckLVStats();
        // if(BattleManager.instance)s
        //     ability.IniAbility();
    }

    public void CheckLVStats()
    {
        if (LVdamage.Count != 3) WarningOnStats();
        if (LVmaxDurability.Count != 3) WarningOnStats();
        if (LVmovementVelocity.Count != 3) WarningOnStats();
        if (LVknockBackResis.Count != 3) WarningOnStats();
        if (LVsize.Count != 3) WarningOnStats();
        if (LVuseTurn.Count != 3) WarningOnStats();
    }

    public void WarningOnStats()
    {
        Debug.LogWarning("Missing stats for " + this.name);
    }

    public string GetOriginalName()
    {
        string originalName = name;
        if (originalName[originalName.Length - 1] == ')')//is (Clone)
            originalName = originalName.Substring(0, originalName.ToCharArray().Length - 7);//Remove (Clone) from weaponName
        return originalName;
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
        //unit.rb2d.AddForce(new Vector2(positionDiff.x, 0) * movementVelocity, ForceMode2D.Impulse);
        unit.rb2d.velocity = new Vector2(positionDiff.x, 0) * movementVelocity;
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
