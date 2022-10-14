using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace UI
{
    public static class LocalKeys
    {
        private static readonly Dictionary<string, int> _speechVariations = new Dictionary<string, int>()
        {
            {"NewCandidateMessage.Friendly.Slang", 1},
            {"NewCandidateMessage.Friendly.Official", 1},
            {"NewCandidateMessage.Neutral.Slang", 1},
            {"NewCandidateMessage.Neutral.Official", 1},
            {"NewCandidateMessage.Angry.Slang", 1},
            {"NewCandidateMessage.Angry.Official", 1},
            {"CreateNewCompanyMessage.Friendly.Official", 1},
            {"CreateNewCompanyMessage.Friendly.Slang", 1},
            {"CreateNewCompanyMessage.Neutral.Official", 1},
            {"CreateNewCompanyMessage.Neutral.Slang", 1},
            {"CreateNewCompanyMessage.Angry.Official", 1},
            {"CreateNewCompanyMessage.Angry.Slang", 1},
            {"CreateNewCompanyMessage.Friendly.Official.Help", 1},
            {"CreateNewCompanyMessage.Friendly.Slang.Help", 1},
            {"CreateNewCompanyMessage.Neutral.Official.Help", 1},
            {"CreateNewCompanyMessage.Neutral.Slang.Help", 1},
            {"CreateNewCompanyMessage.Angry.Official.Help", 1},
            {"CreateNewCompanyMessage.Angry.Slang.Help", 1},
            
            
            {"NewCandidateResolve.Big.Yes", 3},
            {"NewCandidateResolve.Yes", 1},
            {"NewCandidateResolve.Big.No", 3},
            {"NewCandidateResolve.No", 1},
            {"CreateNewCompanyResolve.Big.Yes", 3},
            {"CreateNewCompanyResolve.Yes", 1},
            {"CreateNewCompanyResolve.Big.No", 3},
            {"CreateNewCompanyResolve.No", 1},
            {"CreateNewCompanyResolve.Big.Require", 3},
            {"CreateNewCompanyResolve.Require", 1},
            {"CreateNewCompanyResolve.Big.Invest", 3},
            {"CreateNewCompanyResolve.Invest", 1},
            {"CreateNewCompanyResolve.Help.Yes", 1},
            {"CreateNewCompanyResolve.Help.Big.Yes", 3},
            {"CreateNewCompanyResolve.Help.Big.No", 3},
            {"CreateNewCompanyResolve.Help.No", 1},
            {"CreateNewCompanyResolve.Help.Big.Require", 3},
            {"CreateNewCompanyResolve.Help.Require", 1},
            {"CreateNewCompanyResolve.Help.Big.Invest", 3},
            {"CreateNewCompanyResolve.Help.Invest", 1},
            
            
            {"CreateNewCompanyAnswer.Invest.Yes.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.Invest.No.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.BigInvest.Yes.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.BigInvest.No.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.Require.Yes.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.Require.No.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.BigRequire.Yes.Neutral.Official", 1},
            {"CreateNewCompanyAnswer.BigRequire.No.Neutral.Official", 1},
            {"CreateNewCompanyAnswer.Yes.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.BigYes.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.No.Friendly.Official", 1},
            {"CreateNewCompanyAnswer.BigNo.Neutral.Official", 1},
            
            
            {"CompanyName.HappyScore", 1},
            {"CompanyName.Citizen", 1},
            {"CompanyName.Dollar", 1},
            {"CompanyName.Food", 5},
            {"CompanyName.Fuel", 1},
            {"CompanyName.Policmen", 1},
            {"CompanyName.Criminal", 1},
            {"CompanyName.FireFighter", 1},
            {"CompanyName.Fire", 1},
            {"CompanyName.Military", 1},
            {"CompanyName.Enemy", 1},
            {"CompanyName.GroundTransport", 1},
            {"CompanyName.FlyTrasport", 1},
            {"CompanyName.WaterTransport", 1},
            {"CompanyName.Culture", 1},
            {"CompanyName.Patriotizm", 1},
            {"CompanyName.Electronics", 1},
            {"CompanyName.Logistics", 1},
            {"CompanyName.Worker", 1},
            {"CompanyName.Engineer", 1},
            {"CompanyName.Doctor", 1},
            {"CompanyName.Lawyer", 1},
            {"CompanyName.Scientist", 1},
            {"CompanyName.Energy", 1},
            {"CompanyName.BuildingMaterials", 1},
            {"CompanyName.MassMedia", 1},
            {"CompanyName.Water", 1},
            {"CompanyName.Garbage", 1},
            {"CompanyName.Parasites", 1},
            {"CompanyName.Medicines", 1},
            {"CompanyName.HighTech", 1},
            {"CompanyName.Liberty", 1},
            {"CompanyName.Religion", 1},
            {"CompanyName.Sand", 1},
            {"CompanyName.Metall", 1},
            {"CompanyName.Cement", 1},
            
            
            {"FirstName", 40},
            {"LastName", 40}
        };

        public static string NewCandidateMessage(Altitude altitude, bool isOfficialSpeech)
        {
            var speechStyle = (isOfficialSpeech) ? "Official" : "Slang";
            var key = $"NewCandidateMessage.{altitude.ToString()}.{speechStyle}";
            Validate(key);
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations[key] + 1);
            return $"{key}.{randomVariant}";
        }
        
        public static string CreateNewCompanyMessageWithHelp(Altitude altitude, bool isOfficialSpeech)
        {
            var speechStyle = (isOfficialSpeech) ? "Official" : "Slang";
            var key = $"CreateNewCompanyMessage.{altitude.ToString()}.{speechStyle}.Help";
            Validate(key);
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations[key] + 1);
            return $"{key}.{randomVariant}";
        }

        public static string CreateNewCompanyMessage(Altitude altitude, bool isOfficialSpeech)
        {
            var speechStyle = (isOfficialSpeech) ? "Official" : "Slang";
            var key = $"CreateNewCompanyMessage.{altitude.ToString()}.{speechStyle}";
            Validate(key);
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations[key] + 1);
            return $"{key}.{randomVariant}";
        }

        public static string FirstName()
        {
            Validate("FirstName");
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations["FirstName"] + 1);
            return $"FirstName.{randomVariant}";
        }
        public static string LastName()
        {
            Validate("LastName");
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations["LastName"] + 1);
            return $"LastName.{randomVariant}";
        }
        public static string CompanyName(ResourceType outputRes)
        {
            var baseKey = $"CompanyName.{outputRes.ToString()}";
            Validate(baseKey);
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations[baseKey] + 1);
            return $"{baseKey}.{randomVariant}";
        }

        public static string CreateNewCompanyAnswer(ResolveType resolveType, Altitude altitude, bool isOfficialSpeech, bool addAnswer)
        {
            if ((resolveType == ResolveType.BigRequire || resolveType == ResolveType.BigNo) && altitude == Altitude.Friendly)
            {
                altitude = Altitude.Neutral;
            }
            var speechStyle = (isOfficialSpeech) ? "Official" : "Slang";
            var baseKey = $"";
            if (resolveType == ResolveType.Invest || resolveType == ResolveType.Require ||
                resolveType == ResolveType.BigRequire || resolveType == ResolveType.BigInvest)
            {
                var strAnswer = (addAnswer) ? "Yes" : "No";
                baseKey = $"CreateNewCompanyAnswer.{resolveType}.{strAnswer}.{altitude}.{speechStyle}";
            }
            else
            {
                baseKey = $"CreateNewCompanyAnswer.{resolveType}.{altitude}.{speechStyle}";
            }
            Validate(baseKey);
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations[baseKey] + 1);
            return $"{baseKey}.{randomVariant}";
        }
        public static string NewCandidateResolve(bool yes)
        {
            var endKey = (yes) ? "Yes" : "No";
            var key = $"NewCandidateResolve.Big.{endKey}";
            Validate(key);
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations[key] + 1);
            return $"{key}.{randomVariant}";
        }
        
        public static string CreateNewCompanyResolve(bool isHelpQuery, ResolveType dialog)
        {
            var key = (isHelpQuery) ? "Help" : "";
            key = $"CreateNewCompanyResolve.{key}.{dialog.ToString()}";
            Validate(key);
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations[key] + 1);
            return $"{key}.{randomVariant}";
        }

        public static string GetRandomVariant(string key)
        {
            Validate(key);
            var randomVariant = UnityEngine.Random.Range(1, _speechVariations[key] + 1);
            return $"{key}.{randomVariant}";
        }

        private static void Validate(string key)
        {
            if (!_speechVariations.ContainsKey(key))
            {
                Debug.LogError($"Ключа:{key} нет в LocalKeys");
            }
        }
    }
}
