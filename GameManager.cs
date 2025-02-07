using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text gameResultText; // Reference to the UI text for displaying the result
    public CarController playerCar; // Reference to the player's car
    public CarDriverAI enemyCar; // Reference to the enemy car

    private bool isGameOver = false; // Flag to check if the game has ended

    private void Update()
    {
        // Check if the game is already over
        if (isGameOver) return;

        // Check win/lose/draw conditions
        if (playerCar.health <= 0 && enemyCar.GetComponent<CarController>().health <= 0)
        {
            EndGame("Draw");
        }
        else if (playerCar.health <= 0)
        {
            EndGame("You Lose");
        }
        else if (enemyCar.GetComponent<CarController>().health <= 0)
        {
            EndGame("You Win");
        }
    }

    private void EndGame(string result)
    {
        isGameOver = true;

        // Display the game result
        if (gameResultText != null)
        {
            gameResultText.text = result;
            gameResultText.gameObject.SetActive(true);
        }

        // Optionally, stop cars from moving
        DisableCarControls(playerCar);
        DisableCarControls(enemyCar.GetComponent<CarController>());
    }

    private void DisableCarControls(MonoBehaviour carController)
    {
        if (carController != null)
        {
            carController.enabled = false;
        }
    }
}
