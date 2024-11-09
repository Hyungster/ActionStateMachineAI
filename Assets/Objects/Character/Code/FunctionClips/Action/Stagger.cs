using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagger : Action
{

    public override void Start()
    {
        base.Start();
        character.StartCoroutine(StaggerSequence());
    }

    public IEnumerator StaggerSequence()
    {
        float duration = 0.375f;
        Vector2 startingPos = character.transform.position;
        Vector2 targetPos = startingPos + (startingPos - (Vector2)character.hitCharacter.transform.position).normalized * 5;
        //Debug.DrawLine(startingPos, targetPos, Color.red, 5);

        SpriteRenderer sr = character.visualObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        Color initialColor = sr.color;
        Color damageColor = Color.red;

        
        for (float time = 0; time <= duration; time += Time.deltaTime)
        {
            float factor = time / duration;

            factor = Mathf.Clamp01(factor);
            character.transform.position = Vector2.Lerp(startingPos, targetPos, factor);
            float yPos = Mathf.Sqrt(.25f - Mathf.Pow(factor - 0.5f, 2));
            character.visualObject.transform.localPosition = new Vector3(0, yPos * 1.5f, 0);

            sr.color = Color.Lerp(initialColor, damageColor, Mathf.Clamp(1 - factor, 0, 0.3f));

            yield return null;
        }
        if (!character.alive)
        {
            sr.color = new Color(1, 1, 1, 0.2f);
        }
        character.visualObject.transform.localPosition = Vector3.zero;
        FixedTransition(typeof(Scan));
    }
}
