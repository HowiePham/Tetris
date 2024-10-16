using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BlockColorTemplate", order = 1)]
public class BlockColorTemplate : ScriptableObject
{
    [SerializeField] private List<Sprite> blockColorTemplateList = new List<Sprite>();

    public List<Sprite> BlockColorTemplateList => blockColorTemplateList;
}