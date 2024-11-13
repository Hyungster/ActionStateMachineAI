using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Wait : Action
{
    public override void Start()
    {
        base.Start();
        character.StartCoroutine(WaitToScan());
    }

    private IEnumerator WaitToScan()
    {
        yield return new WaitForSeconds(4);
        FixedTransition(typeof(Scan));
    }
}

public class Scan : Action
{
    public override void Start()
    {
        base.Start();
        character.charactersScanned.Clear();
        List<Collider2D> list = new List<Collider2D>();
        Physics2D.OverlapCircle(character.transform.position, character.scanCollider.radius, character.hurtboxFilter, list);
        foreach (Collider2D c in list)
        {
            if (c.gameObject.CompareTag("Character"))
            {
                CharacterHurtbox hurtbox = c.gameObject.GetComponent<CharacterHurtbox>();
                Character otherCharacter = hurtbox.character;
                if (otherCharacter != null && otherCharacter != character && otherCharacter.alive)
                {
                    if (character.charactersScanned.Count > 0)
                    {
                        if (TryAddFront(otherCharacter)) continue;
                        else if (TryAddBack(otherCharacter)) continue;
                    }
                    else
                    {
                        character.charactersScanned.Add(otherCharacter);
                        continue;
                    }
                }
            }
        }

        if (character.charactersScanned.Count > 0)
        {
            character.targetLocation = character.charactersScanned[0].transform.position;
        }

        End();
    }



    private bool TryAddFront(Character other)
    {
        if (character.charactersScanned.Count > 0)
        {
            float shortestSqrDist = (character.charactersScanned[0].transform.position - character.transform.position).sqrMagnitude;
            float mySqrDist = (other.transform.position - character.transform.position).sqrMagnitude;
            if (mySqrDist <= shortestSqrDist)
            {
                character.charactersScanned.Insert(0, other);
                return true;
            }
            return false;
        }
        return false;
    }

    private bool TryAddBack(Character other)
    {
        if (character.charactersScanned.Count > 0)
        {
            float longestSqrDist = (character.charactersScanned[character.charactersScanned.Count - 1].transform.position - character.transform.position).sqrMagnitude;
            float mySqrDist = (other.transform.position - character.transform.position).sqrMagnitude;
            if (mySqrDist >= longestSqrDist)
            {
                character.charactersScanned.Insert(character.charactersScanned.Count - 1, other);
            }
            return false;
        }
        return false;
    }
}