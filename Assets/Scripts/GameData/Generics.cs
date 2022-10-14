
using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Companies;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace GameData
{


    /// <summary>
    /// (ключ - значение) коллекция с характером отслеживания главного значения(Value) в Data структуре
    /// </summary>
    /// <typeparam name="Key">ключ проходит сравнения с другими ключами в коллекции через IsKey()</typeparam>
    /// <typeparam name="Data">Решающее значение - Value для авто add/remove в коллекцию, все остальные переменные как допы, </typeparam>
    [Serializable]
    public class List<Key, Data> where Data : IKeyValue<Key>, new()
    {
        [SerializeField] private List<Data> _list;

        public List<Key> Keys
        {
            get
            {
                var results = new List<Key>();
                foreach (var data in _list)
                {
                    results.Add(data.Key);
                }

                return results;
            }
        }

        public int Count => _list.Count;

        public Data GetMaxValue
        {
            get
            {
                Data max = new Data();
                foreach (var data in _list)
                {
                    if (data.Value > max.Value)
                    {
                        max = data;
                    }
                }

                return max;
            }
        }

        public List()
        {
            _list = new List<Data>();
        }

        public List(IEnumerable<Data> arr)
        {
            _list = new List<Data>(arr);
        }

        /// <param name="value"> if(less 0) - auto remove() else auto add() </param>
        /// <returns>(value) || (currentValue - value)</returns>
        public float Add(Data newData)
        {
            var index = GetIndex(newData.Key);
            if (index == -1)
            {
                if (newData.Value < 0)
                {
                    newData.Value = 0;
                    _list.Add(newData);
                    
                    if (newData.Key == null)
                    {
                        Debug.Log($"NULL");
                    }
                    return 0;
                }

                _list.Add(newData);
                return newData.Value;
            }

            float sum = _list[index].Value + newData.Value;
            if (sum <= 0)
            {
                newData.Value = 0f;
                _list.RemoveAt(index);
                return newData.Value - sum;
            }
            newData.Value = sum;
            _list[index] = newData;
            return newData.Value;
        }

        /// <param name="value"> if(less 0) - auto remove() else auto add() </param>
        /// <returns>(value) || (currentValue - value)</returns>
        public float Add(Key key, float value)
        {
            var index = GetIndex(key);
            if (index == -1)
            {
                var newData = new Data();
                if (value > 0)
                {
                    newData.Value = value;
                    newData.Key = key;
                    _list.Add(newData);
                    
                    if (newData.Key == null)
                    {
                        Debug.Log($"NULL");
                    }
                    return value;
                }

                return 0;
            }

            var oldData = _list[index];
            float sum = oldData.Value + value;
            if (sum <= 0)
            {
                oldData.Value = 0;
                _list.RemoveAt(index);
                return value - sum;
            }

            oldData.Value = sum;
            _list[index] = oldData;
            return value;
        }

        public Data GetData(Key key)
        {
            var index = GetIndex(key);
            var data = new Data();
            if (index != -1)
            {
                data = _list[index];
            }

            return data;
        }

        public bool TryGetByIndex(int index, out Data data)
        {
            if (_list.Count > index)
            {
                data = _list[index];
                return true;
            }

            data = new Data();
            return false;
        }

        public bool TryGetData(Key key, out Data data)
        {
            var index = GetIndex(key);
            if (index == -1)
            {
                data = new Data();
                data.Key = key;
                return false;
            }

            data = _list[index];
            return true;
        }

        public bool TryGetData(Predicate<Data> match, out Data result)
        {
            result = _list.Find(match);
            if (result != null)
            {
                return true;
            }

            return false;
        }
    

        public void Sort(bool isMaxValue)
        {
            if (isMaxValue)
            {
                _list.Sort((x,y) => x.Value.CompareTo(y.Value));
            }
            else
            {
                _list.Sort((x,y)=> y.Value.CompareTo(x.Value));
            }
        }
        

        public bool Contains(Key key)
        {
            var index = GetIndex(key);
            if (index == -1)
            {
                return false;
            }

            return true;
        }

        public float GetValue(Key key)
        {
            float result = 0;
            if (TryGetData(key, out var data))
            {
                result = data.Value;
            }

            return result;
        }

        /// <summary>
        /// Простое добавление в коллекцию без Value условий
        /// </summary>
        public void Set(Key key, float value)
        {
            var index = GetIndex(key);
            if (index == -1)
            {
                var newData = new Data();
                newData.Key = key;
                newData.Value = value;
                _list.Add(newData);
                
                if (newData.Key == null)
                {
                    Debug.Log($"NULL");
                }
                return;
            }

            var data = _list[index];
            var oldValue = data.Value;
            data.Value = value;
            _list[index] = data;
        }

        /// <summary>
        /// Простое добавление в коллекцию без Value условий
        /// </summary>
        public void Set(Data data)
        {
            var index = GetIndex(data.Key);
            if (index == -1)
            {
                _list.Add(data);
                
                
                if (data.Key == null)
                {
                    Debug.Log($"NULL");
                }
                return;
            }
            var oldValue = _list[index].Value;
            _list[index] = data;
        }

        public float Clear(Key key)
        {
            if (TryGetData(key, out var data))
            {
                _list.Remove(data);
                return data.Value;
            }
            return 0;
        }
        public void Clear(){
            _list.Clear();
        }

        private int GetIndex(Key key)
        {
            return _list.FindIndex((x) => x.IsKey(key));
        }

        public float GetSumValues()
        {
            float result = 0;
            foreach (var item in _list)
            {
                result += item.Value;
            }
            
            return result;
        }
        

        public List<Data> ToList()
        {
            return new List<Data>(_list);
        }

        public void Foreach(Action<Data> handler)
        {
            for(int i = 0; i < _list.Count; i++){
                handler(_list[i]);
            }
        }

        public void Foreach(Func<Data,Data> handler){
            for(int i = 0; i < _list.Count; i++){
              _list[i] = handler(_list[i]);
            }
        }
    }

    public interface IKeyValue<T>
    {
        T Key { get; set; }
        float Value { get; set; }
        bool IsKey(T key);
    }

    /// <summary>
    /// Универсальная структура для List(Key,Data) коллекции на случай если кроме Value никаких доп данных ни требуется 
    /// </summary>
    /// <typeparam name="K">Ключ для List(Key,Data) коллекции</typeparam>
    [Serializable]
    public struct FloatKeyData<K> : IKeyValue<K>
    {
        [SerializeField] private K _key;
        [SerializeField] private float _value;

        public K Key
        {
            get => _key;
            set => _key = value;
        }

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public bool IsKey(K key)
        {
            return EqualityComparer<K>.Default.Equals(key, _key);
        }
    }

    public static class ListExt
    {
        // public static float GetAllHumans(this List<ResourceType, ResourceData> library)
        // {
        //     var people = 0f;
        //     foreach (var citizenType in GameData.HumanTypes)
        //     {
        //         people += library.GetValue(citizenType);
        //     }

        //     return people;
        // }

        // public static void ChangeHumansByPercent(this List<ResourceType, ResourceData> resources,
        //     Action<ResourceType, float> handler, float percent)
        // {
        //     foreach (var type in GameData.HumanTypes)
        //     {
        //         var delta = (resources.GetValue(type) / 100f) * percent;
        //         delta = resources.Add(type, (int) Math.Round(delta));
        //         handler(type, delta);
        //     }
        // }
        // public static void ChangeHumans(this List<Building, OfficeData> offices, 
        public static float Add(this List<ResourceType, ResourceData> resus,ResourceType key, float value, float price){
           var added = resus.Add(key, value);
            if(resus.TryGetData(key, out var data)){
                 data.Price = price;
                 resus.Set(data);
            }
            return added;
        }
        public static void ChangeStuff(this List<Building, FloatKeyData<Building>> offices, BuildingType type, float count, Action<Building, float> handler)
        {
            var list = offices.ToList();
            var delta = 0f;
            foreach (var office in list)
            {
                if (type == office.Key.Type)
                {
                    delta = offices.Add(office.Key, count);
                    handler(office.Key, delta);

                    count -= Math.Abs(delta);
                    if(count <= 0) return;
                }
            }
        }
        public static void ChangeStuff(this List<Building, FloatKeyData<Building>> offices, float count, Action<Building, float> handler)
        {
            var list = offices.ToList();
            var delta = 0f;
            foreach (var office in list)
            {
                delta = offices.Add(office.Key, count);
                handler(office.Key, delta);

                count -= Math.Abs(delta);
                if(count <= 0) return;
            }
        }

        public static List<AmbitionData> ToListByType(this List<AmbitionContainer, AmbitionData> ambitions, params ResourceType[] types){
            var result = new List<AmbitionData>();
            ambitions.Foreach((amb) => 
            {
                if(types.Contains(amb.Input)){
                    result.Add(amb);
                }
            });
            return result;
        }
        public static List<AmbitionData> ToListByExceptType(this List<AmbitionContainer, AmbitionData> ambitions, params ResourceType[] types){
            var result = new List<AmbitionData>();
            ambitions.Foreach((amb) => 
            {
                if(!types.Contains(amb.Input)){
                    result.Add(amb);
                }
            });
            return result;
        }

        public static bool TryGetAmbition<T>(this Transform transform, out T result) where T : MonoBehaviour
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<T>(out result))
                {
                    return true;
                }
            }
            result = null;
            return false;
        }
        public static bool TryGetAmbition(this Transform transform, AmbitionContainer key, out AmbitionContainer result)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<AmbitionContainer>(out result))
                {
                    if (result.Key == key)
                    {
                        return true;
                    }
                }
            }
            result = null;
            return false;
        }

        public static void AmbitionForeach<T>(this Transform transform, Action<T> handler) where T : MonoBehaviour
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<T>(out var result))
                {
                    handler(result);
                }
            }
        }

        public static bool TryGetAmbitions<T>(this Transform transform, out List<T> results) where T : MonoBehaviour
        {
            results = new List<T>();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<T>(out var result))
                {
                    results.Add(result);
                }
            }
            return results.Count > 0;
        }
        
        public static List<AmbitionContainer> GetAmbitions(this Transform transform)
        {
            var list = transform.GetComponentsInChildren<AmbitionContainer>();
            if (list.Length > 0)
            {
                return list.ToList();;
            }
            
            return new List<AmbitionContainer>();
        }
        

        // public static float UpdStuff(this List<Building, SpaceData> spaces, float newStuffCount, ResourceType stuffType)
        // {
        //     var currentStuffCount = spaces.GetSumStuff(stuffType);
        //     var part = currentStuffCount / newStuffCount;
        //     if (part < 1)
        //     {
        //         spaces.Foreach((space) =>
        //         {
        //             space.Stuff 
        //         });
        //     }
        // }

        public static float GetSumValues(this List<Building, SpaceData> offices, BuildingType buildingType)
        {
            var result = 0f;
            offices.Foreach((office) =>
            {
                if (office.Key.Type == buildingType)
                {
                    result += office.Value;
                }
            });
            return result;
        }


        public static float GetSumFactors(this List<AmbitionContainer, AmbitionData> ambitions, ResourceType resType)
        {
            var result = 0f;
            ambitions.Foreach((amb) =>
            {
                if(amb.Input == resType){
                    result += amb.Factor;
                }
            });
            return result;
        }

        public static float GetSumStuff(this List<ResourceType, ResourceData> resources)
        {
            var result = 0f;
            resources.Foreach((res) =>
            {
                if (res.Key.IsStuff())
                {
                    result += res.Value;
                }
            });
            return result;
        }

        public static float GetAllPlacements(this List<SpaceData> offices, BuildingType buildingType)
        {
            var placements = 0f;
            foreach(var office in offices){
                if (office.Key.Type == buildingType)
                {
                    placements += office.Value;
                }
            }
            return placements;
        }

        public static float GetAllPlacements(this List<SpaceData> offices, ResourceType type)
        {
            var placements = 0f;
            foreach(var office in offices){
                if (office.Key.IsSuitType(type))
                {
                    placements += office.Value;
                }
            }
            return placements;
        }
        

        // public static void ChangeAllValuesByPercent(this List<Company> library, float percent)
        // {
        //     foreach (var companyData in library.GetDictionary())
        //     {
        //         var delta = (companyData.Value / 100f) * percent;
        //         library.AddValue(companyData.Key, delta);
        //     }
        // }
    }
}
