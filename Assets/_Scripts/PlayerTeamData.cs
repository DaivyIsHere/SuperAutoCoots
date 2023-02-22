using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerTeamData", menuName = "Coots/PlayerTeamData", order = 1)]
public class PlayerTeamData : ScriptableObject
{
    public List<WeaponData> weapons;
}
