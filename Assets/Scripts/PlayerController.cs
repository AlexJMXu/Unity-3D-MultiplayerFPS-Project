using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField] private float speed = 5f;
	[SerializeField] private float lookSensitivity = 3f;
	[SerializeField] private float thrusterForce = 1000f;

	[SerializeField] private float thrusterFuelBurnSpeed = 1f;
	[SerializeField] private float thrusterFuelRegenSpeed = 0.3f;
	private float thrusterFuelAmount = 2f;

	public float GetThrusterFuelAmount() {
		return thrusterFuelAmount/2;
	}

	[SerializeField] private LayerMask environmentMask;

	[Header("Spring settings:")]
	[SerializeField] private float jointSpring = 20f;
	[SerializeField] private float jointMaxForce = 40f;

	// Component Cache
	private PlayerMotor motor;
	private ConfigurableJoint joint;
	private Animator animator;

	void Start() {
		motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint>();
		animator = GetComponent<Animator>();

		SetJointSettings(jointSpring);
	}

	void Update() {
		RaycastHit _hit;
		if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, environmentMask)) {
			joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
		} else {
			joint.targetPosition = new Vector3(0f,0f,0f);
		}

		// Calculate movement velocity as a 3D Vector
		float _xMove = Input.GetAxis("Horizontal");
		float _zMove = Input.GetAxis("Vertical");
		Vector3 _movHorizontal = transform.right * _xMove; // (1, 0, 0) Vector
		Vector3 _movVertical = transform.forward * _zMove; // (0, 0, 1) Vector
		// Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;
		// Animate movement
		animator.SetFloat("ForwardVelocity", _zMove);
		// Apply movement
		motor.Move(_velocity);

		// Calculate rotation as a 3D vector (turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");
		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
		// Apply rotation
		motor.Rotate(_rotation);

		// Calculate rotation as a 3D vector (looking up and down)
		float _xRot = Input.GetAxisRaw("Mouse Y");
		float _cameraRotationX = _xRot * lookSensitivity;
		// Apply rotation
		motor.RotateCamera(_cameraRotationX);

		Vector3 _thrusterForce = Vector3.zero;
		// Apply thruster force
		if (Input.GetButton("Jump") && thrusterFuelAmount > 0) {
			thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

			if (thrusterFuelAmount >= 0.01) {
				_thrusterForce = Vector3.up * thrusterForce;
				SetJointSettings(0f);
			}
		} else {
			if (transform.position.y <= 2-joint.targetPosition.y) {
				thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
			}
			SetJointSettings(jointSpring);
		}

		thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 2f);
		motor.ApplyThruster(_thrusterForce);
	}

	private void SetJointSettings(float _jointSpring) {
		joint.yDrive = new JointDrive { 
			positionSpring = _jointSpring, 
			maximumForce = jointMaxForce 
		};
	}

}
