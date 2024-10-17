using BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string name;
    public Dictionary<string, Dialogue> dialogues;

    [System.Serializable]
    public class Dialogue
    {
        public Line[] lines; // 여러 줄의 대사
        public Choice[] choices; // 각 대화에 대한 선택지
    }

    [System.Serializable]
    public class Line
    {
        public string line; // 대사 텍스트
    }

    [System.Serializable]
    public class Choice
    {
        public string choiceText;
        public int nextDialogueId;
        public string action;
        public string requiredQuestState;
        public string questId;
        public bool isVisible;
    }
}