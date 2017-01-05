using System;
using UnityEditor;
using UnityEngine;

public class ResProcessor : AssetPostprocessor
{
	void OnPreprocessTexture () {
		if (assetPath.Contains("Action/")) {
			TextureImporter textureImporter  = (TextureImporter) assetImporter;
			textureImporter.textureType = TextureImporterType.Sprite;
			textureImporter.generateMipsInLinearSpace = false;
			textureImporter.filterMode = UnityEngine.FilterMode.Bilinear;
			textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            textureImporter.spritePixelsPerUnit = 100;
            textureImporter.generateMipsInLinearSpace = false;
		}
	}
}

