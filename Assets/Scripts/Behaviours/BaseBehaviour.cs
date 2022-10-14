using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviours
{
    [Serializable]
    public abstract class StepBehaviour : MonoBehaviour
    {
        public abstract bool IsExecute { get; }
        public abstract UniTask Execute();
        public abstract Executer Executer { get; }
        
        private bool _isReady = false;

        public abstract void Init();

        private IEnumerator InitDelay()
        {
            yield return null;
            Init();
            if(!Executer){
                Debug.LogError($"{this} Не походит к Executer либо он вовсе отсутствует ");
            }

            if(enabled) {
                TimeLaps.Instance.AddBehaviour(this);
            }
            _isReady = true;
        }
        private void Start()
        {
            StartCoroutine(InitDelay());
        }
        public void OnEnable(){
            if(_isReady){
                TimeLaps.Instance.AddBehaviour(this);
            }
        }

        public void OnDisable(){
            if(_isReady){
                TimeLaps.Instance.RemoveBehaviour(this);
            }
        }
    }
    

    public abstract class RandomBeh<T> : StepBehaviour where T : Executer
    {
        [Tooltip("Шанс срабатывания поведения")]
        [SerializeField] public float Chance;

        protected T _executer;

        public override Executer Executer => _executer;

        public override bool IsExecute
        {
            get
            {
                var rand = UnityEngine.Random.Range(0f, 100f);
                return rand < Chance;
            }
        }

        public override void Init()
        {
            _executer = GetComponent<T>();
        }

    }

    public abstract class DelayBeh<T> : StepBehaviour where T : Executer
    {
        [Tooltip("Поведение срабатывает раз в какой то delay")]
        [SerializeField] public int Delay;
        
        protected T _executer;
        
        private int _currentStep = 0;
        public override bool IsExecute
        {
            get
            {
                _currentStep++;
                if (_currentStep >= Delay)
                {
                    OnNewLoop();
                    return true;
                }

                return false;
            }
        }

        public override Executer Executer => _executer;


        public virtual void OnNewLoop(){
            _currentStep = 0;
        } 
        
        
        public override void Init()
        {
            _executer = GetComponent<T>();
        }
    }
}
