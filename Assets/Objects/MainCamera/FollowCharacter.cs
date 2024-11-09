using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Character character;
    public Vector2 offset;

    private void Update()
    {
        Vector3 characterPos = character.transform.position;
        transform.position = new Vector3(characterPos.x + offset.x, characterPos.y + offset.y, transform.position.z);
    }
}
