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
        yield return new WaitForSeconds(2);
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
                    TryAddFront(otherCharacter);
                    continue;
                }
            }
        }

        if (character.charactersScanned.Count > 0)
        {
            character.targetLocation = character.charactersScanned[0].transform.position;
        }

        /*
        foreach (Character other in character.charactersScanned)
        {
            Debug.Log((other.transform.position - character.transform.position).sqrMagnitude);
        }
        */

        End();
    }



    private void TryAddFront(Character other)
    {
        foreach (Character c in character.charactersScanned)
        {
            float mySqrDist = (other.transform.position - character.transform.position).sqrMagnitude;
            float otherSqrDist = (c.transform.position - character.transform.position).sqrMagnitude;
            if (mySqrDist > otherSqrDist)
            {
                character.charactersScanned.Add(other);
                return;
            }
        }
        character.charactersScanned.Insert(0, other);
        return;
    }
}