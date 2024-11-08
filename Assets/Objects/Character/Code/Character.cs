using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine.TextCore.Text;
public class Character : MonoBehaviour
{
    [HideInInspector] GameObject visualObject;

    public bool alive = true;
    public bool debug = false;

    [HideInInspector] public Vector2 targetDirection = Vector2.up;
    public Vector2 targetLocation = Vector2.zero;
    public Character targetCharacter = null;


    public CircleCollider2D scanCollider;
    public ContactFilter2D hurtboxFilter;
    [HideInInspector] public List<Character> charactersScanned;

    //character hurtboxes
    public GameObject coneSlashHurtBox;

    [HideInInspector] public int initialActionIndex = 0;

    void Start()
    {
        visualObject = transform.Find("Body/Visual").gameObject;

        List<FunctionClip> functionClips = new List<FunctionClip>(); //temp list to hold functionclips

        List<Type> allFunctionClipTypes = CharacterUtil.GetAllFunctionClipTypes();
        List<Type> allActionClipTypes = CharacterUtil.GetAllActionTypes();


        foreach (ConditionCollection collection in actionTransitionTable.conditionsColumns)
        {
            if (collection == null) break;
            foreach (Condition condition in collection.conditions)
            {
                condition.Init(this);
            }
        }

        

        //add all Actions to rows
        int actionCount = actionTransitionTable.clipRows.Count;
        for (int i = 0; i < actionCount; i++)
        {
            functionClips.Add(ScriptableObject.CreateInstance(allActionClipTypes[actionTransitionTable.clipChoiceIndices[i]]) as FunctionClip);
            actionTransitionTable.clipRows = new List<FunctionClip>(functionClips);
        }
        //add all Actions to table
        for (int i = 0; i < actionTransitionTable.clipRows.Count; i++)
        {
            for (int j = 0; j < actionTransitionTable.conditionsColumns.Count; j++)
            {
                actionTransitionTable.table[i, j] = allFunctionClipTypes[actionTransitionTable.indexTableRows[i].columns[j]];
                //if (debug) Debug.Log(actionTransitionTable.table[i, j].Name);
            }
        }

        //set initial function clips
        ChangeFunctionClip(allActionClipTypes[initialActionIndex], "action");
    }
    
    void Update()
    {
        if (debug)
        {
            if (charactersScanned.Count > 0)
            {
                foreach (Character character in charactersScanned)
                {
                    Vector2 diff = character.transform.position - transform.position;
                    diff.Normalize();
                    Debug.DrawLine(transform.position, (Vector2)transform.position + diff, Color.red, 0.2f);
                }
            }
            Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + targetDirection, Color.green, 0.5f);
        }
        UpdateTracks();
    }

    private void FixedUpdate()
    {
        FixedUpdateTracks();
    }
    
    // 0 [action], 1 [state], 2 [status]
    [HideInInspector] public FunctionTrack[] functionTracks = {new FunctionTrack("action"), new FunctionTrack("state"), new FunctionTrack("status")};
    [HideInInspector] public TransitionTable actionTransitionTable = new TransitionTable();
    [HideInInspector] public TransitionTable stateTransitionTable = new TransitionTable();
    [HideInInspector] public TransitionTable statusTransitionTable = new TransitionTable();

    private void UpdateTracks()
    {
       foreach (FunctionTrack track in functionTracks)
        {
            track.Update();
        }
    }

    private void FixedUpdateTracks()
    {
        foreach (FunctionTrack track in functionTracks)
        {
            track.FixedUpdate();
        }
    }

    [Serializable]
    public class TransitionTable //lookup table to see which clip to transition to given a clip + condition
    {
        public int[] clipChoiceIndices = new int[50];
        public List<FunctionClip> clipRows = new List<FunctionClip>();
        public List<ConditionCollection> conditionsColumns = new List<ConditionCollection>() { null };

        public IndexTableColumns[] indexTableRows = new IndexTableColumns[50];
        [Serializable]
        public class IndexTableColumns
        {
            public int[] columns = new int[50];
        }
        public Type[,] table = new Type[50, 50];
    }

    

    
    //changing out the track's FunctionClip

    public void ChangeFunctionClip(FunctionClip nextClip, string trackName)
    {
        foreach (FunctionTrack track in functionTracks)
        {
            if (trackName.Equals(track.name))
            {
                if (track.clip.overridable || track.clip == null)
                {
                    Type clipType = nextClip.GetType();
                    track.clip = ScriptableObject.CreateInstance(clipType) as FunctionClip;
                    track.clip.Init(this);
                }
                break;
            }
        }
    }

    public void ChangeFunctionClip<T>(string trackName) where T : FunctionClip, new()
    {
        foreach (FunctionTrack track in functionTracks)
        {
            if (trackName.Equals(track.name))
            {
                if (track.clip.overridable || track.clip == null)
                {
                    track.clip = new T();
                    track.clip.Init(this);
                }
                break;
            }
        }
    }

    public void ChangeFunctionClip(System.Type functionClipType, string trackName)
    {
        foreach (FunctionTrack track in functionTracks)
        {
            if (trackName.Equals(track.name))
            {
                if (track.clip == null || track.clip.overridable)
                {
                    track.clip = ScriptableObject.CreateInstance(functionClipType) as FunctionClip;
                    track.clip.Init(this);
                }
                break;
            }
        }
    }

    //collision handler functions: called by children physics

    public void OnHurtboxTriggerEntered(Collider2D collider)
    {

    }

    public void OnNavTriggerEntered(Collider2D collider)
    {

    }

    public void OnScanTriggerEntered(Collider2D collider)
    {
        charactersScanned.Add(collider.gameObject.GetComponent<Character>());
    }
    public void OnScanTriggerExited(Collider2D collider)
    {
        charactersScanned.Remove(collider.gameObject.GetComponent<Character>());
    }
}

//functionclip and track classes
public class FunctionTrack
{
    public string name;
    public bool mute = false;
    public FunctionClip clip;
    public FunctionTrack(string myName) => name = myName;

    public void Update()
    {
        if (!mute && clip != null)
        {
            clip.Update();
        }
    }

    public void FixedUpdate()
    {
        if (!mute && clip != null)
        {
            clip.FixedUpdate();
        }
    }
}



