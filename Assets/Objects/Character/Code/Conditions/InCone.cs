using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Condition/InCone")]
public class InCone : Condition
{
    public int minCharacters = 2;
    public float coneAngle = 60f;

    public override bool Check()
    {
        if (character.charactersScanned.Count != 0)
        {
            List<float> angles = new List<float>();
            foreach (Character other in character.charactersScanned)
            {
                Vector2 diff = other.transform.position - character.transform.position;
                float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                angles.Add(angle);
            }
            List<float[]> numInCone = new List<float[]>();
            foreach (float angle in angles)
            {
                int num = 0;
                float averageAngle = 0;
                foreach (float otherAngles in angles)
                {
                    if (angle <= otherAngles && otherAngles <= angle + coneAngle)
                    {
                        averageAngle += otherAngles;
                        num++;
                    }
                }
                numInCone.Add(new float[2] { num, averageAngle / num });
            }
            int maxNum = 0;
            int maxIndex = 0;
            for (int i = 0; i < numInCone.Count; i++)
            {
                if (numInCone[i][0] > maxNum)
                {
                    maxNum = (int) numInCone[i][0];
                    maxIndex = i;
                }
            }
            float targetAngle = numInCone[maxIndex][1];
            if (maxNum >= minCharacters)
            {
                float offset = coneAngle / (maxNum * 2);
                targetAngle += Random.Range(-offset, offset);
                character.targetDirection = new Vector2(Mathf.Cos(targetAngle * Mathf.Deg2Rad), Mathf.Sin(targetAngle * Mathf.Deg2Rad));
                return true;
            }
        }
        return false;
    }
}
