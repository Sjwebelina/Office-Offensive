using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [SerializeField] private float maxStamina;
    [SerializeField] private float playerStamina;
    [SerializeField] private float minimumRequiredStamina;
    private bool sprinting = false;

    [Range(0,50)] [SerializeField] private float staminaDrain;
    [Range(0,50)] [SerializeField] private float staminaRegen;

    [SerializeField] private Image staminaProgressUI;
    [SerializeField] private CanvasGroup sliderCanvasGroup;
    
    private PlayerMovement playerController;

    void Start()
    {
        playerController = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(!sprinting)
        {
            RegenerateStamina();
        }
        else
        {
            Sprinting();
        }
    }

    void RegenerateStamina()
    {
        if (playerStamina <= maxStamina)
        {
            playerStamina += staminaRegen * Time.deltaTime;

            if (playerStamina >= maxStamina)
            {
                playerStamina = maxStamina;
                sliderCanvasGroup.alpha = 0;
            }
        }
    }

    void Sprinting()
    {
        playerStamina -= staminaDrain * Time.deltaTime;

        if (playerStamina <= 0)
        {
            playerStamina = 0;

            sprinting = false;

            playerController.SetSprintingBool(false);

            sliderCanvasGroup.alpha = 0;
        }
    }

    public void CheckCanSprint()
    {
        if (playerStamina >= minimumRequiredStamina)
        {
            playerController.SetSprintingBool(true);
            sprinting = true;
        }
    }

    public void StopSprinting()
    {
        sprinting = false;
        playerController.SetSprintingBool(false);
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if(value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
