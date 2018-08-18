using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using MORPH3D.COSTUMING;
using MORPH3D.FOUNDATIONS;

public class CollectForOutput : MonoBehaviour {

	// Add a menu item named "Do Something" to MyMenu in the menu bar.
	[MenuItem ("MORPH3D/Collect Dependencies")]
	static void CollectDepenciesMenuItem () {
		CollectForOutput.CollectDependencies (Selection.activeObject as GameObject);
	}
	
	[MenuItem("Assets/Collect Dependencies")]
	private static void CollectDependenciesRIghtClick()
	{
		CollectForOutput.CollectDependencies (Selection.activeObject as GameObject);
	}

	[MenuItem ("MORPH3D/Copy Material On Lod0 To Other LODs")]
	static void CopyMaterialMenuItem () {
		CopyMaterialPropertiesOnLodZeroToOtherLODs (Selection.activeObject as GameObject);
	}
	
	[MenuItem("Assets/Copy Material On Lod0 To Other LODs")]
	private static void CopyMaterialRightClick(){
		CopyMaterialPropertiesOnLodZeroToOtherLODs (Selection.activeObject as GameObject);
	}


	
	
	[MenuItem("Assets/Check For FBX")]
	private static void CheckForFBX(){

//		Debug.Log("PREFAB:"+ AssetDatabase.GetAssetPath(Selection.activeObject).Contains(".prefab"));
		string object_path = AssetDatabase.GetAssetPath (Selection.activeObject);
		bool is_prefab = object_path.Contains (".prefab");
		bool is_fbx = object_path.Contains (".fbx");

		if (is_prefab) {

		} else if (is_fbx) {

		}

		Debug.Log (is_prefab + ":" + is_fbx);


//		GameObject my_object = Selection.activeObject as GameObject;
//		Object[] dependecies = EditorUtility.CollectDependencies(new Object[]{Selection.activeObject}); 
//		Debug.Log(dependecies.Length + " ITEMS FOR " + my_object.name);

//		Debug.Log("/////////////////////////////////");
//		string[] assets = AssetDatabase.GetDependencies(new string[] {AssetDatabase.GetAssetPath(Selection.activeObject)});
//		foreach(string path in assets){
//			//move fbx to prop folder
//			if(path.Contains(".fbx")){
////				string root_path = StripFileName(object_path);
//				string file_name = GetFilename(path);
//				AssetDatabase.MoveAsset(path,assetPath +"/" + file_name);
//			}
//		}

	}


	private static void CopyMaterialPropertiesOnLodZeroToOtherLODs(GameObject go){
		CostumeItem[] all_items = go.GetComponentsInChildren<CostumeItem> (true);
		foreach (CostumeItem item in all_items) {

			SkinnedMeshRenderer[] skinned_meshes = null;
			SkinnedMeshRenderer lodZero = null;
			Material[] lod_zero_materials = null;

			if( item.GetType() == typeof(CIbody)){
				Debug.Log("BODY");
				CIbody body = item as CIbody;
				skinned_meshes = new SkinnedMeshRenderer[body.LODlist.Count];
				int i = 0;
				foreach(CoreMesh mesh in body.LODlist){
					skinned_meshes[i] = mesh.gameObject.GetComponent<SkinnedMeshRenderer>();
					i++;
				}

			}else{
				skinned_meshes = item.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			}

			foreach(SkinnedMeshRenderer renderer in skinned_meshes){
				if(renderer.name.EndsWith("0")){
					lodZero = renderer;
					lod_zero_materials = lodZero.sharedMaterials;
				}
			}
			
			foreach(SkinnedMeshRenderer renderer in skinned_meshes){
				if(renderer != lodZero){
					Material[] sub_materials = renderer.sharedMaterials;
					int i = 0;
					foreach(Material mat in sub_materials){
						mat.CopyPropertiesFromMaterial(lod_zero_materials[i]);
						i++;
					}
					renderer.sharedMaterials = sub_materials;
				}
			}

		}
	}

	private static void CollectDependencies(GameObject go){
		string assetPath = AssetDatabase.GetAssetPath (go);//current folder location
		assetPath = PutGameObjectInFolder (go, assetPath);//destination folder location
		Debug.Log (go.name + ":" + assetPath);
		
		string file_name = GetFilename (assetPath);
		if (file_name.Contains (".fbx") == false) {
			MoveFbxToCorrectLocation (go, assetPath);
		}

		MoveMaterialsToCorrectLocation (go, assetPath); 

	}

