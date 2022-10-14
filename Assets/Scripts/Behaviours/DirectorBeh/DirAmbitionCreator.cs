
using System.Collections.Generic;
using System.Linq;
using Behaviours.Com;
using Cysharp.Threading.Tasks;
using External;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Dir
{
    /// <summary>
    /// 1)либо смотрит на весь templates и берет относительно спроса рыночного - 4%
    /// 2)либо берет рандомную амбицию из templates по аутам опыта - 20%
    /// 3)просто берет амбицию из опыта - 78%
    /// </summary>
    public class DirAmbitionCreator : RandomBeh<AIDirector>
    {
        [Header("Если пустой то автозаполняется из ресурсов")] [SerializeField]
        private List<AmbitionContainer> _templates;

        [Tooltip("Шанс взятия сразу из опыта")] [SerializeField]
        private int _expChance = 80;

        [Tooltip("Шанс взятия новой похожей амбиции")] [SerializeField]
        private int _similiarChance = 95;

        [Tooltip("Доля шаблоновой амбиции")] [SerializeField]
        private Vector2 _experienceImproveFactor = new Vector2(-0.2f, 0.4f);

        [Tooltip("Доля всех денег директора")] [SerializeField]
        private Vector2 _oldAmbitionSupposeMoney = new Vector2(0, 0.2f);

        [Tooltip("Доля шаблоновой амбиции")] [SerializeField]
        private Vector2 _newAmbitionFactor = new Vector2(0, 0.5f);

        [Tooltip("Полноценный диапазон")] [SerializeField]
        private Vector2 _newAmbitionFactorSpend = new Vector2(0, 0.5f);

        [Tooltip("Доля всех денег директора")] [SerializeField]
        private Vector2 _newAmbitionMoney = new Vector2(0,0.5f);
        
        [Tooltip("Доля шаблоновой амбиции")] [SerializeField]
        private Vector2 _newAmbitionResourceSpend = new Vector2(2,50f);

        private List<AmbitionContainer, AmbitionData> _experience;
        private HashSet<LocalCompany> _companies;
        private AmbitionData _waitAmbition;
        private int _waitCompanyAnswers;
        private Dictionary<LocalCompany, bool> _companyAnswers;

        private FindAmbitionRealization _finder;

        private bool IsSprintMode => Chance == 100 && _bufferChance != Chance;
        private float _bufferChance;

        public override void Init()
        {
            base.Init();
            _bufferChance = Chance;
            Chance = 100;
            _experience = _executer.Experience;
            _companies = _executer.CompanyCash;
            _finder = GetComponent<FindAmbitionRealization>();

            if (_templates.Count == 0)
            {
                _templates = AmbitionContainer.GetAllDBMains();
            }
        }

        public override async UniTask Execute()
        {
            if (IsSprintMode)
            {
                Chance = _bufferChance;
            }

            #region Experience Chance

            AmbitionData newAmbition;
            var exps = _experience.ToList();
            if (exps.Count > 0)
            {
                if (Rand.IsChance(_expChance))
                {
                    newAmbition = GetExpAmbition(exps);
                    FindRealization(newAmbition);
                    return;
                }
                else if (Rand.IsChance(_similiarChance))
                {

                    if (TryGetNewSimiliarAmbitionTemplate(exps, out var template))
                    {
                        newAmbition = CreateAmbition(template);
                        FindRealization(newAmbition);
                        return;
                    }

                    Debug.Log("Все шаблоны лежат в опыте, шаблон для новой амбиции будет искаться с новой целью");
                }
            }

            #endregion

            #region Full new Ambition

            if (TryGetNewAmbitionTemplate(out var newTargetTemplate))
            {
                newAmbition = CreateAmbition(newTargetTemplate);
                FindRealization(newAmbition);
                return;
            }

            Debug.Log("Не удалось найти амбицию даже с новой целью");

            #endregion

            return;
        }

        public AmbitionData CreateAmbition(AmbitionContainer template)
        {
            AmbitionData ambition = new AmbitionData();
            ambition.Key = template;
            ambition.Factor = Random.Range(template.Factor * _newAmbitionFactor.x,
                template.Factor * _newAmbitionFactor.y);
            ambition.Money = Random.Range(_executer.Money * _newAmbitionMoney.x, _executer.Money * _newAmbitionMoney.y);
            if (template.TryGetComponent<AmbResourceSpender>(out var resSpender))
            {
                ambition.ResourceSpend = Random.Range(resSpender.ResourceSpend * _newAmbitionResourceSpend.x, resSpender.ResourceSpend * _newAmbitionResourceSpend.y);
            }
            ambition.FactorSpend = Random.Range(_newAmbitionFactorSpend.x, _newAmbitionFactorSpend.y);
            return ambition;
        }

        public AmbitionData GetExpAmbition(List<AmbitionData> exps)
        {
            var newAmbition = exps[Random.Range(0, exps.Count)];
            newAmbition.Factor = Random.Range(newAmbition.Factor * _experienceImproveFactor.x,
                newAmbition.Factor * _experienceImproveFactor.y);
            if (_executer.Money > 1)
            {
                newAmbition.Money = Random.Range(_executer.Money * _oldAmbitionSupposeMoney.x,
                    _executer.Money * _oldAmbitionSupposeMoney.y);
            }

            return newAmbition;
        }

        public void FindRealization(AmbitionData ambition)
        {
            _finder.StartFind(ambition);
            this.enabled = false;
            // if(_executer.Money < 1000) return;
            //
            // #region Создание новой компании
            // ambition.Money = _executer.Money * 0.7f;
            // ambition.Factor = 20;
            // var parent = GameObject.Find("Executors");
            // if(!parent) Debug.LogError("ГО Executors не найден !!!!");
            //
            // var newCompany = ExecuterAsset.CreateLocalCompany(_executer, ambition.Key);
            // await UniTask.Yield();
            // await newCompany.ReviewAmbition(_executer, ambition);
            // ambition.Money = 0;
            // _executer.Experience.Add(ambition);
            // #endregion
            // //TODO: Если компанию не открыть то ....
        }

        private void HandleCompanyReview(LocalCompany company, bool isOk)
        {
            _companyAnswers.Add(company, isOk);
            if (_companyAnswers.Count == _waitCompanyAnswers)
            {
                var positives = _companyAnswers.Where((x) => x.Value);
                if (positives.Count() > 0)
                {
                    var index = Random.Range(0, positives.Count());
                    var pretendent = positives.ToList()[index];
                    pretendent.Key.AddEasyAmbition(_executer, _waitAmbition);
                    _executer.Money -= _waitAmbition.Money;
                    _experience.Add(_waitAmbition);
                }
                this.enabled = true;
            }
        }

        // public float CalcNewFactor(float templateFactor, )
        private bool TryGetNewAmbitionTemplate(out AmbitionContainer result)
        {
            _templates.Shuffle();
            foreach (var template in _templates)
            {
                if (!_experience.TryGetData(template, out var data))
                {
                    result = template;
                    return true;
                }
            }

            result = null;
            return false;
        }

        private bool TryGetNewSimiliarAmbitionTemplate(List<AmbitionData> exps, out AmbitionContainer result)
        {
            exps.Shuffle();
            _templates.Shuffle();
            var expOutputs = new HashSet<ResourceType>();
            foreach (var exp in exps)
            {
                expOutputs.Add(exp.Output);
            }

            foreach (var template in _templates)
            {
                if (expOutputs.Contains(template.Output) && !_experience.TryGetData(template, out var data))
                {
                    result = template;
                    return true;
                }
            }

            result = null;
            return false;

        }
    }
}
