using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : Action
{
    public override void Start()
    {
        base.Start();
        character.alive = false;
        Debug.Log("I die today");
    }
}
