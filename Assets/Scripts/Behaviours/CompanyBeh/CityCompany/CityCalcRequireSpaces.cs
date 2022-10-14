
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Companies;
using GameData.Resources;
using Managers;
using static Managers.AIDirector;

namespace Behaviours.Com
{
    public class CityCalcRequireSpaces : DelayBeh<CitizenCompany>
    {
        public List<BuildingType, RequireSpaceData> Requires;
        private List<ResourceType, ResourceData> _resources;
        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;

        }
        public async override UniTask Execute()
        {
            var localCompanies = _executer.City.FindExecuters<LocalCompany>();
            Requires.Clear();
            var allCitizens = _resources.GetValue(ResourceType.Citizen);
            Requires.Add(BuildingType.House, allCitizens);
            
            foreach (var localCompany in localCompanies)
            {
                if (localCompany.transform.TryGetAmbitions<AmbitionContainer>(out var ambitions))
                {
                    foreach (var amb in ambitions)
                    {
                        if (amb.Input.IsStuff())
                        {
                            var compResources = localCompany.Resources.GetValue(amb.Input);
                            Requires.Add(amb.WorkPlace, compResources);
                        }
                    }
                }
                
            }
            foreach (var building in _executer.City.GetBuildings())
            {
                Requires.Add(building.model.Type, -building.model.GetMaxPlacements());
            }
        }
        public void CalcCitizenRequest()
        {
            var allCitizens = _resources.GetValue(ResourceType.Citizen);
            var placements = _executer.GetAllSpaces().GetSumValues();
            var outCitizens = allCitizens - placements;
            if (outCitizens > 0)
            {
                Requires.Set(BuildingType.House, outCitizens);
            }
        }
    }

}
