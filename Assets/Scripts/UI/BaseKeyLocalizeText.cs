
using Assets.SimpleLocalization;
using UnityEngine;

namespace UI
{
    public class BaseKeyLocalizeText : LocalizeText
    {
        [SerializeField] protected string _baseKey;

        public string BaseKey
        {
            set { _baseKey = value; }
        }

        public override void Init()
        {
            Key = LocalKeys.GetRandomVariant(_baseKey);
            base.Init();
        }

    }
}