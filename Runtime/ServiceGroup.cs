using System;
using System.Collections.Generic;
using UnityEngine;

namespace UExtension.Bootstrap
{
    [Serializable]
    public class ServiceGroup
    {
        [field: SerializeField]
        public int Priority { get; set; }

        [field: SerializeField]
        public List<ScriptableObject> Services { get; set; } = new();
    }
}