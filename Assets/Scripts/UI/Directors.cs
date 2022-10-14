using System.Collections;
using System.Collections.Generic;
using Managers;
using Managers.Com;
using UI;
using UnityEngine;

namespace UI
{
    public class Directors : MonoBehaviour
    {
        [SerializeField] private GameObject _directorMessagePrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private City _city;

        public void Awake()
        {
            _city.OnChangeExecutors += OnChangeCityExecutors;
        }

        public void OnChangeCityExecutors(bool isAdd, Executer exe)
        {
            if (isAdd)
            {
                if (exe is AIDirector)
                {
                    var message = Instantiate(_directorMessagePrefab, _content)
                        .GetComponent<UIDirectorMessage>();
                    message.Init(exe as  AIDirector);
                }
            }
        }
    }
}
