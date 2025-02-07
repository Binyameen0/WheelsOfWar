using UnityEngine;

public class CarDriverAI : MonoBehaviour
{
    [SerializeField] private Transform playerCarTransform; // Reference to the player's car transform.
    private CarController carController;                  // Reference to the AI car's controller.

    public float stoppingDistance = 5f;                   // Distance to stop near the player.
    public float maxSpeed = 20f;                          // Maximum speed of the AI car.
    public float turnSensitivity = 1.5f;                  // Steering sensitivity.

    private void Awake()
    {
        // Ensure CarController is attached to AI car.
        carController = GetComponent<CarController>();
        if (carController == null)
        {
            Debug.LogError("CarController is missing on this AI car!");
        }

        // Ensure the player car transform is assigned.
        if (playerCarTransform == null)
        {
            Debug.LogError("Player car transform is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        // If the player car is not assigned, do nothing.
        if (playerCarTransform == null) return;

        // Move the AI car towards the player's position.
        MoveTowardPlayer();
    }

    private void MoveTowardPlayer()
    {
        Vector3 targetPosition = playerCarTransform.position;  // Get the player's position.
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;  // Direction vector to the player.

        // Calculate the distance to the player.
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        float forwardAmount = 0f; // AI's forward movement amount.
        float turnAmount = 0f;    // AI's turning amount.

        if (distanceToTarget > stoppingDistance)
        {
            // If the AI is farther than the stopping distance, move towards the player.
            forwardAmount = Mathf.Clamp01(1f);  // Full forward speed.

            // Calculate the angle to the player and determine the turning amount.
            float angleToTarget = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);
            turnAmount = Mathf.Clamp(angleToTarget / 45f * turnSensitivity, -1f, 1f);
        }
        else
        {
            // If within stopping distance, stop moving.
            forwardAmount = 0f;
            turnAmount = 0f;
        }

        // Apply forward and turn inputs to the CarController.
        carController.SetAIInputs(forwardAmount, turnAmount);
    }

    // Detect collision with the player and apply damage to both cars.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply damage to both the player and the AI when they collide.
            float damageAmount = 10f;  // Base damage amount.

            // If player hits the AI, apply double damage to the AI.
            if (collision.relativeVelocity.magnitude > 0)
            {
                damageAmount *= 2f; // Double damage for AI when hit by the player.
                carController.TakeDamage(damageAmount);
                collision.gameObject.GetComponent<CarController>().TakeDamage(damageAmount / 2f); // Normal damage to the player.
            }
        }
    }
}
