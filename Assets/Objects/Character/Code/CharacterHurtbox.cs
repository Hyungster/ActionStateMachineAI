using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour
{
    public Character character;
    public Rigidbody2D rb;

    public void Hit(int damage, Character sourceCharacter)
    {
        character.TakeDamage(damage, sourceCharacter);
    }

    public void Hit(Character sourceCharacter)
    {
        character.TakeDamage(1, sourceCharacter);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        character.OnHurtboxTriggerEntered(collider);
    }
}