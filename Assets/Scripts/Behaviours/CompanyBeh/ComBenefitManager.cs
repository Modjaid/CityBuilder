
using System.Collections;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;
using static Managers.AIDirector;

namespace Behaviours.Com
{
    public class ComBenefitManager : DelayBeh<LocalCompany>
    {
        [TextArea(0, 1000)]
        [SerializeField] private string _log;
        public float CommonEconomicBall { get; set; } = 15;
        private List<ResourceType, ResourceData> _resources;
        private float _marketBenefit = 1;
        private float _invetsments = 1;
        private float _monthRent = 0;
        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
        }

        public async override UniTask Execute()
        {
            _log = $"rent:{_monthRent} benefit:{_marketBenefit} investments:{_invetsments} CommonBall:{CommonEconomicBall}\n";

            var monthBall = (_marketBenefit / (_monthRent + _invetsments)) - 1;
            CommonEconomicBall += monthBall;
            if (CommonEconomicBall > 100)
            {
                CommonEconomicBall = 100;
                ResetPoints();
                return;
            }

            if (CommonEconomicBall < 0)
            {
                _executer.BANCROT();
                ResetPoints();
                await UniTask.Yield();
                return;
            }

            if (CommonEconomicBall < 6 && monthBall < 0 && _monthRent > 0)
            {
                
            }

        }
        private void ResetPoints()
        {
            _marketBenefit = 1;
            _invetsments = 1;
            _monthRent = 0;
        }

        private void OnAddCompanyBenefit(float money)
        {
            _marketBenefit += money;
        }
        private void OnAddCompanyInvestments(float money)
        {
            _invetsments += money;
        }
        private void OnPayForRent(float money)
        {
            _monthRent += money;
        }
        private void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(Delay());
        }
        private void OnDisable()
        {
            base.OnDisable();
            var soviet = _executer.Soviet;
            soviet.OnAddBenefit -= OnAddCompanyBenefit;
            soviet.OnAddInvestments -= OnAddCompanyInvestments;
            _executer.OnPayRent -= OnPayForRent;
        }
        private IEnumerator Delay()
        {
            yield return new WaitUntil(() => _executer != null);
            var soviet = _executer.Soviet;
            soviet.OnAddBenefit += OnAddCompanyBenefit;
            soviet.OnAddInvestments += OnAddCompanyInvestments;
            _executer.OnPayRent += OnPayForRent;
        }
    }

}
