
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData.Resources;
using Managers;

namespace Behaviours.Com
{
    public class ComEasyFactor : DelayBeh<Company>
    {
        public List<ResourceData> Resources;

        private ResourceMarket _market;

        public override void Init()
        {
            base.Init();
            _market = _executer.City.Market;
        }

        public async override UniTask Execute()
        {
            foreach (var resData in Resources)
            {
                _market.AddProduct(resData.Key, _executer, resData.Value, resData.Price);
            }
        }
    }
}
