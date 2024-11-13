using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSlash : Action
{
    List<CharacterHurtbox> alreadyHit = new();

    public override void Start()
    {
        base.Start();
        character.StartCoroutine(ConeSlashSequence());
    }

    public IEnumerator ConeSlashSequence()
    {
        float duration = character.beatDuration * 2;
        Quaternion desiredAngle = Quaternion.AngleAxis(Vector3.Angle(Vector3.right, character.targetDirection), Vector3.forward);
        GameObject hitboxInstance = Instantiate(character.coneSlashHurtBox, character.transform.position, desiredAngle);
        character.ConeSlashEffect.transform.rotation = desiredAngle;

        Rigidbody2D hitboxRB = hitboxInstance.GetComponent<Rigidbody2D>();

        character.StartCoroutine(VFX());

        yield return new WaitForSeconds(duration); // windup

        Collider2D[] hitColliders = new Collider2D[10];
        if (0 < hitboxRB.OverlapCollider(character.hurtboxFilter, hitColliders))
        {
            foreach (Collider2D collider in hitColliders)
            {
                if (collider == null) break;
                CharacterHurtbox other = collider.gameObject.GetComponent<CharacterHurtbox>();
                if (other != null && other != character.hurtbox && !alreadyHit.Contains(other))
                {
                    other.Hit(1, character, character.coneSlashHitEffect);
                    alreadyHit.Add(other);
                }
            }
        }
        Destroy(hitboxRB.gameObject);
        FixedTransition(typeof(Scan));
    }

    private IEnumerator VFX()
    {
        ConeSlashEffect effect = character.ConeSlashEffect;

        float duration = character.beatDuration * 2;
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float factor = time / duration;
            float decayFactor = Mathf.Pow(1 - factor, 8);
            effect.phase = Mathf.Lerp(effect.minPhase, effect.maxPhase, decayFactor);
            yield return null;
        }
        yield return null;
    }
}
