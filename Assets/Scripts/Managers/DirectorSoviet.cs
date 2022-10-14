using System;
using System.Collections.Generic;
using GameData;
using Managers.Dir;
using UnityEngine;

namespace Managers
{
    public class DirectorSoviet
    {
        public event Action<float> OnAddInvestments;
        public event Action<float> OnAddBenefit;
        public Director MainDirector => _mainDirector;
        private List<Director, FloatKeyData<Director>> _members;
        private Director _mainDirector;
        private LocalCompany _company;

        public List<Director> Directors => _members.Keys;

        public float AverageMonetaryValue
        {
            get
            {
                var sum = 0f;
                var directors = Directors;
                foreach (var director in directors)
                {
                    sum += director.Money;
                }

                return sum / directors.Count;
            }
        }

        public float GetStockPercen(AIDirector dir) => _members.GetValue(dir);

        public DirectorSoviet(List<Director, FloatKeyData<Director>> members, LocalCompany company)
        {
            _members = members;
            _company = company;
            if (members.Count > 0)
            {
                var memberList = members.ToList();
                _members.Set(memberList[0].Key, 100f);
                memberList[0].Key.CompanyCash.Add(_company);
                if (members.Count > 1)
                {
                    for (int i = 1; i < memberList.Count; i++)
                    {
                        _members.Add(memberList[0].Key, -memberList[i].Value);
                        memberList[i].Key.CompanyCash.Add(_company);
                    }
                }

                FindMainDirector();
            }
        }

        public bool IsMember(Director dir)
        {
            return _members.Contains(dir);
        }


        /// <returns>-1 = быстрый отказ, 0 = ждать ответа игрока, 1 = Быстрое согласие</returns>
        public int ReviewAmbitionWithNewCandidate(AmbitionData newAmbition, AIDirector candidate)
        {
            var votes = FastVoteNewCandidate(candidate, newAmbition);
            if (votes >= _company.AmbitionAcceptRange)
            {
                return 1;
            }
            if (TryGetPlayer(out var player, out var playerStocks))
            {
                if (votes + playerStocks > _company.AmbitionAcceptRange)
                {
                    player.NewCandidateConsent(candidate, newAmbition, _company, votes);
                    return 0;
                }
            }
            return -1;
        }

        public void Investment(Director investor, float money)
        {
            if (float.IsNaN(money) || float.IsInfinity(money))
            {
                Debug.LogError($"Investments is INFINITY");
            }

            if (money <= 0)
            {
                return;
            }
            
            
            var oldCompanyCost = _company.Turnover;
            var newCompanyCost = oldCompanyCost + money;
            
            var updCandidateStocks =
                ((oldCompanyCost * _members.GetValue(investor) * 0.01f) + money) /
                newCompanyCost * 100f;
            _members.Set(investor, updCandidateStocks);
            var members = _members.ToList();
            members.RemoveAll((x) => x.Key == investor);
            foreach (var member in members)
            {
                var updMemberStocks = (oldCompanyCost * _members.GetValue(member.Key) * 0.01f) /
                    newCompanyCost * 100f;
                _members.Set(member.Key, updMemberStocks);
            }
            OnAddInvestments?.Invoke(money);
        }

        private bool TryGetPlayer(out Player player, out float stocks)
        {
            Player result = null;
            bool isExist = false;
            float resultStocks = 0f;
            _members.Foreach((dir) =>
            {
                if (dir.Key is Player)
                {
                    result = (Player) dir.Key;
                    resultStocks = dir.Value;
                    isExist = true;
                }
            });

            player = result;
            stocks = resultStocks;
            return isExist;
        }

        private float FastVoteNewCandidate(Director candidate, AmbitionData ambition)
        {
            var votes = 0f;

            _members.Foreach((member) =>
            {
                if (member.Key is AIDirector)
                {
                    if ((member.Key as AIDirector).TryGetConsent(candidate, ambition))
                    {
                        votes += _members.GetValue(member.Key);
                    }
                }
            });
            return votes;
        }

        /// <returns>true == Все акции директора проданы, директор ушел из компании</returns>
        public bool BuyNewStocks(Director buyer, AIDirector seller, float percent)
        {
            bool isRemove = false;
            var sellPercent = Math.Abs(_members.Add(seller, -percent));
            if (!_members.Contains(seller))
            {
                // Продал все акции
                seller.CompanyCash.Remove(_company);
                isRemove = true;
            }

            if (sellPercent > 0)
            {
                _members.Add(buyer, sellPercent);
                buyer.CompanyCash.Add(_company);
            }
            FindMainDirector();
            return isRemove;
        }

        public void SplitBenefit(float money)
        {
            money /= 100;
            _members.Foreach((member) =>
            {
                member.Key.Money += money * member.Value;
            });
            OnAddBenefit?.Invoke(money * 100);
        }

        public Director FindMainDirector()
        {
            var maxValue = 0f;
            _mainDirector = null;

            _members.Foreach((keyValue) =>
            {
                if (keyValue.Value > maxValue)
                {
                    maxValue = keyValue.Value;
                    _mainDirector = keyValue.Key;
                    _mainDirector.CompanyCash.Add(_company);
                }
            });

            if (_mainDirector == null)
            {
                Debug.LogError("!!!!НЕТУ ДИРЕКТОРОВ В КОМПАНИИ!!!!!");
            }

            return _mainDirector;
        }


    }
}