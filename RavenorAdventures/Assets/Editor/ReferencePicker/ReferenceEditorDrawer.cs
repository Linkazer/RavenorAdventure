using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReferenceEditor))]
public class ReferenceEditorDrawer : PropertyDrawer
{
    public class TypeCache
    {
        public readonly Type[] types;
        public readonly GUIContent[] names;

        public TypeCache(Type[] nTypes, GUIContent[] nNames)
        {
            types = nTypes;
            names = nNames;
        }
    }

    static protected Dictionary<Type, TypeCache> typeCache = new Dictionary<Type, TypeCache>();

    private TypeCache cache;

    private int index;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type type = ((ReferenceEditor)attribute).type;

        cache = GetOrCreateCache(type);

        index = GetType(property.managedReferenceFullTypename);

        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(new Rect(position.position, new Vector2(position.width, 18)), label, index, cache.names);

        if(EditorGUI.EndChangeCheck())
        {
            property.managedReferenceValue = SetType();
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    private TypeCache GetOrCreateCache(Type type)
    {
        if(!typeCache.ContainsKey(type))
        {
            Type[] types = type.Assembly.GetTypes().Where(t => t != type && type.IsAssignableFrom(t) && !t.IsAbstract).ToArray();
            List<GUIContent> names = new List<GUIContent>(types.Length + 1);
            names.Add(new GUIContent("Null"));
            names.AddRange(new List<GUIContent>(types.Select(x => new GUIContent(x.Name))));

            typeCache.Add(type, new TypeCache(types, names.ToArray()));
        }

        return typeCache[type];
    }

    private int GetType(string type)
    {
        for(int i = 0; i < cache.types.Length; i++)
        {
            if (CompareType(cache.types[i], type))
            {
                return i + 1;
            }
        }

        return 0;
    }

    private bool CompareType(Type type, string name)
    {
        string[] data = name.Split(' ');

        return type.Assembly.GetName().Name == data[0] && type.FullName == data[1];
    }

    private object SetType()
    {
        if(index == 0)
        {
            return null;
        }

        ConstructorInfo constructor = cache.types[index - 1].GetConstructor(new Type[] { });
        return constructor.Invoke(new object[] { });
    }
}
