using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	[SerializeField] private GameObject weaponGFX;
	[SerializeField] private Camera cam;
	[SerializeField] private LayerMask mask;

	private WeaponManager weaponManager;
	private PlayerWeapon currentWeapon;

	void Start() {
		if (cam == null) {
			Debug.Log("PlayerShoot: Error no camera referenced.");
			this.enabled = false;
		}

		weaponManager = GetComponent<WeaponManager>();
	}

	void Update() {
		currentWeapon = weaponManager.GetCurrentWeapon();

		if (PauseMenu.isOn) return;

		if (currentWeapon.fireRate <= 0) {
			if (Input.GetButtonDown("Fire1")) {
				Shoot();
			}
		} else {
			if (Input.GetButtonDown("Fire1")) {
				InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
			} else if (Input.GetButtonUp("Fire1")) {
				CancelInvoke("Shoot");
			}
		}
	}

	// Called on the server when a player shoots
	[Command]
	void CmdOnShoot() {
		RpcDoShootEffect();
	}

	// Called on all clients when we need to perform a shoot effect
	[ClientRpc]
	void RpcDoShootEffect() {
		weaponManager.GetCurrentGraphics().muzzleFlash.Play();
	}

	// Called on the server when something is hit
	// Takes in hit point and the normal of the surface
	[Command]
	void CmdOnHit(Vector3 _pos, Vector3 _normal) {
		RpcDoHitEffect(_pos, _normal);
	}

	// Is called on all clients, effects are spawned
	[ClientRpc]
	void RpcDoHitEffect(Vector3 _pos, Vector3 _normal) {
		GameObject _hitEffect = (GameObject) Instantiate (weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
		Destroy(_hitEffect, 1.5f);
	}

	[Client]
	private void Shoot() {
		if (!isLocalPlayer) return;

		// Shooting, call the OnShoot method on the server
		CmdOnShoot();

		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask)) {
			if (_hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot(_hit.collider.name, currentWeapon.damage, transform.name);
			}

			// We hit something, call the OnHit method on the server
			CmdOnHit(_hit.point, _hit.normal);
		}
	}

	[Command]
	private void CmdPlayerShot(string _playerID, int _damage, string _sourceID) {
		Debug.Log(_playerID + " has been shot.");

		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage, _sourceID);
	}

}
