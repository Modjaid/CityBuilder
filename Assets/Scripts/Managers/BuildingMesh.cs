using System;
using Cysharp.Threading.Tasks;
using External;
using PointMap;
using UnityEngine;

namespace Managers{
    public class BuildingMesh : MonoBehaviour
    {
        [SerializeField] public Point Size;
        [SerializeField] private Transform _platform;
        [SerializeField] private Transform _platformMesh;
        [SerializeField] private Transform _model;
        [SerializeField] private OnGroundTriggerStay _groundTrigger;

        public async UniTask Grow()
        {
            var targetScale = _model.localScale;
            var scale = _model.localScale;
            scale.y = 0;
            _model.localScale = scale;
            // await UniTask.WaitUntil(()=> _groundTrigger.IsStay);
            this.transform.position = this.transform.GetSurfacePos();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
    
            while(_groundTrigger.IsStay){
                this.transform.position += Vector3.up * Time.deltaTime;
                await UniTask.Yield();
            }
            while(_model.localScale.y < targetScale.y){
                _model.localScale += Vector3.up * Time.deltaTime * 0.1f;
                await UniTask.Yield();
            }
            _model.localScale = targetScale;
        }
        
        
        [ContextMenu("Rotate")]
        public void Rotate90()
        {
            transform.Rotate(Vector3.up, 90, Space.Self);
        }
    
        [ContextMenu("Scaling")]
        public void Scaling()
        {
            var newPlatformScale = _platform.localScale;
            newPlatformScale = new Vector3(Size.x, newPlatformScale.y, Size.y);
            _platform.localScale = newPlatformScale;
            var half = newPlatformScale.y / 2;
            var pos = new Vector3(newPlatformScale.x / 2, half, -newPlatformScale.z / 2);
            _platform.localPosition = pos;
    
            _model.localPosition = _platform.localPosition + (Vector3.up * half);
        }
    }
}
