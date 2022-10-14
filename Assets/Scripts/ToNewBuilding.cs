using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class ToNewBuilding : MonoBehaviour
{
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _speed;

    private Coroutine _moving;

    private void OnEnable()
    {
        FindObjectOfType<City>().OnChangeBuildings += LerpToTarget;
        // StartCoroutine(InputListening());
    }

    private void OnDisable()
    {
        FindObjectOfType<City>().OnChangeBuildings -= LerpToTarget;
    }

    private void LerpToTarget(bool isAdd, Building target)
    {
        if (_moving != null)
        {
            StopCoroutine(_moving);
        }
        _moving = StartCoroutine(GoTo(target.transform.position));
    }

    // private IEnumerator InputListening()
    // {
    //     while (true)
    //     {
    //         if(Input.GetKeyDown(KeyCode.Mouse0))
    //         {
    //             RaycastHit hit;
    //             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //             if (Physics.Raycast(ray, out hit))
    //             {
    //                 Debug.Log($"Go TO");
    //                yield return StartCoroutine(GoTo(hit.point));
    //                Debug.Log($"Finish");
    //
    //             }
    //         }
    //         yield return null;
    //     }
    // }

    private IEnumerator GoTo(Vector3 target)
    {
        float distance = float.MaxValue;
        while (distance > _stopDistance)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * _speed);

            distance = Vector3.Distance(transform.position, target);
            yield return null;
        }

        _moving = null;
    }
}
