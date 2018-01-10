using UnityEngine;
using System.Collections;

public class Chunk {

	private int chunkSize;
	private int seed;
	private int levelOfDetail;
	private float scale;
	private bool collider = false;
	private Vector2 offset;
	private GameObject[] chunk = new GameObject[7];
	private Transform mapGenerator;
	private Material[] textures;

	public Chunk(int chunkSize, int seed, float scale, Vector2 offset, bool collider, int levelOfDetail, Transform mapGenerator, Material[] textures){
		this.chunkSize = (chunkSize / levelOfDetail) + 1;
		this.seed = seed;
		this.scale = scale;
		this.offset = offset;
		this.collider = collider;
		this.levelOfDetail = levelOfDetail;
		this.mapGenerator = mapGenerator;
		this.textures = textures;
	}

	public void DrawChunk(){
		
		int[][] triangles = new int[7][];
		int objNumber = -1;
		int triIndex = 0;
		float[] mapChecks = { 0.3f, 0.35f, 0.42f, 0.5f, 0.6f, 0.7f };
		float[,] noiseMap = Noise.GenerateNoiseMap (chunkSize, levelOfDetail, seed, scale, offset * ((chunkSize - 1) * levelOfDetail), 5, 2, 0.5f);
		Vector3[][] vertices = new Vector3[7][];
		Vector2[][] uvs = new Vector2[7][];

		for (int i = 0; i < 7; i++) {
			triangles [i] = new int[(chunkSize - 1) * (chunkSize - 1) * 6];
			vertices [i] = new Vector3[chunkSize * chunkSize];
			Debug.Log (chunkSize * chunkSize);
			uvs [i] = new Vector2[chunkSize * chunkSize];
			chunk [i] = new GameObject ();
		}
			
		for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {

				if (noiseMap [x, y] < mapChecks[0]) {
					objNumber = 0;
				} else if (noiseMap [x, y] < mapChecks[1]) {
					objNumber = 1;
				} else if (noiseMap [x, y] < mapChecks[2]) {
					objNumber = 2;
				} else if (noiseMap [x, y] < mapChecks[3]) {
					objNumber = 3;
				} else if (noiseMap [x, y] < mapChecks[4]) {
					objNumber = 4;
				} else if (noiseMap [x, y] < mapChecks[5]) {
					objNumber = 5;
				} else {
					objNumber = 6;
				}

				float[] noiseVal = new float[4];

				if (noiseMap [x, y] < mapChecks[0]) {
					noiseVal [0] = Mathf.Lerp (-5, -1, Mathf.InverseLerp (0, mapChecks[0], noiseMap [x, y]));
				} else if (noiseMap [x, y] < mapChecks[1]) {
					noiseVal [0] = Mathf.Lerp (-1, 0, Mathf.InverseLerp (mapChecks[0], mapChecks[1], noiseMap [x, y]));
				} else if (noiseMap [x, y] < mapChecks[2]) {
					noiseVal [0] = Mathf.Lerp (0, 1, Mathf.InverseLerp (mapChecks[1], mapChecks[2], noiseMap [x, y]));
				} else if (noiseMap [x, y] < mapChecks[3]) {
					noiseVal [0] = Mathf.Lerp (1, 5, Mathf.InverseLerp (mapChecks[2], mapChecks[3], noiseMap [x, y]));
				} else if (noiseMap [x, y] < mapChecks[4]) {
					noiseVal [0] = Mathf.Lerp (5, 10, Mathf.InverseLerp (mapChecks[3], mapChecks[4], noiseMap [x, y]));
				} else if (noiseMap [x, y] < mapChecks[5]) {
					noiseVal [0] = Mathf.Lerp (10, 15, Mathf.InverseLerp (mapChecks[4], mapChecks[5], noiseMap [x, y]));
				} else {
					noiseVal [0] = Mathf.Lerp (15, 18, Mathf.InverseLerp (mapChecks[5], 1f, noiseMap [x, y]));
				}

				vertices [objNumber] [x * chunkSize + y] = new Vector3 (x * levelOfDetail, noiseVal[0], y * levelOfDetail);
				uvs [objNumber] [x * chunkSize + y] = new Vector2 (x * levelOfDetail, y * levelOfDetail);



				if (x < chunkSize - 1 && y < chunkSize - 1) {

					if (noiseMap [x + 1, y + 0] < mapChecks[0]) {
						noiseVal [1] = Mathf.Lerp (-5, -1, Mathf.InverseLerp (0, mapChecks[0], noiseMap [x + 1, y + 0]));
					} else if (noiseMap [x + 1, y + 0] < mapChecks[1]) {
						noiseVal [1] = Mathf.Lerp (-1, 0, Mathf.InverseLerp (mapChecks[0], mapChecks[1], noiseMap [x + 1, y + 0]));
					} else if (noiseMap [x + 1, y + 0] < mapChecks[2]) {
						noiseVal [1] = Mathf.Lerp (0, 1, Mathf.InverseLerp (mapChecks[1], mapChecks[2], noiseMap [x + 1, y + 0]));
					} else if (noiseMap [x + 1, y + 0] < mapChecks[3]) {
						noiseVal [1] = Mathf.Lerp (1, 5, Mathf.InverseLerp (mapChecks[2], mapChecks[3], noiseMap [x + 1, y + 0]));
					} else if (noiseMap [x + 1, y + 0] < mapChecks[4]) {
						noiseVal [1] = Mathf.Lerp (5, 10, Mathf.InverseLerp (mapChecks[3], mapChecks[4], noiseMap [x + 1, y + 0]));
					} else if (noiseMap [x + 1, y + 0] < mapChecks[5]) {
						noiseVal [1] = Mathf.Lerp (10, 15, Mathf.InverseLerp (mapChecks[4], mapChecks[5], noiseMap [x + 1, y + 0]));
					} else {
						noiseVal [1] = Mathf.Lerp (15, 18, Mathf.InverseLerp (mapChecks[5], 1f, noiseMap [x + 1, y + 0]));
					}

					if (noiseMap [x + 0, y + 1] < mapChecks[0]) {
						noiseVal [2] = Mathf.Lerp (-5, -1, Mathf.InverseLerp (0, mapChecks[0], noiseMap [x + 0, y + 1]));
					} else if (noiseMap [x + 0, y + 1] < mapChecks[1]) {
						noiseVal [2] = Mathf.Lerp (-1, 0, Mathf.InverseLerp (mapChecks[0], mapChecks[1], noiseMap [x + 0, y + 1]));
					} else if (noiseMap [x + 0, y + 1] < mapChecks[2]) {
						noiseVal [2] = Mathf.Lerp (0, 1, Mathf.InverseLerp (mapChecks[1], mapChecks[2], noiseMap [x + 0, y + 1]));
					} else if (noiseMap [x + 0, y + 1] < mapChecks[3]) {
						noiseVal [2] = Mathf.Lerp (1, 5, Mathf.InverseLerp (mapChecks[2], mapChecks[3], noiseMap [x + 0, y + 1]));
					} else if (noiseMap [x + 0, y + 1] < mapChecks[4]) {
						noiseVal [2] = Mathf.Lerp (5, 10, Mathf.InverseLerp (mapChecks[3], mapChecks[4], noiseMap [x + 0, y + 1]));
					} else if (noiseMap [x + 0, y + 1] < mapChecks[5]) {
						noiseVal [2] = Mathf.Lerp (10, 15, Mathf.InverseLerp (mapChecks[4], mapChecks[5], noiseMap [x + 0, y + 1]));
					} else {
						noiseVal [2] = Mathf.Lerp (15, 18, Mathf.InverseLerp (mapChecks[5], 1f, noiseMap [x + 0, y + 1]));
					}

					if (noiseMap [x + 1, y + 1] < mapChecks[0]) {
						noiseVal [3] = Mathf.Lerp (-5, -1, Mathf.InverseLerp (0, mapChecks[0], noiseMap [x + 1, y + 1]));
					} else if (noiseMap [x + 1, y + 1] < mapChecks[1]) {
						noiseVal [3] = Mathf.Lerp (-1, 0, Mathf.InverseLerp (mapChecks[0], mapChecks[1], noiseMap [x + 1, y + 1]));
					} else if (noiseMap [x + 1, y + 1] < mapChecks[2]) {
						noiseVal [3] = Mathf.Lerp (0, 1, Mathf.InverseLerp (mapChecks[1], mapChecks[2], noiseMap [x + 1, y + 1]));
					} else if (noiseMap [x + 1, y + 1] < mapChecks[3]) {
						noiseVal [3] = Mathf.Lerp (1, 5, Mathf.InverseLerp (mapChecks[2], mapChecks[3], noiseMap [x + 1, y + 1]));
					} else if (noiseMap [x + 1, y + 1] < mapChecks[4]) {
						noiseVal [3] = Mathf.Lerp (5, 10, Mathf.InverseLerp (mapChecks[3], mapChecks[4], noiseMap [x + 1, y + 1]));
					} else if (noiseMap [x + 1, y + 1] < mapChecks[5]) {
						noiseVal [3] = Mathf.Lerp (10, 15, Mathf.InverseLerp (mapChecks[4], mapChecks[5], noiseMap [x + 1, y + 1]));
					} else {
						noiseVal [3] = Mathf.Lerp (15, 18, Mathf.InverseLerp (mapChecks[5], 1f, noiseMap [x + 1, y + 1]));
					}

					vertices [objNumber] [(x + 1) * chunkSize + y] = new Vector3 ((x + 1) * levelOfDetail, noiseVal[1], y * levelOfDetail);
					uvs [objNumber] [(x + 1) * chunkSize + y] = new Vector2 ((x + 1) * levelOfDetail, y * levelOfDetail);
					vertices [objNumber] [x * chunkSize + (y + 1)] = new Vector3 (x * levelOfDetail, noiseVal[2], (y + 1) * levelOfDetail);
					uvs [objNumber] [x * chunkSize + (y + 1)] = new Vector2 (x * levelOfDetail, (y + 1) * levelOfDetail);
					vertices [objNumber] [(x + 1) * chunkSize + (y + 1)] = new Vector3 ((x + 1) * levelOfDetail, noiseVal[3], (y + 1) * levelOfDetail);
					uvs [objNumber] [(x + 1) * chunkSize + (y + 1)] = new Vector2 ((x + 1) * levelOfDetail, (y + 1) * levelOfDetail);

					triangles [objNumber] [triIndex] = x * chunkSize + y;
					triangles [objNumber] [triIndex + 1] = x * chunkSize + y + chunkSize + 1;
					triangles [objNumber] [triIndex + 2] = x * chunkSize + y + chunkSize;

					triangles [objNumber] [triIndex + 3] = x * chunkSize + y + chunkSize + 1;
					triangles [objNumber] [triIndex + 4] = x * chunkSize + y;
					triangles [objNumber] [triIndex + 5] = x * chunkSize + y + 1;
					triIndex += 6;
				}
					
			}
		}


