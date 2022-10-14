using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace GameData
{
    public static class GameData
    {
        public static readonly Vector3 TEMPLATE_SCALE = new Vector3(0.01f, 0.01f, 0.01f);
        public static float MAIN_SCALE = 0.5f;
        public const float FLOOR_HEIGHT = 2.5f;


        public static readonly ResourceType[][] BuildingPlacements =
        {
            //Manufacture
            new[] {ResourceType.Worker, ResourceType.Engineer},
            //House
            new[] {ResourceType.Citizen},
            //Farm
            new[] {ResourceType.Worker},
            //Office
            new[]
            {
                ResourceType.Worker, ResourceType.Engineer, ResourceType.Doctor,
                ResourceType.Lawyer, ResourceType.Policmen
            },
            //Laboratory
            new[] {ResourceType.Scientist},
            //ArmyBase
            new[] {ResourceType.Military},
            //Chaurch
            new[] {ResourceType.Worker}
        };

        public static readonly ResourceType[] Employers =
        {
            ResourceType.Doctor,
            ResourceType.Engineer,
            ResourceType.Lawyer,
            ResourceType.Policmen,
            ResourceType.Worker,
            ResourceType.Scientist,
            ResourceType.Military
        };

        public static readonly ResourceType[] Stuff =
        {
            ResourceType.Citizen,
            ResourceType.Doctor,
            ResourceType.Engineer,
            ResourceType.Lawyer,
            ResourceType.Policmen,
            ResourceType.Worker,
            ResourceType.Scientist,
            ResourceType.Military
        };

        public static readonly ResourceType[] Human =
        {
            ResourceType.Citizen,
            ResourceType.Doctor,
            ResourceType.Engineer,
            ResourceType.Lawyer,
            ResourceType.Policmen,
            ResourceType.Worker,
            ResourceType.Scientist,
            ResourceType.Military
        };

        public static readonly ResourceType[] PositiveRes =
        {
            ResourceType.Food,
            ResourceType.Fuel,
            ResourceType.GroundTransport,
            ResourceType.FlyTrasport,
            ResourceType.WaterTransport,
            ResourceType.Culture,
            ResourceType.Patriotizm,
            ResourceType.Electronics,
            ResourceType.Logistics,
            ResourceType.Energy,
            ResourceType.Water,
            ResourceType.Medicines,
            ResourceType.Doctor,
            ResourceType.Lawyer,
            ResourceType.Engineer,
            ResourceType.HighTech,
            ResourceType.Liberty,
        };

        public static readonly ResourceType[] NegativeRes =
        {
            (ResourceType.Criminal),
            (ResourceType.Fire),
            (ResourceType.Enemy),
            (ResourceType.Parasites),
            (ResourceType.Garbage)
        };

        public static bool IsChance(this int chance) => (UnityEngine.Random.Range(1, chance + 1) == chance);

        public static bool IsThing(this ResourceType type)
        {
            return !Array.Exists(Stuff, (x) => x == type);
        }

        public static bool IsHuman(this ResourceType type)
        {
            return Array.Exists(Human, (x) => x == type);
        }

        public static bool IsStuff(this ResourceType type)
        {
            return Array.Exists(Stuff, (x) => x == type);
        }

        public static bool IsEmployer(this ResourceType type)
        {
            return Array.Exists(Employers, (x) => x == type);
        }
    }

    public enum BuildingType
    {
        Manufacture,
        House,
        Farm,
        Office,
        Laboratory,
        ArmyBase,
        Church
    }
    public enum Altitude{
        Friendly,
        Neutral,
        Angry
    }

    public enum ResourceType
    {
        HappyScore = 0,
        Citizen,
        Dollar,
        Food,
        Fuel,
        Policmen,
        Criminal,
        FireFighter,
        Fire,
        Military,
        Enemy,
        GroundTransport,
        FlyTrasport,
        WaterTransport,
        Culture,
        Patriotizm,
        Electronics,
        Logistics,
        Worker,
        Engineer,
        Doctor,
        Lawyer,
        Scientist,
        Energy,
        BuildingMaterials,
        MassMedia,
        Water,
        Garbage,
        Parasites,
        Medicines,
        HighTech,
        Liberty,
        Religion,
        Sand,
        Metall,
        Cement,
    }
}
