using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public abstract class LocalizeText : MonoBehaviour
    {
        [FormerlySerializedAs("text")] 
        [SerializeField] public TextMeshProUGUI Text;
        private string _key;

        // [SerializeField] private List<TagData> _tagData;
        private Dictionary<string, string> _tags = new Dictionary<string, string>();
        
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                Localize();
            }
        }
        
        public void SetTags(Dictionary<string, string> tags)
        {
            _tags = tags;
        }

        public void Start()
        {
            Init();
            Localize();
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        public void Localize()
        {
            var localize = LocalizationManager.Localize(_key);
            Text.text = UpdTextTagData(localize);
        }

        protected string UpdTextTagData(string text){
            foreach(var tag in (_tags)){
                text = text.Replace(tag.Key, tag.Value);
            }
            return text;
        }
        
        [ContextMenu("Localize")]
        private void InspectorLocalize()
        {
            Init();
            Text.text = LocalizationManager.Localize(_key);
        }

        public virtual void Init()
        {
            LocalizationManager.LocalizationChanged += Localize;
        }
    }
}
