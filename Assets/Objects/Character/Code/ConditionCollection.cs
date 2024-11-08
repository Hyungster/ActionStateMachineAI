using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ConditionCollection")]
public class  ConditionCollection : ScriptableObject
{
    public List<Condition> conditions = new List<Condition>();

}

public abstract class Condition : ScriptableObject
{
    protected Character character;

    public virtual void Init(Character thisCharacter)
    {
        character = thisCharacter;
    }

    public abstract bool Check();
}

public abstract class Compare2 : Condition
{
    public Compare comparison;

    public enum Compare
    {
        EQUAL,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,
    }

    public override bool Check()
    {
        throw new NotImplementedException();
    }
}
