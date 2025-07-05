using UnityEngine;

public class PlayerMuveSemDots : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;

    [Header("Referências")]
    public Camera mainCamera;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 mousePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Se não tiver uma câmera atribuída, pega a principal
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // Entrada de movimentação
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        // Pega a posição do mouse na tela e converte para o mundo
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            mousePosition = ray.GetPoint(distance);
        }

        // Faz o objeto olhar para o mouse
        Vector3 lookDirection = (mousePosition - transform.position).normalized;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 15f);
        }
    }

    void FixedUpdate()
    {
        // Move o personagem
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
