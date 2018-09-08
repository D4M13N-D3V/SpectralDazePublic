using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class ImageEffect : MonoBehaviour
{
	public Material EffectMaterial;

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (EffectMaterial == null)
			return;
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
		Graphics.Blit(src, dest, EffectMaterial);
	}
}