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
using UnityEngine.Serialization;

namespace Behaviours.Com
{
    public class CitySpaceManager : DelayBeh<CitizenCompany>
    {
        [SerializeField] private float TargetSpace = 1;
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
            var allStuff = _resources.GetValue(ResourceType.Citizen);
            var placements = Spaces.GetSumValues();
            placements = (placements == 0) ? 1 : placements;
            var currentFillSpace = placements / allStuff;
            if (currentFillSpace < TargetSpace)
            {
                placements = (int) ((allStuff - placements) * TargetSpace);
                var citySpaces = _executer.City.GetBuildings(BuildingType.House);
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

        private float UpdSpace(Building space,float newPlacements)
        {
            var offset = space.ChangePlacements(_executer, newPlacements);
            return Spaces.Add(space, offset);
        }

        public void OffsetTargetSpace(float newTargetSpace)
        {
            if (newTargetSpace > TargetSpace)
            {
                var offset = TargetSpace / newTargetSpace;
                TargetSpace += offset;
                if (TargetSpace > 1)
                {
                    TargetSpace = 1;
                }
            }
            else
            {
                var offset = newTargetSpace / TargetSpace;
                TargetSpace -= offset;
                if (TargetSpace < 0.1)
                {
                    TargetSpace = 0.1f;
                }
            }
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
