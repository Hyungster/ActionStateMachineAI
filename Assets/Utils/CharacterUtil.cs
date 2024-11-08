using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

public static class CharacterUtil
{
    public static List<System.Type> GetAllFunctionClipTypes()
    {
        List<Type> types = Assembly.GetAssembly(typeof(FunctionClip)).GetTypes().Where(type => type.IsSubclassOf(typeof(FunctionClip))).ToList();
        for (int i = 0; i < types.Count; i++)
        {
            if ((types[i] == typeof(Action)) || (types[i] == typeof(State)) || (types[i] == typeof(Status)))
            {
                types.RemoveAt(i);
                i--;
            }
        }
        return types;
    }

    public static List<System.Type> GetAllActionTypes()
    {
        return Assembly.GetAssembly(typeof(Action)).GetTypes().Where(type => type.IsSubclassOf(typeof(Action))).ToList();
    }

    public static List<System.Type> GetAllStateTypes()
    {
        return Assembly.GetAssembly(typeof(State)).GetTypes().Where(type => type.IsSubclassOf(typeof(State))).ToList();
    }

    public static List<System.Type> GetAllStatusTypes()
    {
        return Assembly.GetAssembly(typeof(Status)).GetTypes().Where(type => type.IsSubclassOf(typeof(Status))).ToList();
    }
}
