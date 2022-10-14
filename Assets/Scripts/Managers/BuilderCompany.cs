using System.Linq;
using Behaviours.Com;
using GameData;
using UnityEngine;

namespace Managers.Com
{
    public class BuilderCompany : LocalCompany
    {
        public override void Init()
        {
            // City = FindObjectOfType<City>();
            // _soviet = new DirectorSoviet(SovietMembers, this);
            // Ambitions.OnChangeData += OnChangeAmbitions;
        }

        // public override void OnChangeAmbitions(float delta, DirAmbition ambition)
        // {
        //     if(ambition.Value - delta == 0){
                
        //     }else if(ambition.Value == 0){
        //         var realizations = this.gameObject.GetComponents<AmbitionRealizator>().Where((x) => x.AmbitionKey == ambition.Key);
        //         foreach(var component in realizations){
        //             Destroy(component);
        //         }
        //     }

        //     var x = new A();
        // }
    }

    internal class A
    {
    }
}
