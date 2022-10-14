using System;
using Assets.SimpleLocalization;
using GameData;
using Managers;
using UI;
using UnityEngine;

[CreateAssetMenu(fileName = "ExecuterAsset", menuName = "GameData/", order = 1)]
public class ExecuterAsset : MonoBehaviour
{
    [SerializeField] private GameObject _localCompanyPrefab;
    [SerializeField] private GameObject _directorPrefab;

    private static ExecuterAsset inst;


    private Transform _directors;
    private Transform _companies;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            _directors = new GameObject("Directors").transform;
            _companies = new GameObject("Companies").transform;
        }
    }

    public AIDirector CreateDirector()
    {
        var director = Instantiate(_directorPrefab, _directors).GetComponent<AIDirector>();
        director.FirstName = LocalKeys.FirstName();
        director.LastName = LocalKeys.LastName();
        director.Avatar = LevelProperty.Instance.Canvas.RandomDirectorAvatar;
        director.gameObject.name = $"{director.FirstName} {director.LastName}";
        return director;
    }

    public static LocalCompany CreateLocalCompany(AIDirector dir, AmbitionContainer ambition, string companyNameKey)
    {
        var company = Instantiate(inst._localCompanyPrefab).GetComponent<LocalCompany>();
        company.transform.parent = inst._companies;
        // company.Soviet.Directors.Add(dir, 100f);
        var name = $"{ambition.Input}-{ambition.Output}-{ambition.WorkPlace}";
        company.FirstName = companyNameKey;
        company.name = name;
        company.SetMainDirector(dir);
        company.Init();
        return company;
    }


}