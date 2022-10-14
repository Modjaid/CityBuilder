using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundTriggerStay : MonoBehaviour
{
    public bool IsStay;
    private int _groundLayer;
    private int _oceanLayer;

    private void Start(){
        _groundLayer = LayerMask.NameToLayer("Ground");
        _oceanLayer = LayerMask.NameToLayer("Water");
        IsStay = false;
    }
    public void OnTriggerStay(Collider collider){
        if(collider.gameObject.layer == _groundLayer || collider.gameObject.layer == _oceanLayer){
            IsStay = true;

        }
    }
    public void OnTriggerExit(Collider collider){
        IsStay = false;
    }
}
