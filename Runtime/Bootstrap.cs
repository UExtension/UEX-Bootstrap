using System.Collections.Generic;
using UnityEngine;

namespace UExtension.Bootstrap
{
    public class Bootstrap : ScriptableObject
    {
        [field: SerializeField]
        public bool ActiveLogging { get; set; }

        [field: SerializeField]
        public List<ServiceGroup> ServiceGroups { get; set; } = new();

        private void OnEnable()
        {
            BootStrap();
        }

        private void BootStrap()
        {
            ServiceGroups.Sort((a, b) => a.Priority.CompareTo(b.Priority));

            foreach (var group in ServiceGroups)
            {
                var log = $"Bootstrapping services of priority {group.Priority}:";
                foreach (var service in group.Services)
                {
                    log += $"\n\t- {service.name}";
                    if (service is IBootstrapService bootstrapService)
                    {
                        bootstrapService.Bootstrap();
                    }
                }

                if (ActiveLogging)
                {
                    Debug.Log(log);
                }
            }
        }
    }
}