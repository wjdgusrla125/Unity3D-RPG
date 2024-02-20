using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    public virtual bool Execute(GameObject target)
    {
        PlayerMana mana = target.GetComponent<PlayerMana>();

        if (mana != null && !mana.IsEmptyMana())
        {
            return true;
        }
        return false;
    }
}
