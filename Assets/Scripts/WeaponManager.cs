using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour {
	[SerializeField] private string weaponLayerName = "Weapon";

	[SerializeField] private Transform weaponHolder;

	[SerializeField] private PlayerWeapon primaryWeapon;
	private PlayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;

	public bool isReloading = false;

	// Use this for initialization
	void Start () {
		EquipWeapon(primaryWeapon);
	}

	public PlayerWeapon GetCurrentWeapon() {
		return currentWeapon;
	}

	public WeaponGraphics GetCurrentGraphics() {
		return currentGraphics;
	}

	public void EquipWeapon(PlayerWeapon _weapon) {
		currentWeapon = _weapon;

		GameObject _weaponIns = (GameObject) Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
		_weaponIns.transform.SetParent(weaponHolder);

		currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
		if (currentGraphics == null) {
			Debug.LogError("No weapon graphics found on " + _weaponIns.name);
		}

		if (isLocalPlayer) {
			Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
		}
	}

	public void Reload() {
		if (isReloading) return;

		isReloading = true;

		StartCoroutine(Reload_Coroutine());
	}

	private IEnumerator Reload_Coroutine() {
		Debug.Log("Reloading...");

		CmdOnReload();

		yield return new WaitForSeconds(currentWeapon.reloadTime);

		currentWeapon.bullets = currentWeapon.maxBullets;

		isReloading = false;
	}

	[Command]
	private void CmdOnReload() {
		RpcOnReload();
	}

	[ClientRpc]
	void RpcOnReload() {
		Animator anim = currentGraphics.GetComponent<Animator>();
		if (anim != null) {
			anim.SetTrigger("Reload");
		}
	}
}
