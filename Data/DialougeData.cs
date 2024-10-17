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
        public Line[] lines; // ���� ���� ���
        public Choice[] choices; // �� ��ȭ�� ���� ������
    }

    [System.Serializable]
    public class Line
    {
        public string line; // ��� �ؽ�Ʈ
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