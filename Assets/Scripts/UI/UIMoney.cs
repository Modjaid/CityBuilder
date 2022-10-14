using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

public class UIMoney : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Upd());
    }

    private IEnumerator Upd()
    {
        var player = FindObjectOfType<Player>();
        var output = GetComponent<TextMeshProUGUI>();
        while (true)
        {
            yield return new WaitForSeconds(1);
            output.text = $"{(int)player.Money}$";
        }
    }
    
}
