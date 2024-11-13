using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterHurtbox : MonoBehaviour
{
    public Character character;
    public Rigidbody2D rb;

    public void Hit(int damage, Character sourceCharacter, VisualEffectAsset vfx)
    {
        character.TakeDamage(damage, sourceCharacter);

        GameObject hitVFX = new GameObject();
        Vector3 vfxPos = character.transform.position;
        vfxPos += (sourceCharacter.transform.position - vfxPos).normalized / 4;
        hitVFX.transform.position = vfxPos;

        VisualEffect vfxComponent = hitVFX.AddComponent<VisualEffect>();
        vfxComponent.visualEffectAsset = vfx;
        vfxComponent.Play();

        Destroy(hitVFX, 1);
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
