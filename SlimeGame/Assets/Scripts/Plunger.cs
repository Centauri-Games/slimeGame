using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : Gun
{
    public override void Use()
    {
        Debug.Log("Using gun " + itemInfo.itemName);
    }

    public override void End(){}
}
