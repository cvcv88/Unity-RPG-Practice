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
		// 1. ���߿� �ߴ� ���� �ڵ�
		// �÷��̾ �ٶ󺸰� �ִ� ���� ���ϱ� -> ���� ī�޶� ���� �ִ� �� ����
		Vector3 forwardDir = Camera.main.transform.forward; // z��
		Vector3 rightDir = Camera.main.transform.right; // x��

		// 2. y=0 ������ ���� �ڵ�
		// ���� �����ϰ� y��ǥ�� 0���� �����ϱ�
		forwardDir = new Vector3(forwardDir.x, 0, forwardDir.z);
		rightDir = new Vector3(rightDir.x, 0, rightDir.z);

		// 3. ũ�⸦ 1�� ����� �ӵ� �����ϰ� �����ϵ��� �ϴ� ���� �ڵ�
		// �ش� ���⿡���� ũ�Ⱑ 1�� ����(normalized)�� �ʿ�(���⸸ �ʿ��ϱ� ����)
		forwardDir = new Vector3(forwardDir.x, 0, forwardDir.z).normalized;
		rightDir = new Vector3(rightDir.x, 0, rightDir.z).normalized;

		controller.Move(forwardDir * moveDir.z * moveSpeed * Time.deltaTime); // �� ����
		controller.Move(rightDir * moveDir.x * moveSpeed * Time.deltaTime); // �� ����

		// 4. �Է��ϴ� ������ ������ �� �ְ� ȸ��
		// Quaternion.LookRotation() : �ش� ���͸� �ٶ󺸴� ȸ�� ���¸� ��ȯ
		// �޼��� �ȿ��� ��� ��ǥ�� �־���� �Ѵ�
		// Vector3 lookDir = forwardDir * moveDir.z + rightDir * moveDir.x;
		//Quaternion lookRotation = Quaternion.LookRotation(lookDir);
		// transform.rotation = lookRotation;

		// 5. Ű�� �����ٰ� ���� �� ��� ���⿡���� �� ������ �ٶ󺸰� �Ǵ� ������ �ִ�.
		// LookRotation�� zero Vector�� ���� �� ����. �Է��� ���� ���� ��� �ٶ���� ����? z���� �ٶ󺸰� �ȴ�.
		// Vector3 lookDir = forwardDir * moveDir.z + rightDir * moveDir.x;
		// if (lookDir.sqrMagnitude > 0) // if(lookDir != Vector3.zero) �� ����� ���� �� ����(�� �ѹ�)
		// {
		//	Quaternion lookRotation = Quaternion.LookRotation(lookDir);
		//	transform.rotation = lookRotation;
		// }

		// 6. ȸ���ӵ��� �ʹ� ������ ������ õõ�� ȸ���ϵ��� ���� �������ֱ�
		Vector3 lookDir = forwardDir * moveDir.z + rightDir * moveDir.x;
		if (lookDir.sqrMagnitude > 0) // if(lookDir != Vector3.zero) �� ����� ���� �� ����(�� �ѹ�)
		{
			Quaternion lookRotation = Quaternion.LookRotation(lookDir);
			transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10);
			// lookRatation�� ���� transform.rotation���� Time.deltaTime * 100 �ӵ��� �������ش�.
		}


		Vector3 startPos = Vector3.zero;
		Vector3 endPos = Vector3.right;

		if(Vector3.Distance(startPos, endPos) < 1) {} // �Ʒ� �ڵ�� ���� ����
		if((endPos - startPos).magnitude < 1) {} // �� �ڵ�� ���� ����
		if((endPos - startPos).sqrMagnitude < 1 * 1) {} // �� �ڵ� �� �� ���� �ξ� ���� ����

		Vector3 vector3;
		// vector3.magnitude	-> ������ ũ�� ���ϱ�
		// vector3.sqrMagnitude	-> ��Ʈ ������ �������� ���� ũ�� ���ϱ�
		// vector3.normalized	-> ������ ���� ���ϱ�
	}
	private void OnMove(InputValue value)
	{
		Vector3 input = value.Get<Vector2>();
		moveDir.x = input.x;
		moveDir.z = input.y;

		animator.SetFloat("MoveSpeed",moveDir.magnitude);
	}
}