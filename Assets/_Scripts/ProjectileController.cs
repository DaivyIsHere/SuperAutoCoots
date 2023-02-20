using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //*Dynamic
    public UnitController owner;
    public int damage = 0;
    public float knockbackForce = 1f;

    //*Static
    public float defaultUpThrow = 10f;

    [Header("Component")]
    public Rigidbody2D rb2d;

    [Header("LifeTime")]
    public float defualtLifeTime = 2f;
    public float lifeTime = 0f;

    private void Awake() 
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        lifeTime = defualtLifeTime;
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
        target.TakeDamage(damage);
    }

    private void KnockBackTarget(UnitController target)
    {
        float velocityX = owner.rb2d.velocity.x;
        if (velocityX > 0)
            velocityX = Mathf.Clamp(velocityX, 5f, 20f);
        else
            velocityX = Mathf.Clamp(velocityX, -20f, -5f);
        Vector2 knockbackVector = new Vector2(velocityX * knockbackForce, 0);
        knockbackVector.y = defaultUpThrow;//defualt upthrow
        target.ReceiveKnockback(knockbackVector);
    }

    private void OnContactOpponent(UnitController otherUnit)
    {
        //? Perform Knockback
        KnockBackTarget(otherUnit);
        //? Deal dmg
        DamageTarget(otherUnit);
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
