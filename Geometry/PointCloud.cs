// https://bitbucket.org/alkee/aus

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using aus.Extension;

namespace aus.Geometry
{
    public class PointCloud
    {
        public int Count { get; private set; }
        public Vector3[] Points { get; private set; }
        public Vector3[] Normals { get; private set; }
        public Bounds Bounds { get; private set; }

        private readonly Octree<int/*point index*/> octree;

        public PointCloud(Mesh src)
        {
            Points = src.vertices;
            Count = Points.Length;
            Normals = src.normals;
            Bounds = src.bounds;
            if (Normals != null && Normals.Length != Count)
            {
                Debug.LogWarning("Pointcloud has invalid normals");
                Normals = null; // TODO: recalculate normal ?
            }

            // octree calculation
            var diagonalLength = Bounds.DiagonalLength();
            octree = new Octree<int>(diagonalLength, Bounds.center
                , diagonalLength / 100); // cloud comapre 의 기본 geometry radius; https://bitbucket.org/alkee_skia/mars3/issues/230/scene-idea#comment-60583560
            for (var i = 0; i < Count; ++i) octree.Add(i, Points[i]);
        }

        public List<int/*point index*/> GetPointIndices(Vector3 center, float radius)
        {
            return octree.GetNearBy(center, radius);
        }

        public List<Vector3> GetPoints(IEnumerable<int> indices)
        {
            if (indices == null) throw new ArgumentNullException(nameof(indices));
            return indices.Select(x => Points[x]).ToList();
        }

        public List<Vector3> GetPoints(Vector3 center, float radius)
        {
            var indices = GetPointIndices(center, radius);
            return GetPoints(indices);
        }

        public List<int/*point index*/> GetPointIndices(Ray ray, float distance)
        {
            return octree.GetNearBy(ray, distance);
        }

        public List<Vector3> GetPoints(Ray ray, float distance)
        {
            var indices = GetPointIndices(ray, distance);
            return GetPoints(indices);
        }
    }
}