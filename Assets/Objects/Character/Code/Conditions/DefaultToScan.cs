using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Condition/DefualtToScan")]
public class DefaultToScan : Condition
{
    public override bool Check()
    {
        return character.functionTracks[0].clip.GetType() != typeof(Scan);
    }
}
