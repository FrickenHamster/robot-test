using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoboScript : MonoBehaviour {
	private GameObject model;
	private Animator animator;
	private Vector3 prevPos;
	private Rigidbody2D body;

	private float maxSpeed = 7;
	private Vector3 startPosition;

	private RobotState state;
	private bool onFloor;
	private int floorNum;
	private bool rooted;
	private GameObject win;

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

		win = GameObject.FindGameObjectWithTag("WinTrigger");

		startPosition = transform.position;

		state = RobotState.Idle;
		onFloor = false;
		floorNum = 0;
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

		if ((win.transform.position - transform.position).magnitude < 2) {
			win.GetComponent<Animator>().SetBool("param_idletowinpose", true);
			GameController.Instance.WinScreen.SetActive(true);
		}

		if (transform.position.y < -6)
			GameController.Instance.LoseScreen.SetActive(true);

		prevPos = this.transform.position;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Floor") {
			floorNum++;
			onFloor = true;
			if (state == RobotState.Jump)
				SetState(RobotState.Idle);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Floor") {
			floorNum--;
			if (floorNum == 0)
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
		vv.y = 18;
		body.velocity = vv;
	}

	private IEnumerator AttackCoroutine() {
		rooted = true;
		for (float i = 0; i < 1.1; i += Time.deltaTime)
			yield return null;
		Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + 1.5f, transform.position.y + 0.5f),
			new Vector2(1.5f, 1), 0);
		foreach (var col in colliders) {
			if (col.gameObject.tag == "Enemy")
				Destroy(col.gameObject);
		}
		for (float i = 0; i < .6; i += Time.deltaTime)
			yield return null;
		rooted = false;
		SetState(RobotState.Idle);
	}

}
