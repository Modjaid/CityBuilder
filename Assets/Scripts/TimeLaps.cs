
using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEditor;
using UnityEngine;
using Beh = Behaviours.StepBehaviour;

namespace Managers
{
    public class TimeLaps : MonoBehaviour
    {
        [TextArea(0, 1000)] [SerializeField] private string _log;
        [SerializeField] private bool _pause = false;
        public static TimeLaps Instance = null;
        public float DayDelay = 3;
        public int CurrentDay = 0;
        public event Action OnNextStep;

        private List<Beh> _behaviours;
        private Queue<Beh> _destroyBehs;
        private Queue<Beh> _addBehs;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            _behaviours = new List<Beh>();
            _destroyBehs = new Queue<Beh>();
            _addBehs = new Queue<Beh>();
        }

        public void Start()
        {
            Upd();
            StartCoroutine(DebugLog());
        }

        public void SetPause(bool isPause)
        {
            _pause = !isPause;
        }

        private async UniTask Upd()
        {
            float currentDelay;
            while (true)
            {
                currentDelay = DayDelay;
                while (currentDelay > 0)
                {
                    await UniTask.WaitUntil(() => !_pause);
                    await UniTask.Yield();
                    currentDelay -= Time.deltaTime;
                }

                CurrentDay++;
                OnNextStep?.Invoke();

                UpdBehCollections();
                foreach (var beh in _behaviours)
                {
                    if (beh.IsExecute)
                    {
                        await beh.Execute();
                    }
                }
            }
        }

        private void UpdBehCollections()
        {
            while (_addBehs.Count > 0)
            {
                _behaviours.Add(_addBehs.Dequeue());
            }

            while (_destroyBehs.Count > 0)
            {
                _behaviours.Remove(_destroyBehs.Dequeue());
            }
        }

        public void AddBehaviour(Beh behaviour)
        {
            _addBehs.Enqueue(behaviour);
        }

        public void RemoveBehaviour(Beh behaviour)
        {
            if (_behaviours.Contains(behaviour))
            {
                _destroyBehs.Enqueue(behaviour);
            }
        }

        public T GetBehaviour<T>() where T : Beh
        {
            T result = null;
            foreach (var beh in _behaviours)
            {
                if (beh is T)
                {
                    return result;
                }
            }

            return result;
        }

        private IEnumerator DebugLog()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                _log = "";
                foreach (var beh in _behaviours)
                {
                    _log += $"{beh.Executer.name} : {beh} \n";
                }
            }
        }


#if UNITY_EDITOR
        [MenuItem("SceneObjects/TimeLaps")]
        public static void InstantiateEditor()
        {
            var go = new GameObject("[TimeLaps]");
            go.AddComponent<TimeLaps>();
        }
#endif
    }
}
