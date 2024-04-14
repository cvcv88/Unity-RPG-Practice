using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	[SerializeField] CharacterController controller;
	[SerializeField] Animator animator;
	[SerializeField] float moveSpeed;

	Vector3 moveDir;

	private void Update()
	{
		Move();
	}
	private void Move()
	{
		Vector3 forwardDir = Camera.main.transform.forward;
		Vector3 rightDir = Camera.main.transform.right;

		controller.Move(forwardDir * moveDir.z * moveSpeed * Time.deltaTime);
		controller.Move(rightDir * moveDir.x * moveSpeed * Time.deltaTime);
	}
	private void OnMove(InputValue value)
	{
		Vector3 input = value.Get<Vector2>();
		moveDir.x = input.x;
		moveDir.z = input.y;

		animator.SetFloat("MoveSpeed",moveDir.magnitude);
	}
}