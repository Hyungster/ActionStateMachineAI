using UnityEngine;
using System.Collections.Generic;
using System.Collections;


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
                    character.charactersScanned.Add(otherCharacter);
                    //if (character.debug) Debug.Log(otherCharacter);
                }
            }
        }
        if (character.charactersScanned.Count == 1)
        {
            character.targetLocation = character.charactersScanned[0].transform.position;
        }
        End();
    }
}