using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

public class UILibraryManager
{
    private const int MenuPriority = -50;
    private const string PrefabHolderPath = "Assets/Prefabs/UI/Utilitaires/UIPrefabHolder.asset";

    private static UIPrefabHolder LocatePrefabHolder() => AssetDatabase.LoadAssetAtPath<UIPrefabHolder>(PrefabHolderPath);

    private static void SafeInstatiate(Func<UIPrefabHolder, GameObject> itemSelector)
    {
        UIPrefabHolder prefabHolder = LocatePrefabHolder();

        if(!prefabHolder)
        {
            return;
        }

        var item = itemSelector(prefabHolder);
        var instance = PrefabUtility.InstantiatePrefab(item, Selection.activeTransform);

        Undo.RegisterCompleteObjectUndo(instance, $"Create {instance.name}");
        Selection.activeObject = instance;
    }

    [MenuItem("GameObject/UI/Ravenor/Text", priority = MenuPriority)]
    private static void CreateText()
    {
        SafeInstatiate(prefabHolder => prefabHolder.TextPrefab);
    }

    [MenuItem("GameObject/UI/Ravenor/Button", priority = MenuPriority)]
    private static void CreateButton()
    {
        SafeInstatiate(prefabHolder => prefabHolder.Button);
    }

    [MenuItem("GameObject/UI/Ravenor/ButtonWithText", priority = MenuPriority)]
    private static void CreateButtonWithText()
    {
        SafeInstatiate(prefabHolder => prefabHolder.ButtonWithText);
    }
}
#endif