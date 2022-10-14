using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Initializator;
using Managers;
using UnityEngine;

public class TestExecutors : MonoBehaviour
{
    [TextArea(0,1000)]
    [SerializeField] private string resoursLog;
    [SerializeField] private City _city;
    // [SerializeField] private List<ExecutorInit> inits;
    [SerializeField] private List<Executer> _executors;

    void Start()
    {
        _city.Init();
        // foreach(ExecutorInit<Executer> item in inits){
        //     // _executors.Add(item.CreateAndInit());
        // }
        
        foreach (var exe in _executors)
        {
            // exe.Start();
        }

        // StartCoroutine(DebugResources());
    }

    // public IEnumerator DebugResources(){
    //     while(true){
    //          var resus = _city.Resources.GetDictionary();
    //          resoursLog = "";
    //          foreach (var kv in resus)
    //          {
    //              if(kv.Value == 0) continue;
    //              resoursLog += $"{kv.Key} = {kv.Value}\n";
    //          }
    //           yield return new WaitForSeconds(1);
    //     }
    // }
}
