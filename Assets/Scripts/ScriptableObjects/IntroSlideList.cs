using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IntroSlideList", menuName = "ScriptableObjects/IntroSlideList", order = 1)]
[System.Serializable]
public class IntroSlideList : ScriptableObject
{
    public List<UIIntroStruct> slidesList = new List<UIIntroStruct>();

    // Structs
    [System.Serializable]
    public struct UIIntroStruct
    {
        public Sprite image;
        public List<string> textList;
    }
}
