using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboScript : MonoBehaviour {
	private GameObject model;
	private Animator animator;
	private Vector3 prevPos;
	private Rigidbody2D body;

	private float maxSpeed = 7;

	private RobotState state;
	private bool onFloor;
	private bool rooted;

	private enum RobotState {
		Idle = 0,
		Walk = 1,
		Jump = 2,
		Attack = 3,
	}


	void Start() {
		model = transform.GetChild(0).gameObject;
		animator = model.GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();

		state = RobotState.Idle;
		onFloor = false;
	}

	void Update() {
		float hAxis = Input.GetAxis("Horizontal");
		if (!rooted)
			transform.Translate(hAxis * maxSpeed * Time.deltaTime, 0, 0, Space.World);
		if (Mathf.Abs(hAxis) < 0.2)
			hAxis = 0;

		if (Input.GetKeyDown(KeyCode.Space) && this.onFloor) {
			SetState(RobotState.Jump);
			animator.speed = 1;
			StartCoroutine(JumpCoroutine());
		}

		if (Input.GetKeyDown(KeyCode.J) && this.onFloor) {
			SetState(RobotState.Attack);
			animator.speed = 1;
			StartCoroutine(AttackCoroutine());
		}

		if (state != RobotState.Attack) {
			if (hAxis > 0) {
				model.transform.rotation = Quaternion.Euler(0, 90, 0);
			} else if (hAxis < 0) {
				model.transform.rotation = Quaternion.Euler(0, -90, 0);
			}
		}

		float speed = Mathf.Abs(transform.position.x - prevPos.x) / Time.deltaTime;

		if (state == RobotState.Walk || state == RobotState.Idle) {
			if (hAxis != 0) {
				SetState(RobotState.Walk);
				animator.speed = speed * 0.3f;
			} else {
				SetState(RobotState.Idle);
				animator.speed = 1;
			}
		}

		Camera.main.transform.position = new Vector3(this.transform.position.x, transform.transform.position.y + 2.5f, -10);

		prevPos = this.transform.position;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Floor") {
			onFloor = true;
			if (state == RobotState.Jump)
				SetState(RobotState.Idle);
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Floor") {
			onFloor = false;
		}
	}

	private void SetState(RobotState state) {
		if (this.state == state)
			return;
		this.state = state;
		animator.SetInteger("Animation", (int)state);
	}

	private IEnumerator JumpCoroutine() {
		rooted = true;
		for (float i = 0; i < .5; i += Time.deltaTime)
			yield return null;
		rooted = false;
		Vector3 vv = body.velocity;
		vv.y = 10;
		body.velocity = vv;
	}

	private IEnumerator AttackCoroutine() {
		rooted = true;
		for (float i = 0; i < 1.4; i += Time.deltaTime)
			yield return null;
		rooted = false;
		SetState(RobotState.Idle);
	}

}
