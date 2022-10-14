using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using GameData.Companies;
using UnityEngine;

namespace Managers.Com{
     [CreateAssetMenu(fileName = "ForeignCompany", menuName = "Managers/Company/Foreign", order = 1)]
    public class ForeignCompany : Company
    {
        public override SpaceData GetSpaceData(Building building)
        {
            throw new NotImplementedException();
        }

        public override List<Building, SpaceData> GetAllSpaces()
        {
            throw new NotImplementedException();
        }
    }
}

