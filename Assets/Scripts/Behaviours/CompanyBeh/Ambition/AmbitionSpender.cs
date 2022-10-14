using Cysharp.Threading.Tasks;
using GameData;
using Managers;
using UnityEngine;

namespace Behaviours.Com{
    public class AmbitionSpender : AmbitionRealizator
    {
        public float AmbitionSpendSpeed;
        public async override UniTask Execute()
        {
            Container.Factor -= AmbitionSpendSpeed;
            if (Container.Factor < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
