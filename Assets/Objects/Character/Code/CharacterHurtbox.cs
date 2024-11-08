using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour
{
    public Character character;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        character.OnHurtboxTriggerEntered(collider);
    }
}
