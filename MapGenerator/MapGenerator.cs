using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public int mapSize;
	public int chunkSize;
	public int viewDistance;
	public int seed;
	public float scale;
	public Material[] textures;
	public GameObject player;
	World world;

	void Start(){
		world = new World (mapSize, chunkSize, viewDistance, seed, scale, gameObject.transform, textures);
	}

	void Update(){
		world.DrawMap (player);

		if (Input.GetKeyDown (KeyCode.F1)) {
			player.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}
}
