using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Condition/TargetDistance")]
public class TargetDistance : Compare2
{
    public float distance = 1;

    public override bool Check()
    {
        if (character.charactersScanned.Count < 1) return false;

        Vector2 diff = character.transform.position - character.charactersScanned[0].transform.position;
        float sqrDistance = distance * distance;
        float diffSqrDistance = diff.sqrMagnitude;

        switch (comparison)
        {
            case Compare.EQUAL:
                return sqrDistance == diffSqrDistance;
            
            case Compare.GREATER_THAN:
                return diffSqrDistance > sqrDistance;

            case Compare.LESS_THAN:
                return diffSqrDistance < sqrDistance;

            case Compare.GREATER_THAN_OR_EQUAL:
                return diffSqrDistance >= sqrDistance;

            case Compare.LESS_THAN_OR_EQUAL:
                return diffSqrDistance <= sqrDistance;
        }
        return false;
    }
}
