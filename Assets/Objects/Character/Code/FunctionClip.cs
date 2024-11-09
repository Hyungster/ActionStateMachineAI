using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class FunctionClip : ScriptableObject
{
    protected Character character;
    public bool overridable = true;
    protected int myRowIndex = 0;
    protected List<ConditionCollection> conditionCollections;

    public virtual void Init(Character thisCharacter)
    {
        character = thisCharacter;

        if (character.debug)
        {
            Debug.Log(character.gameObject.name + " Entered Clip: " + this.GetType());
        }

        for (int i = 0; i < character.actionTransitionTable.clipRows.Count; i++)
        {
            if (character.actionTransitionTable.clipRows[i].GetType() == this.GetType())
            {
                myRowIndex = i;
                break;
            }
        }
        conditionCollections = new List<ConditionCollection>(character.actionTransitionTable.conditionsColumns);
        Start();
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void End()
    {

    }

    public virtual void CheckTransition()
    {
        for (int i = 0; i < conditionCollections.Count; i++)
        {
            if (conditionCollections[i] == null)
            {
                continue;
            }
            else if (CheckConditionCollection(conditionCollections[i]))
            {
                Debug.Log(conditionCollections[i].name);
                Type toClip = character.actionTransitionTable.table[myRowIndex, i];
                HandleTransition(toClip);
                return;
            }
        }
    }

    public virtual bool CheckConditionCollection(ConditionCollection collection)
    {
        for (int i = 0; i < collection.conditions.Count; i++)
        {
            if (!collection.conditions[i].Check()) return false;
        }
        return true;
    }

    public virtual void HandleTransition(Type toClip)
    {
        if (toClip.IsSubclassOf(typeof(Action)))
        {
            character.ChangeFunctionClip(toClip, "action");
        }
        else if (toClip.IsSubclassOf(typeof(State)))
        {
            character.ChangeFunctionClip(toClip, "state");
        }
        else if (toClip.IsSubclassOf(typeof(Status)))
        {
            character.ChangeFunctionClip(toClip, "status");
        }
    }

    public virtual IEnumerator EndWithDuration(float duration)
    {
        for (float now = 0f; now < duration; now += Time.deltaTime)
        {
            yield return null;
        }
        CheckTransition();
    }

    public virtual void FixedTransition(Type t)
    {
        HandleTransition(t);
    }
}

public abstract class Action : FunctionClip
{
    public float duration;

    public override void End()
    {
        CheckTransition();
    }
}

public abstract class State : FunctionClip
{
    public override void Update()
    {
        base.Update();
        CheckTransition();
    }
}

public abstract class Status : FunctionClip
{
    public float duration;
}


