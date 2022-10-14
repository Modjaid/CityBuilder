
using GameData;
using GameData.Companies;
using Managers.Dir;
using UnityEngine;

namespace Managers.Com
{
    public class PartyCompany : Company
    {
        [SerializeField] private List<Director, FloatKeyData<Director>> _members;

        public void Add(Director director, float percent)
        {
            var delta = percent / 100f;
            _members.Foreach((member) =>
            { 
                member.Value = member.Value - (member.Value * delta);
                return member;
            });

            _members.Add(director, percent);
        }

        public override SpaceData GetSpaceData(Building building)
        {
            throw new System.NotImplementedException();
        }

        public override List<Building, SpaceData> GetAllSpaces()
        {
            throw new System.NotImplementedException();
        }
        
    }
}
