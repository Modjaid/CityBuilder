using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProperty : MonoBehaviour
{
    public static LevelProperty Instance;

    [SerializeField] public CanvasData Canvas;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
