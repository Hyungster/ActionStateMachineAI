using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pounce : Action
{

    List<CharacterHurtbox> alreadyHit = new();
    public override void Start()
    {
        base.Start();
        character.StartCoroutine(PounceSequence());
    }

    public IEnumerator PounceSequence()
    {
        float duration = character.beatDuration;
        Vector2 startingPos = character.transform.position;
        Vector2 targetPos = character.targetLocation;

        character.SetTrailEmitterActive(true);

        for (float time = 0; time <= duration; time += Time.deltaTime)
        {
            float factor = time / duration;
            float cubeFactor = Mathf.Pow((1.5874f * (factor - 0.5f)), 3) + 0.5f;
            factor = Mathf.Lerp(factor, cubeFactor, 0.65f);

            factor = Mathf.Clamp01(factor);
            character.transform.position = Vector2.Lerp(startingPos, targetPos, factor );
            float yPos = Mathf.Sqrt(.25f - Mathf.Pow(factor - 0.5f, 2));
            character.visualObject.transform.localPosition = new Vector3(0, yPos * 1.5f, 0);

            Collider2D[] hitColliders = new Collider2D[10];
            if (0 < character.hurtbox.rb.OverlapCollider(character.hurtboxFilter, hitColliders))
            {
                foreach (Collider2D collider in hitColliders)
                {
                    if (collider == null) break;
                    CharacterHurtbox other = collider.gameObject.GetComponent<CharacterHurtbox>();
                    if (other != null && other != character.hurtbox && !alreadyHit.Contains(other))
                    {
                        other.Hit(1, character, character.pounceHitEffect);
                        alreadyHit.Add(other);
                    }
                }
            }

            yield return null;
        }
        character.visualObject.transform.localPosition = Vector3.zero;
        character.SetTrailEmitterActive(false);
        FixedTransition(typeof(Scan));
    }
}
