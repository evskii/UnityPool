using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public static class EvMath
{
	public static float Map(float x, float a, float b, float c, float d) {
		return (x - a) / (b - a) * (d - c) + c;
	}

	
	/*
	 *	THIS SHIT DOES NOT WORK LOL
	 *	Looping through all of those pixels is just too much work
	 *	Also the method used just doesnt work
	 */
	// public static Color TextureColor(Texture2D texture) {
	// 	Color finalColor = Color.white;
	// 	List<Color> allColors = new List<Color>();
	//
	// 	for (int x = 0; x < texture.width; x++) {
	// 		for (int y = 0; y < texture.height; y++) {
	// 			allColors.Add(texture.GetPixel(x, y));
	// 			finalColor += texture.GetPixel(x, y);
	// 		}
	// 	}
	//
	// 	finalColor = finalColor / texture.width * texture.height;
	//
	// 	return finalColor;
	// }
	//
	// public static Color TextureColor(Material material) {
	// 	return TextureColor((Texture2D) material.mainTexture);
	// }
}
