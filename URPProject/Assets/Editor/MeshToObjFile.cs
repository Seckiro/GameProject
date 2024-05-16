using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.Rendering;
using Unity.Collections;

public class MeshToObjFile : MonoBehaviour
{
    public GameObject gameobject1;
    public GameObject gameobject2;

    public string path = "Assets/ExportedObjMesh.obj";

    [ContextMenu("GenObjFile")]
    public void GenObjFile()
    {
        //ExportMeshToObjFile(new List<MeshFilter>() {
        //gameobject1.GetComponent<MeshFilter>(),
        //gameobject2.GetComponent<MeshFilter>()}, path);

        ExportMeshToObjPng(new List<MeshRenderer>() {
        gameobject2.GetComponent<MeshRenderer>()});
    }

    public void ExportMeshToObjPng(List<MeshRenderer> meshRenderers)
    {
        foreach (var item in meshRenderers)
        {
            Material[] materials = item.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                Texture texture = materials[i].mainTexture;
                if (texture != null)
                {
                    NativeArray<byte> _buffer = new NativeArray<byte>(texture.width * texture.height * 4, Allocator.Persistent, NativeArrayOptions.UninitializedMemory); ;

                    RenderTexture flip = new RenderTexture(texture.width, texture.height, 0);

                    var (scale, offs) = (new Vector2(1, 1), new Vector2(0, 0));

                    Graphics.Blit(null, flip, materials[i]);
                    //Graphics.Blit(texture, flip, scale, offs);

                    AsyncGPUReadback.RequestIntoNativeArray(ref _buffer, flip, 0, (request) =>
                    {
                        if (request.hasError)
                        {
                            Debug.Log("GPU readback error detected.");
                            return;
                        }
                        using var encoded = ImageConversion.EncodeNativeArrayToPNG(_buffer, flip.graphicsFormat, (uint)flip.width, (uint)flip.height);
                        File.WriteAllBytes(path.Replace(".obj", $"_{item.name}.png"), encoded.ToArray());
                        GameObject.Destroy(flip);
                        _buffer.Dispose();
                        AssetDatabase.Refresh();
                        Debug.Log("Complete!!");
                    });
                }
            }
        }
    }

    public static void ExportMeshToObjFile(List<MeshFilter> listSkineMeshmeshs, string path)
    {
        File.WriteAllText(path, MeshToString(listSkineMeshmeshs));
        AssetDatabase.Refresh();


    }

    struct MeshData
    {
        public Mesh mesh;
        public string name;
        public Vector3 centerOffset;
    }

    public static string MeshToString(List<MeshFilter> listSkineMeshmeshs)
    {
        if (listSkineMeshmeshs == null || listSkineMeshmeshs.Count <= 0)
        {
            Debug.LogError("SkinnedMeshRenderer is null");
            return "";
        }

        Vector3 fristMeshPos = listSkineMeshmeshs[0].transform.position;

        List<MeshData> meshDatas = new List<MeshData>();

        foreach (var item in listSkineMeshmeshs)
        {
            meshDatas.Add(new MeshData() { mesh = item.sharedMesh, name = item.name, centerOffset = fristMeshPos - item.transform.position });
        }

        int verticesCount = 0;
        string objString = "";
        foreach (var meshData in meshDatas)
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
               meshData.mesh.triangles[i] + 1 + verticesCount, meshData.mesh.triangles[i + 1] + 1 + verticesCount, meshData.mesh.triangles[i + 2] + 1 + verticesCount);
            }

            objString += $"{verticesString}{normalsString}{uvString}g {meshData.mesh.name}\n{trianglesString}";
            verticesCount += meshData.mesh.vertices.Length;
        }

        return objString;
    }
}
