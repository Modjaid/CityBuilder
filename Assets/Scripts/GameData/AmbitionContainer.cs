using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours.Com;
using GameData.Companies;
using Managers;
using UnityEngine;

namespace GameData
{
    public class AmbitionContainer : MonoBehaviour, IEquatable<AmbitionContainer>
    {
        [TextArea(0, 1000)] [SerializeField] public string Description;
        [SerializeField] public bool IsHide = false;
        [SerializeField] public ResourceType Input;
        [SerializeField] public ResourceType Output;
        [SerializeField] public BuildingType WorkPlace;
        [SerializeField] public float Factor = 1;
        [SerializeField] public float Money;
        [SerializeField] public AmbitionContainer Key;

        public float ResourceSpend
        {
            get
            {
                if (TryGetComponent<AmbResourceSpender>(out var resSpender))
                {
                    return resSpender.ResourceSpend;
                }

                return 0.01f;
            }
            set
            {
                if (TryGetComponent<AmbResourceSpender>(out var resSpender))
                {
                    resSpender.ResourceSpend = value;
                }
            }
        }
        
        public float AmbitionSpend
        {
            get
            {
                if (TryGetComponent<AmbitionSpender>(out var ambSpender))
                {
                    return ambSpender.AmbitionSpendSpeed;
                }

                return 0.01f;
            }
            set
            {
                if (TryGetComponent<AmbitionSpender>(out var ambSpender))
                {
                    ambSpender.AmbitionSpendSpeed = value;
                }
            }
        }

        public bool TryGetSpaceData(Building building, out SpaceData space)
        {
            if (TryGetComponent<AmbStuffManager>(out AmbStuffManager stuffManager))
            {
                if (stuffManager.Spaces.TryGetData(building, out space))
                {
                    return true;
                }
            }

            space = new SpaceData();
            return false;
        }
        public static List<AmbitionContainer> GetAllDBMains()
        {
            var ambitions = UnityEngine.Resources.LoadAll<AmbitionContainer>("Ambitions");
            var result = new List<AmbitionContainer>();
            foreach (var ambition in ambitions)
            {
                if (!ambition.IsHide)
                {
                    result.Add(ambition);
                }
            }

            return result;
        }

        public static List<AmbitionContainer> GetAllDBHides()
        {
            var ambitions = UnityEngine.Resources.LoadAll<AmbitionContainer>("Ambitions");
            var result = new List<AmbitionContainer>();
            foreach (var ambition in ambitions)
            {
                if (ambition.IsHide)
                {
                    result.Add(ambition);
                }
            }

            return result;
        }

        public static List<AmbitionContainer> GetDBAmbitionsByTarget(ResourceType outTarget)
        {
            var ambitions = UnityEngine.Resources.LoadAll<AmbitionContainer>("Ambitions");
            var result = new List<AmbitionContainer>();
            foreach (var ambition in ambitions)
            {
                if (ambition.Output == outTarget && !ambition.IsHide)
                {
                    result.Add(ambition);
                }
            }

            return result;
        }


        public bool Equals(AmbitionContainer obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            if (this.Output != obj.Output)
            {
                return false;
            }

            // if (this.Inputs.Count != obj.Inputs.Count)
            // {
            //     return false;
            // }
            //
            // foreach (var resourceData in obj.Inputs)
            // {
            //     if (!this.Inputs.Contains(resourceData))
            //     {
            //         return false;
            //     }
            // }

            return true;
        }

        public static AmbitionContainer Instantiate(AmbitionContainer ambition, AIDirector director)
        {
            // Director = director;
            // Key = FindDbAmbition(ambition);
            return GameObject.Instantiate(ambition);
        }
        // public static AmbitionTemplate operator +(AmbitionTemplate a, AmbitionTemplate b)
        // {
        //     AmbitionTemplate result = new AmbitionTemplate();
        //     result.Key = a.Key;
        //     result.Money = a.Money + b.Money;
        //     result.Factor = a.Factor + b.Factor;
        //     result.FactorSpend = b._factorSpend;
        //     result.ResourceSpend = b.ResourceSpend;
        //     return result;
        // }
    }
}