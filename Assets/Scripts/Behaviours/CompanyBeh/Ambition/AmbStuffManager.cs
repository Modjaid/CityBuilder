using System.Collections;
using System.Collections.Generic;
using Behaviours.Dir;
using Cysharp.Threading.Tasks;
using External;
using GameData;
using GameData.Companies;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class AmbStuffManager : AmbitionRealizator
    {
        public float TargetSpace;
        public List<Building, SpaceData> Spaces;
        private List<ResourceType, ResourceData> _resources;

        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
            Spaces.Foreach((space) => space.Key.ChangePlacements(_executer, space.Value));
        }

        public async override UniTask Execute()
        {
            var allStuff = _resources.GetValue(Container.Input);
            var placements = Spaces.GetSumValues();
            placements = (placements == 0) ? 1 : placements;
            var currentFillSpace = placements / allStuff;
            
            if (currentFillSpace < TargetSpace)
            {
                placements = (int) ((allStuff - placements) * TargetSpace);
                var citySpaces = _executer.City.GetBuildings(Container.WorkPlace);
                citySpaces.Shuffle();
                foreach (var citySpace in citySpaces)
                {
                    if (GetPermision(citySpace.model, citySpace.dir))
                    {
                        placements -= UpdSpace(citySpace.model, placements);
                    }
                }
            }
            else if(currentFillSpace > TargetSpace)
            {
                placements = (int) ((allStuff * TargetSpace) - placements);
                foreach (var spacesKey in Spaces.Keys)
                {
                    placements -= UpdSpace(spacesKey, placements);
                }
            }
        }

        public void ForceClear()
        {
            Spaces.Foreach(data => data.Key.ClearCompany(_executer));
        }

        private float UpdSpace(Building space,float newPlacements)
        {
            var offset = space.ChangePlacements(_executer, newPlacements);
            return Spaces.Add(space, offset);
        }
        

        private bool GetPermision(Building space, AIDirector director)
        {
            if (director.TryGetComponent<DirRentCollector>(out var collector))
            {
                var companyDebt = collector.GetDebt(_executer);
                var companyMoney = _resources.GetValue(ResourceType.Dollar);
                if (companyDebt > 0)
                {
                    Debug.Log(
                        $"Компания ищет новые офисы у тех кому должна с успехом:{companyMoney > companyDebt}");
                    if (companyMoney < companyDebt)
                    {
                        return false;
                    }
        
                    collector.PrePayFoRent(_executer, space);
                }
            }

            return true;
        }
        public async void OnDisable()
        {
            await Execute();
            Spaces.Foreach((space) =>
            {
                space.Key.ChangePlacements(_executer, -space.Value);
                space.Value = 0;
                return space;
            });
            base.OnDisable();
        }
    }
}
