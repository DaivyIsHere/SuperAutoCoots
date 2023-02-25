using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitController : MonoBehaviour
{
    [Header("Component")]
    public UnitWeaponDisplay unitWeaponDisplay;//Called by BattleManager
    public Rigidbody2D rb2d;

    [Header("State")]
    public StateManager stateManager;

    [Header("Canvas")]
    public TMP_Text debugText;
    public Image turnIndicator;

    [Header("WeaponData")]
    public List<WeaponData> allWeapons;//Assign by BattleManager
    public WeaponData currentWeapon;//Assign by BattleManager
    public event Action OnWeaponChange;
    public event Action<int> OnWeaponRemove;
    public event Action OnWeaponAttack;//Called by attackState

    [Header("Phsyics")]
    public LayerMask groundLayer;
    //public float groundY = 0f;
    //public float lastAirHeight = 0f;

    public bool lastFrameGrounded = true;

    [Header("Dynamic Stats")]
    //info
    public BattleSide side;
    public int currentWeaponTurn = 0;
    public int facing = 1;//face right

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
        debugText.transform.eulerAngles = Vector3.zero;
        debugText.text = stateManager.GetCurrentStateName();
    }

    void FixedUpdate()
    {
        if (IsGrounded() && (lastFrameGrounded == false))
        {
            if (stateManager.currentState != null)
            {
                stateManager.currentState.OnLanded(stateManager);
                //print("Land");
            }
        }
        lastFrameGrounded = IsGrounded();
        //CalculateLastAirHeight();    
    }

    void OnDestroy()
    {
        if (BattleManager.instance)
            BattleManager.instance.OnTurnChange -= OnTurnChange;
    }

    public void FaceTowards(float velocityX)
    {
        if (velocityX > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facing = 1;
        }
        else if (velocityX < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            facing = -1;
        }
    }

    public int GetCurrentWeaponIndex()
    {
        for (int i = 0; i < allWeapons.Count; i++)
        {
            if (currentWeapon == allWeapons[i])
            {
                return i;
            }
        }

        return -1;
    }

    public int GetNextWeaponIndex()
    {
        int currentIndex = GetCurrentWeaponIndex();
        int nextWeaponIndex;
        if (currentIndex + 1 < allWeapons.Count)
            nextWeaponIndex = currentIndex + 1;
        else
            nextWeaponIndex = 0;

        return nextWeaponIndex;
    }

    #region Turn

    private void OnTurnChange(BattleSide currentSide)
    {
        turnIndicator.gameObject.SetActive(IsOwnTurn());

        if (IsOwnTurn())
        {
            currentWeaponTurn += 1;
            if (currentWeaponTurn > currentWeapon.useTurn)
            {
                //switch next weapon;

                if (allWeapons.Count > 1)
                    SwitchToNextWeapon();

                currentWeaponTurn = 1;//1 because we will attack once this turn with the next weapon
            }
            //Invoke attack ready on turn start
            stateManager.currentState.OnAttackReady(stateManager);
        }
    }

    public void SwitchToNextWeapon()
    {
        int currentIndex = GetCurrentWeaponIndex();
        int nextWeaponIndex;
        if (currentIndex + 1 < allWeapons.Count)
            nextWeaponIndex = currentIndex + 1;
        else
            nextWeaponIndex = 0;

        currentWeapon = allWeapons[nextWeaponIndex];
        OnWeaponChange?.Invoke();
    }

    private bool IsOwnTurn()
    {
        return side == BattleManager.instance.currentSide;
    }

    public void OnAttack()//Called by attackState
    {
        OnWeaponAttack?.Invoke();
    }

    #endregion

    #region ReceiveInteraction

    public void TakeDamage(int damage, UnitController attacker)
    {
        if(allWeapons.Count <= 0)//no weapons
            return;

        currentWeapon.durability -= damage;
        ValuePopupManager.instance.NewValuePopup(transform.position, transform, damage);

        if (currentWeapon.durability <= 0)
        {
            //Die
            OnWeaponRemove?.Invoke(GetCurrentWeaponIndex());
            allWeapons.Remove(currentWeapon);
            BattleManager.instance.WeaponBroke(currentWeapon, this);

            //Switch to next weapon
            if (allWeapons.Count > 0)
                SwitchToNextWeapon();
            else
            {
                stateManager.SwitchState(stateManager.knockdownState);

                BattleManager.instance.GameOver(side);
            }
        }

        ///Face toward the hitter
        FaceTowards(transform.position.x < attacker.transform.position.x ? 1 : -1);

        if(AudioManager.instance)
            AudioManager.instance.PlayHit();
    }

    public void ReceiveKnockback(Vector2 knockbackVector)
    {
        // if (invinciTimeCD > 0)
        //     return;
        // else
        //     invinciTimeCD = defualtInvincTime;
        rb2d.AddForce(knockbackVector *= (1 - currentWeapon.knockBackResis), ForceMode2D.Impulse);
        stateManager.currentState.OnTakeHit(stateManager);
    }

    #endregion

    #region Physic

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (stateManager.currentState == stateManager.attackState)
                FaceTowards(facing * -1);
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
