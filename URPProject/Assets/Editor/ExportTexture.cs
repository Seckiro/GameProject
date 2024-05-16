using System.IO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public static class ExportTexture
{
    public static void ExportTextureToFile(Texture texture, string path)
    {
        if (texture == null)
        {
            Debug.LogError("texture is null");
            return;
        }

        NativeArray<byte> textureBuffer = new NativeArray<byte>(texture.width * texture.height * 4, Allocator.Persistent, NativeArrayOptions.UninitializedMemory); ;

        RenderTexture flip = new RenderTexture(texture.width, texture.height, 0);

        Vector2 scale = new Vector2(1, 1);

        Vector2 offs = new Vector2(0, 0);

        Graphics.Blit(texture, flip, scale, offs);

        AsyncGPUReadback.RequestIntoNativeArray(ref textureBuffer, flip, 0, (request) =>
        {
            if (request.hasError)
            {
                Debug.Log("GPU readback error detected.");
                return;
            }
            NativeArray<byte> encodBuffer = ImageConversion.EncodeNativeArrayToPNG(textureBuffer, flip.graphicsFormat, (uint)flip.width, (uint)flip.height);
            File.WriteAllBytes(path, encodBuffer.ToArray());
            GameObject.DestroyImmediate(flip);
            textureBuffer.Dispose();
            encodBuffer.Dispose();
        });

        AsyncGPUReadback.WaitAllRequests();
    }
}
