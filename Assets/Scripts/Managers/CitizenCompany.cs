
using Behaviours.Com;
using GameData;
using GameData.Companies;
using UnityEngine;

namespace Managers
{
    public class CitizenCompany : Company
    {
        public override void Init()
        {
            base.Init();
            City.MainCompany = this;
        }

        public override float GetPayForRent(float mustPay)
        {
            if(TryGetComponent<CitySpaceManager>(out var spaceManager))
            {
                var canPay = Resources.GetValue(ResourceType.Dollar);
                canPay = canPay == 0 ? 1 : canPay;
                spaceManager.OffsetTargetSpace(canPay / mustPay);
            }
            else
            {
                Debug.LogError($"component CitySpaceManager not found in CitizenCompany");
            }
        
            return base.GetPayForRent(mustPay);
        }

        public override SpaceData GetSpaceData(Building building)
        {
            return GetComponent<CitySpaceManager>().Spaces.GetData(building);
        }

        public override List<Building, SpaceData> GetAllSpaces()
        {
            return GetComponent<CitySpaceManager>().Spaces;
        }
    }
}