using UnityEngine;

public class PlayerBootstrap : MonoBehaviour
{
    private PlayerModel playerModel;

    private PlayerPresenter playerPresenter;

    public void Init(PlayerUI playerView, PlayerMovement playerMovement, PlayerAtack playerAtack)
    {
        playerModel = new PlayerModel();

        playerPresenter = new PlayerPresenter(playerView, playerMovement, playerAtack, playerModel);

        playerPresenter.Enable();

        playerView.Init();
        playerAtack.Init();
        playerMovement.Init();
    }

    private void OnDisable()
    {
        if (playerPresenter != null)
            playerPresenter.Disable();
    }
}
