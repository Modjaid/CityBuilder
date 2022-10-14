
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public abstract class Executer : MonoBehaviour
    {
        [TextArea(0,1000)]
        [SerializeField] public string Log;

        [SerializeField] private Sprite _avatar;
        [SerializeField] protected string _firstNameKey;
        [SerializeField] protected string _lastNameKey;
        
        [SerializeField] private float _age;
        
        public City City { get; private set; }
        public string FirstNameKey => _firstNameKey;
        public string LastNameKey => _lastNameKey;

        public Sprite Avatar
        {
            get => _avatar;
            set => _avatar = value;
        }
        public virtual string FirstName
        {
            get => LocalizationManager.Localize(_firstNameKey);
            set => _firstNameKey = value;
        }
        public virtual string LastName
        {
            get => LocalizationManager.Localize(_lastNameKey);
            set => _lastNameKey = value;
        }
        public float Age
        {
            get => _age;
            set => _age = value;
        }

        private bool _isInited = false;


#if UNITY_EDITOR
        public virtual void Save()
        {
            var text = this.GetType().ToString();
            AssetDatabase.CreateFolder("Assets", "CustomFolder");
            AssetDatabase.CreateAsset(this, $"Assets/CustomFolder/{text}.asset");
            AssetDatabase.SaveAssets();
        }
#endif

        public void OnDestroy(){
            City.Remove(this);
        }

        public virtual void Init()
        {
            City = FindObjectOfType<City>();
            _isInited = true;
        }

        private void Start(){
            if (!_isInited)
            {
                Init();
            }
            City.Add(this);
            StartCoroutine(DebugLog());
        }
        
        private IEnumerator DebugLog(){
            while(true){
                yield return new WaitForSeconds(1);
                Log = "";
                OnUpdateLog();
            }
        }

        protected virtual void OnUpdateLog(){}
    }
}