using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjetileData", menuName = "Coots/ProjectileData", order = 1)]
public class ProjectileData : ScriptableObject
{
    [Range(1f, 3f)] public float knockbackForce = 1f;//velocity.magnitude multiply by this
    public float backStabDamageMult = 1f;
    public bool penetrate = true;
    public float defaultLifeTime = 1f;
}
