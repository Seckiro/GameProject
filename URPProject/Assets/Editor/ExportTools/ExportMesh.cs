using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class ExportMesh
{
    /// <summary>
    /// 到处数据格式
    /// </summary>
    public enum ExportFileType
    {
        SingleFile,
        MultipleFiles,
    }

    public struct MeshData
    {
        public Mesh mesh;
        public string name;
        public Vector3 centerOffset;
    }

    public static void ExportSkinnedMeshRendererToObj(List<SkinnedMeshRenderer> listSkinnedMeshRenderer, string path, ExportFileType exportFileType = ExportFileType.SingleFile)
    {
        if (listSkinnedMeshRenderer == null || listSkinnedMeshRenderer.Count <= 0)
        {
            Debug.LogError("listSkinnedMeshRenderer is null");
        }

        Vector3 fristMeshPos = listSkinnedMeshRenderer[0].transform.position;

        List<MeshData> listMeshDatas = new List<MeshData>();

        foreach (var item in listSkinnedMeshRenderer)
        {
            //Mesh mesh = new Mesh();
            //item.BakeMesh(mesh);

            listMeshDatas.Add(new MeshData()
            {
                name = item.name,
                mesh = item.sharedMesh,
                centerOffset = fristMeshPos - item.transform.position,
            });
        }
        MeshDataToObjFile(listMeshDatas, path, exportFileType);
    }

    public static void ExportMeshFilterToObj(List<MeshFilter> listMeshFilter, string path, ExportFileType exportFileType = ExportFileType.SingleFile)
    {
        if (listMeshFilter == null || listMeshFilter.Count <= 0)
        {
            Debug.LogError("listMeshFilter is null");
        }

        Vector3 fristMeshPos = listMeshFilter[0].transform.position;

        List<MeshData> listMeshDatas = new List<MeshData>();

        foreach (var item in listMeshFilter)
        {
            listMeshDatas.Add(new MeshData()
            {
                name = item.name,
                mesh = item.sharedMesh,
                centerOffset = fristMeshPos - item.transform.position,
            });
        }
        MeshDataToObjFile(listMeshDatas, path, exportFileType);
    }

    private static void MeshDataToObjFile(List<MeshData> listMeshDatas, string path, ExportFileType exportFileType)
    {
        int verticesCount = 0;
        string objFileContent = string.Empty;
        foreach (var meshData in listMeshDatas)
        {
            string verticesString = "";
            foreach (Vector3 v in meshData.mesh.vertices)
            {
                verticesString += string.Format("v {0} {1} {2}\n", v.x + meshData.centerOffset.x, v.y + meshData.centerOffset.y, v.z + meshData.centerOffset.z);
            }

            string normalsString = "";
            foreach (Vector3 n in meshData.mesh.normals)
            {
                normalsString += string.Format("vn {0} {1} {2}\n", n.x, n.y, n.z);
            }

            string uvString = "";
            foreach (Vector2 uv in meshData.mesh.uv)
            {
                uvString += string.Format("vt {0} {1}\n", uv.x, uv.y);
            }

            string trianglesString = "";
            for (int i = 0; i < meshData.mesh.triangles.Length; i += 3)
            {
                trianglesString += string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                meshData.mesh.triangles[i] + 1 + verticesCount,
                meshData.mesh.triangles[i + 1] + 1 + verticesCount,
                meshData.mesh.triangles[i + 2] + 1 + verticesCount);
            }
            objFileContent += $"{verticesString}{normalsString}{uvString}g {meshData.mesh.name}\n{trianglesString}";

            if (exportFileType == ExportFileType.SingleFile)
            {
                verticesCount += meshData.mesh.vertices.Length;
            }
            else
            {
                File.WriteAllText(path.Replace(".obj", $"_{meshData.mesh.name}.obj"), objFileContent);
            }
        }
        if (exportFileType == ExportFileType.SingleFile)
        {
            File.WriteAllText(path, objFileContent);
        }
    }
}
