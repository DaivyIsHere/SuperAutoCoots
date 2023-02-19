using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitController : MonoBehaviour
{
    [Header("Component")]
    public Rigidbody2D rb2d;
    //public PhysicsMaterial2D fullFriction;

    [Header("State")]
    public StateManager stateManager;
    //public bool isAttacking = false;

    [Header("Canvas")]
    public TMP_Text debugText;
    public Image turnIndicator;

    [Header("UnitData")]
    private UnitData originalUnitData;
    public UnitData unitData;

    [Header("Phsyics")]
    public LayerMask groundLayer;
    public float groundY = 0f;
    //public float lastAirHeight = 0f;

    public bool lastFrameGrounded = true;
    public float defaultUpThrow = 10f;

    [Header("Dynamic Stats")]
    //info
    public int unitID;
    public BattleSide side;
    public UnitController currentOpponent;
    public List<UnitController> ignoreTargets;//have done dmg to which unit in the same turn
    //stats
    public int facing = 1;
    public int currentHealth;
    // public float invinciTimeCD = 0f;
    // public float defualtInvincTime = 0.5f;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        BattleManager.instance.OnTurnChange += OnTurnChange;
    }

    void Update()
    {
        debugText.text = stateManager.GetCurrentStateName();
    }

    void FixedUpdate()
    {
        if (IsGrounded() && (lastFrameGrounded == false))
        {
            if (stateManager.currentState != null)
            {
                stateManager.currentState.OnLanded(stateManager);
                print("Land");
            }
        }
        lastFrameGrounded = IsGrounded();
        //CalculateLastAirHeight();    
    }

    public void IniUnitData(UnitData originalUnitData)
    {
        this.originalUnitData = originalUnitData;
        unitData = Instantiate(this.originalUnitData);

        //Apply stats
        currentHealth = unitData.maxhealth;
        transform.localScale = Vector3.one * unitData.size;
    }

    private void OnTurnChange(BattleSide currentSide)
    {
        turnIndicator.gameObject.SetActive(IsOwnTurn());

        if (!IsOwnTurn())
        {
            ignoreTargets.Clear();
        }
        else
        {
            //Invoke attack ready on turn start
            stateManager.currentState.OnAttackReady(stateManager);
        }
    }

    private bool IsOwnTurn()
    {
        return side == BattleManager.instance.currentSide;
    }

    #region StateAction

    public void PerformAttack()
    {
        Vector2 positionDiff = (currentOpponent.transform.position - transform.position).normalized;//new Vector2(currentOpponent.transform.position.x - transform.position.x, 0).normalized;
        rb2d.velocity = new Vector2(positionDiff.x, 0) * unitData.dashVelocity;
    }

    #endregion

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (currentOpponent && other.gameObject == currentOpponent.gameObject)
    //     {
    //         ///Kockback
    //         Vector2 knockbackVector = new Vector2(transform.position.x - other.transform.position.x, 0).normalized;
    //         knockbackVector *= 5f;//kockback force
    //         knockbackVector.y = 5f;//Kockback Upthrow
    //         if (IsOwnTurn())
    //             knockbackVector *= unitData.knockBackResis;
    //         _rb2d.AddForce(knockbackVector, ForceMode2D.Impulse);
    //         //_rb2d.AddForce(new Vector2(0, (1f - unitData.knockBackResis) * other.relativeVelocity.magnitude), ForceMode2D.Impulse);
    //         //_rb2d.AddForce(other.relativeVelocity * (1f - unitData.knockBackResis), ForceMode2D.Impulse);

    //         ///Recover
    //         isRecovering = true;
    //         recoverCountDown = unitData.recoverTime;

    //         if (!IsOwnTurn())
    //         {
    //             ///resolve Attack
    //             int damageToTake = other.gameObject.GetComponent<UnitController>().unitData.attack;
    //             currentHealth -= damageToTake;
    //             //HealthDislay
    //             healthDisplay.text = currentHealth.ToString();

    //             //Death
    //             if (currentHealth <= 0)
    //                 print("die");

    //             //VP
    //             ValuePopupManager.instance.NewValuePopup((Vector2)transform.position + new Vector2(0, 1.5f), transform, damageToTake);
    //         }
    //         else
    //         {
    //             BattleManager.instance.EndTurn();
    //         }
    //     }
    // }

    #region ReceiveInteraction

    public void TakeDamage(UnitController attacker, int damage)
    {
        currentHealth -= damage;
        ValuePopupManager.instance.NewValuePopup(transform.position, transform, damage);
    }

    public void ReceiveKnockback(UnitController attacker, Vector2 knockbackVector)
    {
        // if (invinciTimeCD > 0)
        //     return;
        // else
        //     invinciTimeCD = defualtInvincTime;
        rb2d.AddForce(knockbackVector *= unitData.knockBackResis, ForceMode2D.Impulse);
        stateManager.currentState.OnTakeHit(stateManager);
    }

    #endregion

    #region SendInteraction

    private void DamageTarget(UnitController target)
    {
        target.TakeDamage(this, unitData.attack);
    }

    private void KnockBackTarget(UnitController target)
    {
        float velocityX = rb2d.velocity.x;
        if (velocityX > 0)
            velocityX = Mathf.Clamp(velocityX, 5f, 20f);
        else
            velocityX = Mathf.Clamp(velocityX, -20f, -5f);
        Vector2 knockbackVector = new Vector2(velocityX * 1f, 0);
        knockbackVector.y = defaultUpThrow;//defualt upthrow
        target.ReceiveKnockback(this, knockbackVector);
    }

    private bool NotInIgnoreList(UnitController unit)
    {
        foreach (var u in ignoreTargets)
        {
            if (unit == u)
                return false;
        }
        return true;
    }

    #endregion 

    #region Physic

    private void OnContactOpponent(UnitController otherUnit)
    {
        if (IsOwnTurn())
        {
            //In attack State
            if (stateManager.currentState == stateManager.attackState)
            {
                //avaliable target
                if (NotInIgnoreList(otherUnit))
                {
                    //? Perform Knockback
                    KnockBackTarget(otherUnit);
                    //? Deal dmg
                    DamageTarget(otherUnit);

                    //? Add to ignoreList
                    ignoreTargets.Add(otherUnit);
                }
            }
        }
        else
        {
            // We compute all the contact stuffs for ourself during enemy's turn
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<UnitController>())
        {
            UnitController otherUnit = other.GetComponent<UnitController>();
            if (otherUnit.side != side)
                OnContactOpponent(otherUnit);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<UnitController>())
        {
            UnitController otherUnit = other.GetComponent<UnitController>();
            if (otherUnit.side != side)
                OnContactOpponent(otherUnit);
        }
    }

    // private void CalculateLastAirHeight()
    // {
    //     if(IsGrounded())
    //     {
    //         groundY = transform.position.y;
    //     }
    //     else
    //     {
    //         float airHeight = transform.position.y - groundY;
    //         if(airHeight > lastAirHeight)
    //             lastAirHeight = airHeight;
    //     }
    // }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, GetComponent<CircleCollider2D>().radius * 1.1f, groundLayer))
            return true;
        else
            return false;
    }

    #endregion
}
