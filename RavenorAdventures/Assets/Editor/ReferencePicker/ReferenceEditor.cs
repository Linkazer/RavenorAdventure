using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ravenor.referencePicker
{
    public class ReferenceEditor : PropertyAttribute
    {
        public readonly Type type;

        public ReferenceEditor(Type nType)
        {
            type = nType;
        }
    }
}