using Cysharp.Threading.Tasks;
using GameData;
using GameData.Companies;
using GameData.Resources;
using Managers;

namespace Behaviours.Com
{
    public class ComPassFreePlacements : DelayBeh<Company>
    {
        private List<ResourceType, ResourceData> _resources;
        private List<Building, SpaceData> _spaces;

        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
            // _spaces = _executer.Spaces;
        }
        public async override UniTask Execute()
        {
            // _spaces.Foreach((space) =>{
            //     var spaceStuff = space.Stuff.GetSumValues();
            //     if(space.Value > spaceStuff){
            //         var removePlacements = spaceStuff - space.Value;
            //         _spaces.Add(space.Key, removePlacements);
            //         space.Key.ChangePlacements(_executer, removePlacements);
            //     }
            // });
        }
    }

}
