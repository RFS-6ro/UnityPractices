using GameCore.SaveSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.Resources
{
	public static class AssetUtilities
	{
		public static Material LoadMaterial(MaterialType type)
		{
			return Resources.Load($"RuntimeMaterials/{type}", typeof(Material)) as Material;
		}

		public static Color ToColor(this MaterialType type)
		{
			uint HexVal = (uint)type;
			byte R = (byte)((HexVal >> 16) & 0xFF);
			byte G = (byte)((HexVal >> 8) & 0xFF);
			byte B = (byte)((HexVal) & 0xFF);
			return new Color(R / 255f, G / 255f, B / 255f, 1.0f);
		}

		public static Page GetNewPageInstance(PageType type)
		{
			return (Resources.Load($"Pages/{type}", typeof(GameObject)) as GameObject)?.GetComponent<Page>();
		}

		public static void SaveTextureToFile(Texture2D texture, string filename)
		{
			string path = Application.persistentDataPath + "/Save/";

			if (System.IO.Directory.Exists(path) == false)
			{
				System.IO.Directory.CreateDirectory(path);
			}
			System.IO.File.WriteAllBytes(path + filename, texture.EncodeToPNG());
		}

		public static Texture2D LoadTextureFromFile(string filename)
		{
			string path = Application.persistentDataPath + "/Save/";

			if (System.IO.Directory.Exists(path) == false)
			{
				return null;
			}
			byte[] bytes = System.IO.File.ReadAllBytes(path + filename);
			Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			texture.LoadImage(bytes);
			return texture;
		}

		public static void UpdateFileName(string oldFileName, string newFileName)
		{
			string path = Application.persistentDataPath + "/Save/";

			if (System.IO.Directory.Exists(path) == false)
			{
				return;
			}
			System.IO.File.Move(path + oldFileName, path + newFileName);
		}

		public static void DeleteFile(string filename)
		{
			string path = Application.persistentDataPath + "/Save/";

			if (System.IO.Directory.Exists(path) == false)
			{
				return;
			}
			System.IO.File.Delete(path + filename);
		}

		public static GameObject LoadPrefab(string prefab)
		{
			return Resources.Load($"Prefabs/{prefab}", typeof(GameObject)) as GameObject;
		}
	}
}
