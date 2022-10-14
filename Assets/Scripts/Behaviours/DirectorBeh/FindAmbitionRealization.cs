using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameData;
using Managers;
using UI;
using UnityEngine;

namespace Behaviours.Dir
{
    public class FindAmbitionRealization : RandomBeh<AIDirector>
    {
        [SerializeField] private AmbitionData _ambition;
        [SerializeField] private int _forNewCompanyDelay;

        private int _currentForNewCompanyDelay;
        private HashSet<LocalCompany> _companies;
        private List<AmbitionContainer, AmbitionData> _experience;
        private Queue<LocalCompany> _targets;

        private DirAmbitionCreator _creator;

        public override void Init()
        {
            base.Init();
            _companies = _executer.CompanyCash;
            _experience = _executer.Experience;
            _creator = GetComponent<DirAmbitionCreator>();
            _currentForNewCompanyDelay = _forNewCompanyDelay;
        }
        public async override UniTask Execute()
        {
            if (_ambition.Key == null)
            {
                Debug.LogError("FindAmbitionRealization is not has any ambition");
                return;
            }
            if (_targets == null)
            {
                foreach (var company in _companies)
                {
                    if (company.TryAddAmbition(_executer, _ambition))
                    {
                        _executer.Money -= _ambition.Money;
                        _experience.Add(_ambition);
                        _currentForNewCompanyDelay = _forNewCompanyDelay;
                        this.enabled = false;
                        _creator.enabled = true;
                        return;
                    }
                }
                var others = _executer.City.FindExecuters<LocalCompany>().Where((x) => !_companies.Contains(x));
                _targets = new Queue<LocalCompany>(others);
            }

            while (_targets.Count() > 0)
            {
                var company = _targets.Dequeue();
                var answer = company.ReviewAmbitionWithNewCandidate(_executer, _ambition);

                switch (answer)
                {

                    case -1: continue;
                    case 0:
                        this.enabled = false;
                        // Debug.Log($"Начало ожидания ответа игрока");
                        return;
                    case 1:
                        _targets = new Queue<LocalCompany>();
                        company.AddEasyAmbition(_executer, _ambition);
                        _executer.Money -= _ambition.Money;
                        _executer.CompanyCash.Add(company);
                        _experience.Add(_ambition);
                        _currentForNewCompanyDelay = _forNewCompanyDelay;
                        this.enabled = false;
                        _creator.enabled = true;
                        return;
                }
            }

            _currentForNewCompanyDelay--;

            if (_currentForNewCompanyDelay <= 0)
            {
                _executer.SendQueryForCreateNewCompany(_ambition);
                this.enabled = false;
            }

        }

        public void ClearFind()
        {
            _targets = null;
            _currentForNewCompanyDelay = _forNewCompanyDelay;
        }

        public void StartFind(AmbitionData ambition)
        {
            _ambition = ambition;
            this.enabled = true;
        }
    }

}
