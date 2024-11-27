using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviourPunCallbacks
{

    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;


    public float lookSpeed = 2f;
    public float lookXLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    public Slider staminaBar;
    public float stamina, maxStamina;
    public float runCost;
    public float chargeRate;
    public bool isRecharging;


    CharacterController characterController;
    void Start()
    {


        if (photonView.IsMine) //SE A VISAO FOR DO JOGADOR
        {
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


        }

        if (!photonView.IsMine)
        {
            staminaBar.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        if (photonView.IsMine) //SE A VISAO FOR DO JOGADOR
        {
            #region Handles Movment
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Press Left Shift to run
            bool canRun = stamina > 0;
            bool isRunning = Input.GetKey(KeyCode.LeftShift) && canRun; // Só corre se houver stamina

            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            staminaBar.value = stamina / maxStamina;
            if (isRunning)
            {
                stamina -= runCost * Time.deltaTime; // Reduz stamina enquanto corre
                if (stamina < 0) stamina = 0; // Garante que não fique negativa
                isRecharging = false; // Reset da regeneração
            }
            else if (!isRecharging && stamina < maxStamina)
            {
                StartCoroutine(rechargeStamina()); // Inicia regeneração se não estiver recarregando
            }




            #endregion

            #region Handles Jumping
            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            #endregion

            #region Handles Rotation
            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }

            #endregion
        }

    }

    private IEnumerator rechargeStamina()
    {
        isRecharging = true; // Marca como regenerando
        yield return new WaitForSeconds(1f); // Aguarda 1 segundo antes de começar

        while (stamina < maxStamina)
        {
            stamina += chargeRate * Time.deltaTime; // Regenera gradualmente
            if (stamina > maxStamina) stamina = maxStamina; // Garante que não ultrapasse o máximo
            staminaBar.value = stamina / maxStamina; // Atualiza a barra de stamina
            yield return null; // Espera até o próximo frame
        }

        isRecharging = false; // Termina regeneração
    }
}
