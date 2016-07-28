using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField] private Behaviour[] componentsToDisable;

	[SerializeField] private string remoteLayerName = "RemotePlayer";

	[SerializeField] private string playerPrefix = "Player ";

	public Camera sceneCamera;

	void Start() {
		if (!isLocalPlayer) {
			DisableComponents();
			AssignRemoteLayer();
		} else {
			sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive(false);
			}
		}

		RegisterPlayer();
	}

	void RegisterPlayer() {
		string _ID = playerPrefix + GetComponent<NetworkIdentity>().netId;
		transform.name = _ID;
	}

	void AssignRemoteLayer() {
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	void DisableComponents() {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable[i].enabled = false;
		}
	}

	void OnDisable() {
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(true);
		}
	}

}
