using Cysharp.Threading.Tasks;
using GameData;
using GameData.Companies;
using GameData.Resources;
using Managers;

namespace Behaviours.Com
{
    public class AmbPlacementFinder : AmbitionRealizator
    {
        private List<ResourceType, ResourceData> _resources;
        private List<Building, SpaceData> _offices;

        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
            // _offices = _executer.Spaces;
        }

        public async override UniTask Execute()
        {
                //TODO: HERE
            // var outResidents = _resources.GetValue(AmbitionKey.Input) - offices.GetSumStuff(AmbitionKey.Input);
            // if (outResidents > 0)
            // {
            //     foreach (var office in offices)
            //     {
            //         if (office.Key.Type == AmbitionKey.WorkPlace)
            //         {
            //             var difference = office.Key.ChangePlacements(_executer, outResidents);
            //             if (difference > 0)
            //             {
            //                 var updOffice = office;
            //                 updOffice.Value += difference;
            //                 updOffice.ChangeStuff(AmbitionKey.Input, difference, _executer);
            //                 _offices.Set(updOffice);
            //                 outResidents -= difference;
            //             }
            //         }
            //     }
            //
            //     if (outResidents > 0)
            //     {
            //         foreach (var building in _executer.City.GetBuildings(AmbitionKey.WorkPlace))
            //         {
            //             var difference = building.model.ChangePlacements(_executer, outResidents);
            //             if (difference > 0)
            //             {
            //                 var updOffice = new SpaceData();
            //                 updOffice.Key = building.model;
            //                 updOffice.Stuff = new List<ResourceType, FloatKeyData<ResourceType>>();
            //                 updOffice.Value = difference;
            //                 updOffice.ChangeStuff(AmbitionKey.Input, difference, _executer);
            //                 _offices.Set(updOffice);
            //                 outResidents -= difference;
            //             }
            //         }
            //     }
            // }
        }
    }
}
