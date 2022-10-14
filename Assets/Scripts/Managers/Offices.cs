using Cysharp.Threading.Tasks;
using GameData;
using GameData.Companies;
using Managers;


namespace Behaviours.Com{


    public class OfficeManager : DelayBeh<LocalCompany>
    {
       private List<Building, SpaceData> _buildings;


        public override void Init()
        {
            base.Init();
            // _buildings = _executer.Spaces;
        }
        public async override UniTask Execute()
        {
            
        }
    }
}
