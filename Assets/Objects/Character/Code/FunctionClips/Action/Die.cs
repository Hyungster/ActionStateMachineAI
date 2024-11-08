using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Die : Action
{
    public override void Start()
    {
        base.Start();
        character.alive = false;
    }
}
