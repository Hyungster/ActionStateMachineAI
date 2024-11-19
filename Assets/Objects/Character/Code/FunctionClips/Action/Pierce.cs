using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Pierce : Action
{
    List<CharacterHurtbox> alreadyHit = new();

    public override void Start()
    {
        base.Start();
        character.StartCoroutine(PierceSequence());
    }

    public IEnumerator PierceSequence()
    {

        
        float duration = character.beatDuration / 4;
        Vector2 startingPos = character.transform.position;
        Vector2 targetPos = character.charactersScanned[0].transform.position;
        Vector2 direction = (targetPos - startingPos);
        Vector2 normDirection = direction.normalized;

        targetPos += direction.normalized;

        RaycastHit2D[] hits = new RaycastHit2D[10];
        if (0 < character.hurtbox.rb.Cast(direction, character.hurtboxFilter, hits, direction.magnitude))
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null) break;
                CharacterHurtbox other = hit.collider.gameObject.GetComponent<CharacterHurtbox>();
                if (other != null && other != character.hurtbox && !alreadyHit.Contains(other))
                {
                    other.Hit(1, character, character.pierceHitEffect);
                    alreadyHit.Add(other);
                }
            }
        }

        Debug.Log(normDirection);
        Debug.DrawLine(character.transform.position, (Vector2)character.transform.position + normDirection, Color.red, 1);

        GameObject pierceObject = character.pierceEffect.gameObject;

        float desiredAngle = Mathf.Atan2(normDirection.y, normDirection.x) * Mathf.Rad2Deg;
        pierceObject.transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
        character.StartCoroutine(VFX());
        for (float time = 0; time <= duration; time += Time.deltaTime)
        {
            float factor = time / duration;

            float sqrFactor = Mathf.Clamp01(Mathf.Pow(factor, 3));

            character.transform.position = Vector2.Lerp(startingPos, targetPos, sqrFactor);
            yield return null;
        }

        duration = character.beatDuration * 3 / 4;
        yield return new WaitForSeconds(duration);

        FixedTransition(typeof(Scan));
        
    }

    private IEnumerator VFX()
    {
        float duration = character.beatDuration;
        for (float time = 0; time <= duration; time += Time.deltaTime)
        {
            float factor = time / duration;

            float upAndDownFactor = Mathf.Sin(6.28f * factor - 1.6f) / 2 + 0.5f;

            character.pierceEffect.phase = Mathf.Lerp(character.pierceEffect.minPhase, character.pierceEffect.maxPhase, upAndDownFactor);
            yield return null;
        }
    }
}