	private static void MoveFbxToCorrectLocation(GameObject go, string assetPath){
		string[] assets = AssetDatabase.GetDependencies(new string[] {AssetDatabase.GetAssetPath(go)});
		foreach(string path in assets){
			//move fbx to prop folder
			if(path.Contains(".fbx")){

				string file_name = GetFilename(path);
				
				Debug.Log("FOUND FBX AT:"+file_name);
				Debug.Log("FOUND FBX:"+assetPath);
				AssetDatabase.MoveAsset(path,StripFileName(assetPath) +"/" + file_name);
			}
		}
	}

	private static string PutGameObjectInFolder(GameObject go, string assetPath){
		string asset_directory = StripFileName (assetPath);

		if (asset_directory.Contains (go.name) == false) {

			if (AssetDatabase.IsValidFolder (asset_directory + "/" + go.name) == false) {
				AssetDatabase.CreateFolder (asset_directory, go.name);
				AssetDatabase.Refresh ();
			}

			AssetDatabase.MoveAsset (assetPath, asset_directory + "/" + go.name + "/" + GetFilename (assetPath));
			AssetDatabase.Refresh ();

		}

		return AssetDatabase.GetAssetPath (go);

	}
	
	private static void MoveMaterialsToCorrectLocation(GameObject go, string assetPath){
		
		string material_destination = StripFileName(assetPath) + "/Materials";
		Debug.Log (material_destination);
		
		SkinnedMeshRenderer[] skinned_meshes = go.GetComponentsInChildren<SkinnedMeshRenderer> (true);
		foreach (SkinnedMeshRenderer renderer in skinned_meshes) {
			
			Material[] materials = renderer.sharedMaterials;
			int i = 0;
			foreach(Material mat in materials){
				
				string material_location = StripFileName( AssetDatabase.GetAssetPath(mat));
				if(material_destination != material_location){
					Debug.Log("not in proper palce:" + mat.name);
					if(AssetDatabase.IsValidFolder(material_destination) == false){
						AssetDatabase.CreateFolder(StripFileName(assetPath), "Materials");
						AssetDatabase.Refresh();
					}

					AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(mat), material_destination + "/" + GetFilename(AssetDatabase.GetAssetPath(mat)));
				}

				MoveTexturesToCorrectLocation(mat, assetPath);
				i++;
			}
			
			renderer.sharedMaterials = materials;
		}
		
	}
	
	
	private static void MoveTexturesToCorrectLocation(Material mat, string assetPath){
		string root_folder = StripFileName (assetPath);
		string texture_folder_name = "Textures";
		string texture_destination = root_folder + "/" + texture_folder_name;
		List<Texture> textures = getTextures (mat);
		if (textures.Count > 0) {
			Debug.Log(mat.name+" has text:"+ textures.Count);
			foreach(Texture txtr in textures){
				string file_path = AssetDatabase.GetAssetPath(txtr);
				string texture_path = StripFileName(AssetDatabase.GetAssetPath(txtr));
				string texture_name = file_path.Substring(file_path.LastIndexOf("/")+1);
				Debug.Log(texture_destination + "/" + texture_name);
				
				if(texture_destination != texture_path){
					
					if(AssetDatabase.IsValidFolder(texture_destination) == false){
						AssetDatabase.CreateFolder(root_folder, texture_folder_name);
						AssetDatabase.Refresh();
					}
					
					string error = AssetDatabase.ValidateMoveAsset(file_path, texture_destination + "/" + texture_name);
					if(string.IsNullOrEmpty(error) == true){
						Debug.Log("CAN MOVE TEXTURE");
						AssetDatabase.MoveAsset(file_path,  texture_destination + "/" + texture_name);
					}else{
						Debug.Log("NO MOVE:"+error);
					}
					
				}
			}
		}
		
	}

	

	private static List<Texture> getTextures(Material material){
		string assetPath=AssetDatabase.GetAssetPath(material);        
		if (string.IsNullOrEmpty(assetPath))
			return null;
		Shader shader=material.shader;            
		List<Texture> materials=new List<Texture>();
		for (int i = 0; i < ShaderUtil.GetPropertyCount(shader) ; i++) {                
			if (ShaderUtil.GetPropertyType(shader,i) == ShaderUtil.ShaderPropertyType.TexEnv){
				Texture texture = material.GetTexture(ShaderUtil.GetPropertyName(shader,i));
				if(texture != null)
					materials.Add(texture);
			}            
		}
		return materials;
	}   
	
	private static string StripFileName(string path){
		return path.Substring (0, path.LastIndexOf ("/"));
	}
	
	private static string GetFilename(string path){
			return path.Substring (path.LastIndexOf ("/") + 1);
	}

}
