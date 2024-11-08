using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Scan : Action
{
    public override void Start()
    {
        base.Start();
        List<Collider2D> list = new List<Collider2D>();
        Physics2D.OverlapCircle(character.transform.position, character.scanCollider.radius, character.hurtboxFilter, list);
        foreach (Collider2D c in list)
        {
            if (c.gameObject.CompareTag("Character"))
            {
                CharacterHurtbox hitbox = c.gameObject.GetComponent<CharacterHurtbox>();
                Character otherCharacter = hitbox.character;
                if (otherCharacter != null && otherCharacter != character && otherCharacter.alive)
                {
                    character.charactersScanned.Add(otherCharacter);
                    //if (character.debug) Debug.Log(otherCharacter);
                }
            }
        }
        End();
    }
}