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
        float desiredAngle = Mathf.Atan2(character.targetDirection.y, character.targetDirection.x) * Mathf.Rad2Deg;

        Quaternion desiredQuaternion = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
        GameObject hitboxInstance = Instantiate(character.coneSlashHurtBox, character.transform.position, desiredQuaternion);
        character.coneSlashEffect.transform.rotation = desiredQuaternion;
        character.coneSlashEffect.transform.position = character.transform.position;

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
        ConeSlashEffect effect = character.coneSlashEffect;

        effect.brightness = effect.startBrightness;
        effect.sharpness = 1;
        effect.ringWidth1 = effect.startRingWidth1;
        effect.ringWidth1 = effect.startRingWidth2;

        float duration = character.beatDuration * 2 * 4/5;
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float factor = time / duration;
            float decayFactor = Mathf.Clamp01(Mathf.Pow(1 - factor, 4));
            effect.phase = Mathf.Lerp(effect.minPhase, effect.maxPhase, decayFactor);
            effect.ringWidth1 = Mathf.Lerp(1, effect.startRingWidth1, factor);
            effect.ringWidth2 = Mathf.Lerp(1, effect.startRingWidth2, factor);
            yield return null;
        }

        duration = character.beatDuration;
        effect.brightness = effect.maxBrightness;
        effect.sharpness = effect.maxSharpness;
        effect.ringWidth1 = effect.endRingWidth1;
        effect.ringWidth2 = effect.endRingWidth2;

        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float factor = time / duration;
            float decayFactor = Mathf.Clamp01(1 / Mathf.Pow(1 - factor, -4));
            effect.phase = Mathf.Lerp(1.1f, effect.minPhase, decayFactor);
            effect.brightness = Mathf.Lerp(1, 0, -decayFactor + 1);
            yield return null;
        }
        effect.brightness = 0;
        yield return null;
    }
}
