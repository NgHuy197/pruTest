using UnityEngine;

public class SimpleInputController : MonoBehaviour
{
    public Player player;

    public KeyCode attackKey;

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            player.PerformLightAttack();
        }
    }
}