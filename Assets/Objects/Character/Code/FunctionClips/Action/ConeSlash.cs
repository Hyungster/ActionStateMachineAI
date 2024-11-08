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
        bool hurtboxUsed = false;
        Rigidbody2D hurtboxRB = Instantiate(character.coneSlashHurtBox, character.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            if (time > hitMoment && !hurtboxUsed)
            {
                hurtboxUsed = true;
                Collider2D[] hitColliders = new Collider2D[10];
                if (0 < hurtboxRB.OverlapCollider(character.hurtboxFilter, hitColliders))
                {
                    foreach (Collider2D collider in hitColliders)
                    {
                        
                        if (collider == null) break;
                        Debug.Log("ahh");
                        Character other = collider.gameObject.GetComponent<Character>();
                        if (other != null && other != character)
                        {
                            other.ChangeFunctionClip<Die>("action");
                            if (character.debug) Debug.Log("Setting a character to Stagger!!");
                        }
                    }
                }
                Destroy(hurtboxRB.gameObject);
                
            }
            yield return null;
        }
        FixedTransition(typeof(Scan));
    }
}
