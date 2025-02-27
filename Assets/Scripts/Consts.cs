using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    public const float FieldWidth = 14f;
    public const float FieldHeight = 18f;
    public const float StrikerRadius = 0.35f;
    public const float BallRadius = 0.25f;
    public const float MagnetRadius = 0.25f;
    public const float MaxStrikerSpeed = 33f;
    public const int MaxPoints = 6;
    public const int MaxWins = 2;
    public const float MaxAIStateChangeTime = 3f;
    public static float[] PointsPosZ = { 0.05f, 0.11f, 0.17f, 0.23f, 0.29f, 0.35f, 0.41f, 0.05f };
    public static Vector3[] BallStartPos = { new Vector3(-5.5f, 0.75f, -7.5f), new Vector3(5.5f, 0.75f, -7.5f), new Vector3(-5.5f, 0.75f, 7.5f), new Vector3(5.5f, 0.75f, 7.5f) };
}
