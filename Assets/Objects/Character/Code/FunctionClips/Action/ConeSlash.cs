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
        GameObject hitboxInstance = Instantiate(character.coneSlashHurtBox, character.transform.position, Quaternion.AngleAxis(Vector3.Angle(Vector3.right, character.targetDirection), Vector3.back));
        Rigidbody2D hitboxRB = hitboxInstance.GetComponent<Rigidbody2D>();

        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            yield return null;
        }

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
}
