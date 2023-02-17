using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuePopupManager : Singleton<ValuePopupManager>
{
    public GameObject VpPref;

    public void NewValuePopup(Vector2 pos, Transform container, int damage)
    {
        GameObject newVP = Instantiate(VpPref, pos, Quaternion.identity, container);
        newVP.GetComponent<ValuePopup>().textDisplay.text = damage.ToString();
    }
}
