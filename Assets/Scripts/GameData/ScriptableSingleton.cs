using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;

public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        static T s_Instance;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                    CreateAndLoad();
                return s_Instance;
            }
        }

        protected ScriptableSingleton()
        {
            if (s_Instance != null)
            {
                Debug.LogError("Singleton already exists!");
            }
            else
            {
                s_Instance = this as T;
                Assert.IsFalse(s_Instance == null);
            }
        }

        static void CreateAndLoad()
        {
            Assert.IsTrue(s_Instance == null);

            if (s_Instance == null)
            {
                // Resources.LoadAll<T>("/");
                var inst = CreateInstance<T>() as ScriptableSingleton<T>;
                Assert.IsFalse(inst == null);
                inst.hideFlags = HideFlags.HideAndDontSave;
            }

            Assert.IsFalse(s_Instance == null);
        }

    }
