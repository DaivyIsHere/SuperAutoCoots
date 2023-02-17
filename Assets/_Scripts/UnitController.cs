using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitController : MonoBehaviour
{
    [Header("Component")]
    public UnitController currentOpponent;
    private Rigidbody2D _rb2d;
    //* Canavs
    public TMP_Text healthDisplay;
    public TMP_Text debugText;
    public Image turnIndicator;

    private UnitData originalUnitData;
    public UnitData unitData;

    //*Dynamic Stats
    public int unitID;
    public BattleSide side;
    public int currentHealth;
    public bool isRecovering = false;
    public float recoverCountDown;
    public Vector2 targetPosition;

    //* Static Value

    // [Header("Attibute")]
    // //* Stats
    // public int attack = 1;
    // public int maxhealth = 5;

    // //* Speedup
    // public float maxSpeed = 10f;//magnitude
    // public float acceleration = 10f;
    // [Space]
    // //* Knockback
    // public float knockBackUpThrow = 5f;
    // public float knockBackForce = 5f;
    // public Vector2 knockBackVector;
    // [Space]
    // //*Recover
    // public float recoverTime = 0.3f;
    // [Space]
    // //*Target
    // public float targetUpdateInterval = 0.5f;

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(UpdateTargetPosition());
    }

    void Update()
    {
        HandleMovement();
        HandleRecover();

        if (isRecovering)
            debugText.text = "recovering";
        else
            debugText.text = "";

        turnIndicator.gameObject.SetActive(IsOwnTurn());
    }

    public void IniUnitData(UnitData originalUnitData)
    {
        this.originalUnitData = originalUnitData;
        unitData = Instantiate(this.originalUnitData);

        //Apply stats
        currentHealth = unitData.maxhealth;
        healthDisplay.text = currentHealth.ToString();
        transform.localScale = Vector3.one * unitData.size;
    }

    private void HandleMovement()
    {
        if (!currentOpponent)
            return;

        if (isRecovering)
            return;

        if (_rb2d.velocity.magnitude < unitData.maxSpeed)
        {
            Vector2 horizontalDis = new Vector2(targetPosition.x - transform.position.x, 0);
            _rb2d.AddForce(unitData.acceleration * horizontalDis.normalized * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private bool IsOwnTurn()
    {
        return side == BattleManager.instance.currentSide;
    }

    private IEnumerator UpdateTargetPosition()
    {
        while (true)
        {
            if (currentOpponent)
                targetPosition = currentOpponent.transform.position;
            else
                targetPosition = Vector2.zero;
            yield return new WaitForSeconds(unitData.targetUpdateInterval);
        }
    }

    private void HandleRecover()
    {
        if (isRecovering)
        {
            recoverCountDown -= Time.deltaTime;
            if (recoverCountDown <= 0)
                isRecovering = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentOpponent && other.gameObject == currentOpponent.gameObject)
        {
            ///Kockback
            Vector2 knockbackVector = new Vector2(transform.position.x - other.transform.position.x, 0).normalized;
            knockbackVector *= 5f;//kockback force
            knockbackVector.y = 5f;//Kockback Upthrow
            if (IsOwnTurn())
                knockbackVector *= unitData.knockBackResis;
            _rb2d.AddForce(knockbackVector, ForceMode2D.Impulse);
            //_rb2d.AddForce(new Vector2(0, (1f - unitData.knockBackResis) * other.relativeVelocity.magnitude), ForceMode2D.Impulse);
            //_rb2d.AddForce(other.relativeVelocity * (1f - unitData.knockBackResis), ForceMode2D.Impulse);

            ///Recover
            isRecovering = true;
            recoverCountDown = unitData.recoverTime;

            if (!IsOwnTurn())
            {
                ///resolve Attack
                int damageToTake = other.gameObject.GetComponent<UnitController>().unitData.attack;
                currentHealth -= damageToTake;
                //HealthDislay
                healthDisplay.text = currentHealth.ToString();

                //Death
                if (currentHealth <= 0)
                    print("die");

                //VP
                ValuePopupManager.instance.NewValuePopup((Vector2)transform.position + new Vector2(0, 1.5f), transform, damageToTake);
            }
            else
            {
                BattleManager.instance.EndTurn();
            }
        }
    }
}
