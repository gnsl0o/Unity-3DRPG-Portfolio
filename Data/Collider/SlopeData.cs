using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlopeData
{
    [field: SerializeField][field: Range(0f, 1f)] public float StepHeightPecentage { get; private set; } = 0.25f;
    [field: SerializeField][field: Range(0f, 5f)] public float FloatRayDistacne { get; private set; } = 2f;

    [field: SerializeField][field: Range(0f, 50f)] public float StepReachForce { get; private set; } = 25f;
}
