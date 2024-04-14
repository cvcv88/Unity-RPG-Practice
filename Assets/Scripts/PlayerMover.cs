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
		// 1. 공중에 뜨는 오류 코드
		// 플레이어가 바라보고 있는 방향 구하기 -> 메인 카메라가 보고 있는 앞 방향
		Vector3 forwardDir = Camera.main.transform.forward; // z축
		Vector3 rightDir = Camera.main.transform.right; // x축

		// 2. y=0 적용한 수정 코드
		// 땅과 평행하게 y좌표만 0으로 수정하기
		forwardDir = new Vector3(forwardDir.x, 0, forwardDir.z);
		rightDir = new Vector3(rightDir.x, 0, rightDir.z);

		// 3. 크기를 1로 만들어 속도 일정하게 유지하도록 하는 수정 코드
		// 해당 방향에서의 크기가 1인 벡터(normalized)가 필요(방향만 필요하기 때문)
		forwardDir = new Vector3(forwardDir.x, 0, forwardDir.z).normalized;
		rightDir = new Vector3(rightDir.x, 0, rightDir.z).normalized;

		controller.Move(forwardDir * moveDir.z * moveSpeed * Time.deltaTime); // 앞 방향
		controller.Move(rightDir * moveDir.x * moveSpeed * Time.deltaTime); // 옆 방향

		// 4. 입력하는 방향대로 움직일 수 있게 회전
		// Quaternion.LookRotation() : 해당 벡터를 바라보는 회전 상태를 반환
		// 메서드 안에는 상대 좌표를 넣어줘야 한다
		// Vector3 lookDir = forwardDir * moveDir.z + rightDir * moveDir.x;
		//Quaternion lookRotation = Quaternion.LookRotation(lookDir);
		// transform.rotation = lookRotation;

		// 5. 키를 누르다가 뗐을 때 어느 방향에서든 앞 방향을 바라보게 되는 오류가 있다.
		// LookRotation은 zero Vector를 가질 수 없다. 입력이 없을 때는 어디를 바라봐야 하지? z축을 바라보게 된다.
		// Vector3 lookDir = forwardDir * moveDir.z + rightDir * moveDir.x;
		// if (lookDir.sqrMagnitude > 0) // if(lookDir != Vector3.zero) 이 방법이 조금 더 빠름(비교 한번)
		// {
		//	Quaternion lookRotation = Quaternion.LookRotation(lookDir);
		//	transform.rotation = lookRotation;
		// }

		// 6. 회전속도가 너무 빠르기 때문에 천천히 회전하도록 선형 보간해주기
		Vector3 lookDir = forwardDir * moveDir.z + rightDir * moveDir.x;
		if (lookDir.sqrMagnitude > 0) // if(lookDir != Vector3.zero) 이 방법이 조금 더 빠름(비교 한번)
		{
			Quaternion lookRotation = Quaternion.LookRotation(lookDir);
			transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10);
			// lookRatation을 지금 transform.rotation에서 Time.deltaTime * 100 속도로 보간해준다.
		}


		Vector3 startPos = Vector3.zero;
		Vector3 endPos = Vector3.right;

		if(Vector3.Distance(startPos, endPos) < 1) {} // 아래 코드와 완전 동일
		if((endPos - startPos).magnitude < 1) {} // 위 코드와 완전 동일
		if((endPos - startPos).sqrMagnitude < 1 * 1) {} // 위 코드 두 줄 보다 훨씬 빠른 연산

		Vector3 vector3;
		// vector3.magnitude	-> 벡터의 크기 구하기
		// vector3.sqrMagnitude	-> 루트 연산을 진행하지 않은 크기 구하기
		// vector3.normalized	-> 벡터의 방향 구하기
	}
	private void OnMove(InputValue value)
	{
		Vector3 input = value.Get<Vector2>();
		moveDir.x = input.x;
		moveDir.z = input.y;

		animator.SetFloat("MoveSpeed",moveDir.magnitude);
	}
}