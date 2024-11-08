using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Condition/CharactersScannedCount")]
public class CharactersScannedCount : Compare2
{
    public int scanCount = 0;
    

    public override bool Check()
    {
        if (comparison == Compare.EQUAL)
        {
            return scanCount == character.charactersScanned.Count;
        }
        else if (comparison == Compare.GREATER_THAN)
        {
            return character.charactersScanned.Count > scanCount;
        }
        else if (comparison == Compare.LESS_THAN)
        {
            return character.charactersScanned.Count < scanCount;
        }
        else if (comparison == Compare.GREATER_THAN_OR_EQUAL)
        {
            return character.charactersScanned.Count >= scanCount;
        }
        else if (comparison == Compare.LESS_THAN_OR_EQUAL)
        {
            return character.charactersScanned.Count <= scanCount;
        }
        return false;
    }
}
