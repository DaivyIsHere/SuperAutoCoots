using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Component")]
    public UnitController owner;
    public ProjectileData projectileData;
    public Rigidbody2D rb2d;
    public SpriteRenderer sprite;

    public int damage = 0;
    public float lifeTime = 0f;
    public int facing = 1;

    //*Const
    private float minVelocityKnockback = 5f;
    private float maxVelocityKnockback = 20f;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (!projectileData)
            Debug.LogError("No ProjectileData Assigned.");

        lifeTime = projectileData.defaultLifeTime;
        if (projectileData.effectByGravity)
            rb2d.gravityScale = 1;
        else
            rb2d.gravityScale = 0;
    }

    void Update()
    {
        if (lifeTime > 0)
            lifeTime -= Time.deltaTime;
        else
            Destroy(this.gameObject);
    }

    private void DamageTarget(UnitController target)
    {
        if (target.facing == facing)
            target.TakeDamage(Mathf.RoundToInt(damage * projectileData.backStabDamageMult), owner);
        else
            target.TakeDamage(damage, owner);
    }

    private void KnockBackTarget(UnitController target)
    {
        float velocityX = owner.rb2d.velocity.x;
        if (velocityX >= 0)
            velocityX = Mathf.Clamp(velocityX, minVelocityKnockback, maxVelocityKnockback);
        else
            velocityX = Mathf.Clamp(velocityX, -maxVelocityKnockback, -minVelocityKnockback);
        Vector2 knockbackVector = new Vector2(velocityX * projectileData.knockbackMult, 0);
        knockbackVector.y = projectileData.upthrowForce;//defualt upthrow
        target.ReceiveKnockback(knockbackVector);
    }

    private void OnContactOpponent(UnitController otherUnit)
    {
        //? Perform Knockback
        KnockBackTarget(otherUnit);
        //? Deal dmg
        DamageTarget(otherUnit);

        if (!projectileData.penetrate)
            Destroy(this.gameObject);

        //dont collide with it again
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), otherUnit.GetComponent<Collider2D>(), true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //detect opponent
        if (other.gameObject.GetComponent<UnitController>())
        {
            UnitController otherUnit = other.GetComponent<UnitController>();
            if (otherUnit.side != owner.side)
                OnContactOpponent(otherUnit);
        }
    }
}
