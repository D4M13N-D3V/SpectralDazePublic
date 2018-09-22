using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class DimensionPeekEffect : MonoBehaviour
{
	public Material EffectMaterial;

	[ShowInInspector]
	public static Matrix4x4 Mtx;
	[ShowInInspector]
	public static Mesh DrawMesh;
	[ShowInInspector]
	public static Material DrawMaterial;

	public static float Intensity = 0f;

	private const string MASK_NAME = "_PeekMask";
	private const string INTENSITY_NAME = "_PeekIntensity";

	private CommandBuffer cmdBuffer;

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (EffectMaterial == null || DrawMesh == null || DrawMaterial == null)
			return;

		RenderTextureDescriptor rtd = new RenderTextureDescriptor(Screen.width, Screen.height);
		int id = Shader.PropertyToID(MASK_NAME);

		cmdBuffer = new CommandBuffer();
		cmdBuffer.GetTemporaryRT(id, rtd, FilterMode.Trilinear);
		cmdBuffer.SetRenderTarget(id);
		cmdBuffer.ClearRenderTarget(true, true, Color.clear);
		cmdBuffer.DrawMesh(DrawMesh, Mtx, DrawMaterial);
		cmdBuffer.SetGlobalTexture(id, id);
		cmdBuffer.SetGlobalFloat(Shader.PropertyToID(INTENSITY_NAME), Intensity);
		cmdBuffer.Blit(src, dest, EffectMaterial);
		cmdBuffer.ReleaseTemporaryRT(id);

		Graphics.ExecuteCommandBuffer(cmdBuffer);
	}
}