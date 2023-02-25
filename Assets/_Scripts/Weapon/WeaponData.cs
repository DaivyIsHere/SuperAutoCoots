using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Coots/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public Sprite weaponSprite;
    [Multiline(5)]
    public string description;

    public int damage { get { return LVdamage[level] + additionalDamage; } }
    public int maxDurability { get { return LVmaxDurability[level] + additionalDurability; } }
    public float movementVelocity { get { return LVmovementVelocity[level] + additionalVelocity; } }
    public int useTurn { get { return LVuseTurn[level]; } }
    public float knockbackMult { get { return LVknockbackMult[level]; } }
    public float knockBackResis { get { return LVknockBackResis[level]; } }
    public float size { get { return LVsize[level]; } }
    public float backStabDamageMult { get { return LVbackStabDamageMult[level]; } }
    public float defaultLifeTime { get { return LVprojectileLifeTime[level]; } }

    //[Header("Level Stats")]
    [GUIColor(1, 0.6f, 0.4f)][LabelText("DMG")][HorizontalGroup("Basic Stats", LabelWidth = 20)] public List<int> LVdamage = new List<int>() { 1, 1, 1 };
    [GUIColor(1, 0.6f, 0.4f)][LabelText("HEALTH")][HorizontalGroup("Basic Stats", LabelWidth = 20)] public List<int> LVmaxDurability = new List<int>() { 5, 5, 5 };
    [GUIColor(1, 0.6f, 0.4f)][LabelText("SPEED")][HorizontalGroup("Basic Stats", LabelWidth = 20)] public List<float> LVmovementVelocity = new List<float>() { 10f, 10f, 10f };//magnitude
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)][LabelText("KBForce")][HorizontalGroup("Knockback", LabelWidth = 20)][Range(1f, 3f)] public List<float> LVknockbackMult = new List<float>() { 1, 1, 1 };//velocity.magnitude multiply by this
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)][LabelText("KBResis")][HorizontalGroup("Knockback", LabelWidth = 20)][Range(0f, 1f)] public List<float> LVknockBackResis = new List<float>() { 0, 0, 0 };
    [LabelText("Turn")][HorizontalGroup("Info", LabelWidth = 20)] public List<int> LVuseTurn = new List<int>() { 2, 2, 2 };//how many turns before switching to the next weapon
    [LabelText("Size")][HorizontalGroup("Info", LabelWidth = 20)] public List<float> LVsize = new List<float>() { 1, 1, 1 };
    [LabelText("BackStab")] public List<float> LVbackStabDamageMult = new List<float>() { 1, 1, 1 };
    public List<float> LVprojectileLifeTime = new List<float>() { 1, 1, 1 };
    public bool projectilePenetrate = true;

    [Header("Projectile")]
    public ProjectileData projectileData;
    public Sprite projectileSpriteOverride;//Leave empty will use original
    public Vector2 spawnOffset;
    public float spawnRotZ;
    public Vector2 spawnVelocity;
    public float spawnScale = 1;
    public bool pflipX = false;
    public bool worldSpaceSpawn = false;//spawn as unit's child or not

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
        Vector2 velocity = new Vector2(spawnVelocity.x * unitFacing, spawnVelocity.y);
        GameObject projectilePref = (GameObject)Resources.Load("Prefabs/Projectile");
        ProjectileController projectile = Instantiate(projectilePref, spawnPos, Quaternion.identity).GetComponent<ProjectileController>();

        //Vector2 spawnEuler = new Vector3(0, unitFacing == 1 ? 0 : 180, spawnRotZ);
        if (!worldSpaceSpawn)
        {
            projectile.transform.parent = unit.transform;
            if (unitFacing == 1)//Face right
            {
                projectile.transform.eulerAngles = new Vector3(0, 0, spawnRotZ);
                projectile.sprite.flipX = pflipX ? true : false;
            }
            else//Face Left
            {
                projectile.transform.eulerAngles = new Vector3(0, 0, -spawnRotZ);
                projectile.sprite.flipX = pflipX ? false : true;
            }

            projectile.rb2d.isKinematic = true;
        }
        else
        {
            projectile.transform.eulerAngles = new Vector3(0, unitFacing == 1 ? 0 : 180, spawnRotZ);
            projectile.rb2d.isKinematic = false;
        }

        projectile.rb2d.velocity = velocity;
        projectile.owner = unit;
        projectile.damage = damage;
        projectile.facing = unitFacing;

        projectile.projectileData = projectileData;
        projectile.projectileData.knockbackMult = knockbackMult;
        projectile.projectileData.backStabDamageMult = backStabDamageMult;
        projectile.projectileData.penetrate = projectilePenetrate;
        projectile.projectileData.defaultLifeTime = defaultLifeTime;

        projectile.sprite.sprite = projectileSpriteOverride ? projectileSpriteOverride : weaponSprite;
        projectile.sprite.transform.localScale = Vector3.one * spawnScale;
    }

}
