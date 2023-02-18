using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Coots/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("Attribute")]
    //* Stats
    public int attack = 1;
    public int maxhealth = 5;
    [Space]
    //*Size
    public float size = 1f;
    [Space]
    //* Attack
    public float dashVelocity = 10f;//magnitude
    public float dashDuraing = 0.5f;
    [Space]
    //* Speed
    public float maxSpeed = 10f;//magnitude
    public float acceleration = 10f;
    [Space]
    //*Recover
    public float recoverTime = 0.3f;
    [Space]
    //*Target
    public float targetUpdateInterval = 0.5f;
    [Space]
    //* Knockback
    public float knockBackForce = 5f;
    [Range(0f,1f)] public float knockBackResis = 0f;
    // public float knockBackUpThrow = 5f;
    // public Vector2 knockBackVector;
}