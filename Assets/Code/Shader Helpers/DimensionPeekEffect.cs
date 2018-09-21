using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionPeekEffect : MonoBehaviour
{
	[SerializeField]
	private Material peekMaterial;

	[SerializeField] private Camera dimensionBCam, dimensionCutCam;

	private RenderTexture peekVisualRT, peekMaskRT;

	private void Start()
	{
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
		peekVisualRT = new RenderTexture(Screen.width, Screen.height, 0);
		peekMaskRT = new RenderTexture(Screen.width, Screen.height, 1);

		dimensionBCam.targetTexture = peekVisualRT;
		dimensionCutCam.targetTexture = peekMaskRT;

		peekMaterial.SetTexture("_DimensionB", peekVisualRT);
		peekMaterial.SetTexture("_DimensionCut", peekMaskRT);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (peekMaterial == null)
			return;
		Graphics.Blit(src, dest, peekMaterial);
	}
}