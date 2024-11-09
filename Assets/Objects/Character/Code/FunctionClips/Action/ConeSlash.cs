using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSlash : Action
{
    float hitMoment = 0.5f;

    public override void Start()
    {
        base.Start();
        character.StartCoroutine(ConeSlashSequence());
    }

    public IEnumerator ConeSlashSequence()
    {
        duration = 1f;
        bool hitboxUsed = false;
        GameObject hitboxInstance = Instantiate(character.coneSlashHurtBox, character.transform.position, Quaternion.AngleAxis(Vector3.Angle(Vector3.right, character.targetDirection), Vector3.back));
        Rigidbody2D hitboxRB = hitboxInstance.GetComponent<Rigidbody2D>();
        for (float time = 0f; time <= duration; time += Time.deltaTime)
        {
            if (time > hitMoment && !hitboxUsed)
            {
                hitboxUsed = true;
                
                Collider2D[] hitColliders = new Collider2D[10];
                if (0 < hitboxRB.OverlapCollider(character.hurtboxFilter, hitColliders))
                {
                    foreach (Collider2D collider in hitColliders)
                    {
                        if (collider == null) break;
                        CharacterHurtbox other = collider.gameObject.GetComponent<CharacterHurtbox>();
                        if (other != null && other != character.hurtbox)
                        {
                            other.Hit(1, character);
                        }
                    }
                }
                Destroy(hitboxRB.gameObject);
                
            }
            yield return null;
        }
        FixedTransition(typeof(Scan));
    }
}
