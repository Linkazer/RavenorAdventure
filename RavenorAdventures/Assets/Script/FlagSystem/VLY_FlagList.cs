using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Flag List", menuName ="Valley/Create Flag List")]
public class VLY_FlagList : ScriptableObject
{
    [SerializeField] private List<string> incrementalFlag;
    public List<string> IncrementalsFlags => incrementalFlag;

    [SerializeField] private List<string> triggerFlag;
    public List<string> TriggerFlags => triggerFlag;
}
