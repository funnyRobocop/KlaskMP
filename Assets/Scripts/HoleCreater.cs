using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCreater : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D hole2DCollider2;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider GeneratedMeshCollider;
    private Mesh GeneratedMesh;

    private void Start()
    {
        hole2DCollider.transform.position = new Vector2(0f, 7.5f);
        hole2DCollider2.transform.position = new Vector2(0f, -7.5f);
        MakeHole2D();
        Make3DMeshCollider();
    }

    private void MakeHole2D()
    {
        Vector2[] pointPositions = hole2DCollider.GetPath(0);

        for (int i = 0; i < pointPositions.Length; i++)
        {
            pointPositions[i] = hole2DCollider.transform.TransformPoint(pointPositions[i]);
        }

        Vector2[] pointPositions2 = hole2DCollider2.GetPath(0);

        for (int i = 0; i < pointPositions2.Length; i++)
        {
            pointPositions2[i] = hole2DCollider2.transform.TransformPoint(pointPositions2[i]);
        }

        ground2DCollider.pathCount = 3;
        ground2DCollider.SetPath(1, pointPositions);
        ground2DCollider.SetPath(2, pointPositions2);
    }

    private void Make3DMeshCollider()
    {
        GeneratedMesh = ground2DCollider.CreateMesh(true, true);
        GeneratedMeshCollider.sharedMesh = GeneratedMesh;
    }
}