		for (int i = 0; i < 7; i++) {
			
		

			Mesh mesh = new Mesh ();
			mesh.vertices = vertices[i];
			mesh.uv = uvs[i];
			mesh.triangles = triangles[i];
			mesh.RecalculateBounds ();
			mesh.RecalculateNormals ();

			chunk[i].AddComponent<MeshRenderer> ();
			chunk[i].AddComponent<MeshFilter> ();
			if (collider)
				chunk[i].AddComponent<MeshCollider> ();
			chunk[i].AddComponent<DestroyChunk> ();

			chunk [i].GetComponent<MeshRenderer> ().material = textures [i];
			chunk [i].GetComponent<MeshRenderer> ().material.SetFloat ("_Glossiness", 0f);

			chunk[i].GetComponent<MeshFilter> ().mesh = mesh;
			if (collider)
				chunk[i].GetComponent<MeshCollider> ().sharedMesh = mesh;
				
			chunk[i].transform.position = new Vector3 (offset.x * (chunkSize - 1) * levelOfDetail, 0, offset.y * (chunkSize - 1) * levelOfDetail);
			chunk[i].transform.parent = mapGenerator;


		}
	}

	public void DestroyChunk (){
		for (int i = 0; i < 7; i++) {
			chunk[i].GetComponent<DestroyChunk> ().DestroyObj ();
		}
	}

	public bool Correct(int chunkSize, float scale, bool collider, int levelOfDetail){

		if (this.chunkSize == (chunkSize / levelOfDetail) + 1 && this.scale == scale && this.collider == collider && this.levelOfDetail == levelOfDetail)
			return true;
		else
			return false;

	}
		
	public Vector2 GetPosition(){
		return offset;
	}

}
