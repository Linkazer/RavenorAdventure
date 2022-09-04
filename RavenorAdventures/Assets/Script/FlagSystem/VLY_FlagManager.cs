using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_FlagManager : RVN_Singleton<VLY_FlagManager>
{
    [SerializeField] private VLY_FlagList flagList;

    private static Dictionary<string, int> flags = new Dictionary<string, int>();

    private static Dictionary<string, List<Action<int>>> incrementalFlagListeners = new Dictionary<string, List<Action<int>>>();
    private static Dictionary<string, List<Action>> triggerFlagListeners = new Dictionary<string, List<Action>>();

    private Dictionary<string, List<Action>> triggerToRemove = new Dictionary<string, List<Action>>();
    private Dictionary<string, List<Action<int>>> incrementalToRemove = new Dictionary<string, List<Action<int>>>();

    [SerializeField] private List<CPN_IncrementFlagListener> flagListeners;

    protected override void OnAwake()
    {
        DontDestroyOnLoad(gameObject);

        //Ajoute tous les flags existant
        flags.Clear();
        incrementalFlagListeners.Clear();
        triggerFlagListeners.Clear();

        foreach (string flagName in flagList.IncrementalsFlags)
        {
            flags.Add(flagName, 0);

            //Debug.Log(flagName);
            incrementalFlagListeners.Add(flagName, new List<Action<int>>());
            
        }

        foreach(string flagName in flagList.TriggerFlags)
        {
            triggerFlagListeners.Add(flagName, new List<Action>());
        }

        foreach(CPN_IncrementFlagListener lst in flagListeners)
        {
            lst.enabled = true;
        }
    }

    private void LateUpdate()
    {
        foreach (KeyValuePair<string, List<Action>> tfl in triggerToRemove)
        {
            foreach (Action act in tfl.Value)
            {
                triggerFlagListeners[tfl.Key].Remove(act);
            }
        }

        foreach (KeyValuePair<string,List<Action<int>>> tfl in incrementalToRemove)
        {
            foreach(Action<int> act in tfl.Value)
            {
                incrementalFlagListeners[tfl.Key].Remove(act);
            }
        }

        triggerToRemove = new Dictionary<string, List<Action>>();
        incrementalToRemove = new Dictionary<string, List<Action<int>>>();
    }

    public static void AddIncrementFlagListener(string flagName, Action<int> callback)
    {

        if(!incrementalFlagListeners[flagName].Contains(callback))
        {
            incrementalFlagListeners[flagName].Add(callback);
        }
    }
    public static void RemoveIncrementFlagListener(string flagName, Action<int> callback)
    {
        if (incrementalFlagListeners[flagName].Contains(callback))
        {
            if(instance.incrementalToRemove.ContainsKey(flagName))
            {
                instance.incrementalToRemove[flagName].Add(callback);
            }
            else
            {
                instance.incrementalToRemove.Add(flagName, new List<Action<int>>());
                instance.incrementalToRemove[flagName].Add(callback);
            }
            //incrementalFlagListeners[flagName].Remove(callback);
        }
    }

    public static void AddTriggerFlagListener(string flagName, Action callback)
    {
        if (!triggerFlagListeners[flagName].Contains(callback))
        {
            triggerFlagListeners[flagName].Add(callback);
        }
    }
    public static void RemoveTriggerFlagListener(string flagName, Action callback)
    {
        if (triggerFlagListeners[flagName].Contains(callback))
        {
            if (instance.triggerToRemove.ContainsKey(flagName))
            {
                instance.triggerToRemove[flagName].Add(callback);
            }
            else
            {
                instance.triggerToRemove.Add(flagName, new List<Action>());
                instance.triggerToRemove[flagName].Add(callback);
            }
            //triggerFlagListeners[flagName].Remove(callback);
        }
    }


    public static void IncrementFlagValue(string flagName, int incrementValue)
    {
        if(flags.ContainsKey(flagName))
        {
            flags[flagName] += incrementValue;

            foreach(Action<int> act in incrementalFlagListeners[flagName])
            {
                act?.Invoke(flags[flagName]);
            }
        }
    }

    public static int GetFlagValue(string flagName)
    {
        if (flags.ContainsKey(flagName))
        {
            return flags[flagName];
        }
        return -1;
    }

    public static void TriggerFlag(string triggerName)
    {
        foreach (Action act in triggerFlagListeners[triggerName])
        {
            
            act?.Invoke();
        }
    }
}
