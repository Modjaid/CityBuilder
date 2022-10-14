using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpdateLayout : MonoBehaviour
    {
        [SerializeField] private RectTransform _directorsRect;
        [SerializeField] private RectTransform _companiesRect;
        [SerializeField] private RectTransform _mailRect;
        [SerializeField] private RectTransform _newsRect;

        public void DirectorsUpdate()
        {
            StartCoroutine(Delay(_directorsRect));
        }
        public void CompaniesUpdate()
        {
            StartCoroutine(Delay(_companiesRect));
        }
        public void MailUpdate(bool isActive)
        {
            StartCoroutine(Delay(_mailRect));
        }
        public void NewsUpdate()
        {
            StartCoroutine(Delay(_newsRect));
        }

        private IEnumerator Delay(RectTransform rect){
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rect);
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rect);
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rect);
        }
    }
}
