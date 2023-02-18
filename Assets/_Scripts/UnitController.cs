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

    [Header("Dynamic Stats")]
    //info
    public int unitID;
    public BattleSide side;
    public UnitController currentOpponent;
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

        }
        else
        {
            //Invoke attack ready on turn start
            stateManager.currentState.OnAttackReady(stateManager);
        }
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

    public void ReceiveKnockback(UnitController attacker, Vector2 knockbackVector)
    {
        // if (invinciTimeCD > 0)
        //     return;
        // else
        //     invinciTimeCD = defualtInvincTime;
        rb2d.AddForce(knockbackVector *= unitData.knockBackResis, ForceMode2D.Impulse);
        stateManager.currentState.OnTakeHit(stateManager);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //hit opponent
        if (other.gameObject.GetComponent<UnitController>()?.side == side.Opposite())
        {
            if (IsOwnTurn())
            {
                if (stateManager.currentState == stateManager.attackState)
                {
                    ///Perform Knockback
                    //print(Mathf.Abs(_rb2d.velocity.x));
                    float velocityX = rb2d.velocity.x;
                    if (velocityX > 0)
                        velocityX = Mathf.Clamp(velocityX, 5f, 20f);
                    else
                        velocityX = Mathf.Clamp(velocityX, -20f, -5f);
                    Vector2 knockbackVector = new Vector2(velocityX * 1f, 0);
                    //knockbackVector *= Mathf.Abs(_rb2d.velocity.x) * 1f;//unitData.knockBackForce;
                    knockbackVector.y = 10f;//defualt upthrow
                    other.GetComponent<UnitController>().ReceiveKnockback(this, knockbackVector);
                }
            }
            else
            {
                // We compute all the contact stuffs for ourself during enemy's turn
            }
        }
    }

    private bool IsOwnTurn()
    {
        return side == BattleManager.instance.currentSide;
    }

}