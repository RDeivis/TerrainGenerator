using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World {

	private int mapSize;
	private int chunkSize;
	private int viewDistance;
	private int seed;
	private float scale;
	private Transform mapGenerator;
	private Material[] textures;
	private List<Chunk> chunks = new List<Chunk> ();
	private List<Chunk> newChunks = new List<Chunk> ();
	private List<Chunk> chunksToDestroy = new List<Chunk> ();
	private List<Chunk> chunksToDraw = new List<Chunk> ();

	public World(int mapSize, int chunkSize, int viewDistance, int seed, float scale, Transform mapGenerator, Material[] textures){
		this.mapSize = mapSize;
		this.chunkSize = chunkSize;
		this.viewDistance = viewDistance;
		this.seed = seed;
		this.scale = scale;
		this.mapGenerator = mapGenerator;
		this.textures = textures;
	}

	public void DrawMap(GameObject player){

		if (chunks.Count == 0 && chunksToDraw.Count == 0)
			ChunksToDrawList (player);

		if (chunks.Count > 0)
			ChunksToDestroyOrKeep (player);

		if (newChunks.Count > 0)
			ChunksToRedrawOrKeep (player);

		if (chunksToDestroy.Count > 0)
			ChunksToDrawRemaining (player);

		if (chunksToDraw.Count > 0) {
			chunksToDraw [0].DrawChunk ();
			newChunks.Add (chunksToDraw [0]);
			chunksToDraw.RemoveAt (0);
		}

		if (chunksToDraw.Count == 0 && chunksToDestroy.Count > 0) {
			foreach (var chunkE in chunksToDestroy) {
				chunkE.DestroyChunk ();
			}
			chunksToDestroy.Clear ();
		}

		chunks.Clear ();
		if (newChunks.Count > 0) {
			foreach (var chunkE in newChunks) {
				chunks.Add (chunkE);
			}
			newChunks.Clear ();
		}

	}


	private void ChunksToDrawRemaining(GameObject player){
		
		for (int x = -viewDistance/2; x < viewDistance/2; x++) {
			for (int y = -viewDistance/2; y < viewDistance/2; y++) {

				Vector2 offset = new Vector2 (Mathf.Floor (player.transform.position.x / chunkSize) + x, Mathf.Floor (player.transform.position.z / chunkSize) + y);
				bool found = false;

				foreach (var chunkE in newChunks) {
					if (chunkE.GetPosition ().x == offset.x && chunkE.GetPosition ().y == offset.y) {
						found = true;
					}
				}

				if (!found) {
					foreach (var chunkE in chunksToDraw) {
						if (chunkE.GetPosition ().x == offset.x && chunkE.GetPosition ().y == offset.y) {
							found = true;
						}
					}

					if (!found) {
						if (x > -2 && x < 2 && y > -2 && y < 2)
							chunksToDraw.Add (new Chunk (chunkSize, seed, scale, offset, true, 2, mapGenerator, textures));
						else if (x > -4 && x < 4 && y > -4 && y < 4)
							chunksToDraw.Add (new Chunk (chunkSize, seed, scale, offset, false, 5, mapGenerator, textures));
						else
							chunksToDraw.Add (new Chunk (chunkSize, seed, scale, offset, false, 25, mapGenerator, textures));
					}

				}

			}
		}

	}

	private void ChunksToRedrawOrKeep(GameObject player){

		List<Chunk> tmpChunks = new List<Chunk> ();

		foreach (var chunkE in newChunks) {

			float x = chunkE.GetPosition ().x - Mathf.Floor (player.transform.position.x / chunkSize);
			float y = chunkE.GetPosition ().y - Mathf.Floor (player.transform.position.z / chunkSize);
			bool collider = false;
			int levelOfDetail = 25;

			if (x > -2 && x < 2 && y > -2 && y < 2) {
				collider = true;
				levelOfDetail = 2;
			} else if (x > -4 && x < 4 && y > -4 && y < 4) {
				levelOfDetail = 5;
			}
			
			if (!chunkE.Correct (chunkSize, scale, collider, levelOfDetail)) {
				chunksToDestroy.Add (chunkE);
				chunksToDraw.Add (new Chunk (chunkSize, seed, scale, chunkE.GetPosition (), collider, levelOfDetail, mapGenerator, textures));
			} else {
				tmpChunks.Add (chunkE);
			}

		}

		newChunks.Clear ();
		foreach (var chunkE in tmpChunks) {
			newChunks.Add (chunkE);
		}
		tmpChunks.Clear ();

	}

	private void ChunksToDestroyOrKeep(GameObject player){

		foreach (var chunkE in chunks) {	
			float xPos = chunkE.GetPosition ().x - Mathf.Floor (player.transform.position.x / chunkSize);
			float yPos = chunkE.GetPosition ().y - Mathf.Floor (player.transform.position.z / chunkSize);

			if (xPos < 0)
				xPos *= -1;
			if (yPos < 0)
				yPos *= -1;

			if (xPos > viewDistance / 2 || yPos > viewDistance / 2) {
				chunksToDestroy.Add (chunkE);
			} else {
				newChunks.Add (chunkE);
			}

		}

	}

	private void ChunksToDrawList(GameObject player){

		for (int x = -viewDistance/2; x < viewDistance/2; x++) {
			for (int y = -viewDistance/2; y < viewDistance/2; y++) {

				Vector2 offset = new Vector2 (Mathf.Floor (player.transform.position.x / chunkSize) + x, Mathf.Floor (player.transform.position.z / chunkSize) + y);

				if (x > -2 && x < 2 && y > -2 && y < 2)
					chunksToDraw.Add (new Chunk (chunkSize, seed, scale, offset, true, 2, mapGenerator, textures));
				else if (x > -4 && x < 4 && y > -4 && y < 4)
					chunksToDraw.Add (new Chunk (chunkSize, seed, scale, offset, false, 5, mapGenerator, textures));
				else
					chunksToDraw.Add (new Chunk (chunkSize, seed, scale, offset, false, 25, mapGenerator, textures));
			}
		}

	}


}
