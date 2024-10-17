using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    public float damage;
    public float cool;
    public string animationName;
    public Sprite Icon;
    public AudioClip soundEffect;
}