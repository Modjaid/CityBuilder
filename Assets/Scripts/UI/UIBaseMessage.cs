using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization;
using Cysharp.Threading.Tasks;
using External;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public abstract class UIBaseMessage : MonoBehaviour
    {
        [SerializeField] private bool _resolved = false;
        [SerializeField] protected Image _avatar;
        [SerializeField] protected Transform _positiveAnswers;
        [SerializeField] protected Transform _negativeAnswers;
        protected Dictionary<string, string> _tagsData;
        protected List<Resolve> _dialogButtons;
        protected string _messageKey;

        private int _waitSteps;

        private Action<ResolveType> _playerHandler;
        private ResolveType _defaultAnswer;


        public virtual void Init()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(OnChangeSelected);
            var toggle = GetComponent<Toggle>();
            toggle.group = transform.parent.GetComponent<ToggleGroup>();
            _dialogButtons = new List<Resolve>();
            _tagsData = new Dictionary<string, string>();
            LocalizationManager.LocalizationChanged += Localize;
        }
        
        public void OnEnable()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(UpdLayout);
        }
        public void OnDisable()
        {
            GetComponent<Toggle>().onValueChanged.RemoveListener(UpdLayout);
        }


        public void AddTagsData(Dictionary<string, string> tagsData)
        {
            _tagsData = tagsData;
        }

        public void AddResolveOptions(Action<ResolveType> handler, (ResolveType, string)[] resolves,
            ResolveData[] allResolves)
        {
            _dialogButtons.AddRange(SetButtons(resolves, allResolves));
            _playerHandler = handler;
        }

        public void AddResolveOptions(Action<ResolveType> handler, (ResolveType, string)[] resolves,
            ResolveData[] allResolves, int stepDelay, ResolveType defaultAnswer = ResolveType.None)
        {
            TimeLaps.Instance.OnNextStep += OnNextStep;
            _waitSteps = stepDelay;
            _defaultAnswer = defaultAnswer;
            _dialogButtons.AddRange(SetButtons(resolves, allResolves));
            _playerHandler = handler;
        }

        public abstract void OnChangeSelected(bool isSelected);

        public virtual void OnPressResolve(ResolveType resolve)
        {
            TimeLaps.Instance.OnNextStep -= OnNextStep;
            ClearButtons();
            _playerHandler(resolve);
        }

        public virtual void OnMessageTimeOut()
        {
        }

        public void ClearButtons()
        {
            foreach (var button in _dialogButtons)
            {
                Destroy(button.gameObject);
            }

            _dialogButtons.Clear();
            UpdLayout(true);
        }

        private void OnNextStep()
        {
            _waitSteps--;
            if (_waitSteps <= 0)
            {
                if (_defaultAnswer != ResolveType.None)
                {
                    OnPressResolve(_defaultAnswer);
                }
                else
                {
                    OnMessageTimeOut();
                }
            }
        }

        private List<Resolve> SetButtons((ResolveType type, string data)[] resolves, ResolveData[] allResolves)
        {
            var allResolvesList = allResolves.ToList();
            var results = new List<Resolve>();
            foreach (var resolve in resolves)
            {
                if (allResolvesList.TryFind(((asset) => asset.Type == resolve.type), out var data))
                {
                    Transform parent = _negativeAnswers;
                    Color color = LevelProperty.Instance.Canvas.NegativeColor;
                    if (data.IsPositive)
                    {
                        parent = _positiveAnswers;
                        color = LevelProperty.Instance.Canvas.PositiveColor;
                    }

                    var newButton = Instantiate(LevelProperty.Instance.Canvas.ButtonPrefab, parent);
                    newButton.name = data.Type.ToString();
                    newButton.GetComponentInChildren<Image>().color = color;
                    newButton.GetComponent<Resolve>().Answer = data.Type;
                    newButton.GetComponent<BaseKeyLocalizeText>().BaseKey = data.LocalKey;
                    newButton.GetComponent<BaseKeyLocalizeText>().enabled = true;
                    newButton.GetComponent<Resolve>().Subscribe(this);
                    newButton.GetComponent<Resolve>().AddData(resolve.data);
                    results.Add(newButton.GetComponent<Resolve>());
                }
            }

            return results;
        }


        public void Resolve()
        {

        }

        public void SetImage()
        {

        }

        public void SetOptions()
        {

        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
            TimeLaps.Instance.OnNextStep -= OnNextStep;
        }

        protected string UpdTextTagData(string text)
        {
            foreach (var tag in _tagsData)
            {
                // var localWord = LocalizationManager.Localize(tag.Value);
                text = text.Replace(tag.Key, tag.Value);
            }

            return text;
        }

        public abstract void Localize();

        private void UpdLayout(bool isActive)
        {
            Yield();
        }
        private async UniTask Yield()
        {
            var rect = GetComponentInParent<RectTransform>();
            await UniTask.Yield();
            if(!this.gameObject.activeSelf) return;
            LayoutRebuilder.MarkLayoutForRebuild(rect);
            await UniTask.Yield();
            LayoutRebuilder.MarkLayoutForRebuild(rect);
            await UniTask.Yield();
            LayoutRebuilder.MarkLayoutForRebuild(rect);
        }
        
    }
}
