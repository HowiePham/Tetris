using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ShapeReader : TemporaryMonoSingleton<ShapeReader>
{
    [SerializeField] private List<TextAsset[]> _shapeTextAssetList;
    [SerializeField] private TextAsset[] _shapeTextAsset;

    [SerializeField] private bool _shoudRandom;

    [SerializeField] private int defaultShape;

    private Dictionary<int, List<Shape>> _shapeListDict;

    protected override void Awake()
    {
        base.Awake();
        InitializeTextAssetList();
    }

    private void InitializeTextAssetList()
    {
        _shapeTextAssetList = new List<TextAsset[]>();

        _shapeTextAssetList.Add(_shapeTextAsset);
    }

    private void Start()
    {
        ReadShape();
    }

    public Shape GetShape()
    {
        List<Shape> shapeList = _shapeListDict[0];
        int randomIndex = Random.Range(0, shapeList.Count * 1000) % shapeList.Count;

        return (Shape)shapeList[randomIndex].Clone();
    }

    private void ReadShape()
    {
        _shapeListDict = new Dictionary<int, List<Shape>>();

        Random.InitState((int)DateTime.Now.Ticks);
        var csvReader = new CsvReader();

        for (int i = 0; i < _shapeTextAssetList.Count; i++)
        {
            List<Shape> shapeList = new List<Shape>();

            var textAsset = _shapeTextAssetList[i];

            foreach (var shapeTextAsset in textAsset)
            {
                var shapeData = csvReader.ReadCsv(shapeTextAsset.text);
                shapeList.Add(new Shape(shapeData, shapeTextAsset.name));
                // print(shapeData);
            }

            _shapeListDict.Add(i, shapeList);
        }
    }
}