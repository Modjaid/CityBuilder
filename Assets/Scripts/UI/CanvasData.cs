using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "CanvasData", menuName = "GameData/CanvasData", order = 1)]
public class CanvasData : ScriptableObject 
{
   [SerializeField] public Color PositiveColor;
   [SerializeField] public Color NegativeColor;
   [SerializeField] public GameObject ButtonPrefab;

   [SerializeField] private Sprite[] _directorAvatars;
   [SerializeField] private CompanyAvatars[] _companyAvatars;
   
   public Sprite RandomDirectorAvatar => _directorAvatars[Random.Range(0, _directorAvatars.Length)];

   public Sprite RanomCompanyAvatar(ResourceType type) => _companyAvatars[(int) type].GetRanomAvatar;

   [ContextMenu("Initialize Sections")]
   public void InitMarketSection(){
      var valuesAsArray = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList();
      _companyAvatars = new CompanyAvatars[valuesAsArray.Count];
      for(int i = 0; i < valuesAsArray.Count; i++){
         _companyAvatars[i] = new CompanyAvatars(valuesAsArray[i].ToString());
      }
   }

   [Serializable]
   public class CompanyAvatars
   {
      public string SectionName;
      public Sprite[] Avatars;

      public CompanyAvatars(string name)
      {
         SectionName = name;
      }

      public Sprite GetRanomAvatar => Avatars[Random.Range(0, Avatars.Length)];
   }
}

