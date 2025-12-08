using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerController player, PlayerStateMachine stateMachine, InputReader inputReader)
        : base(player, stateMachine, inputReader) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Player Death State Entered");
        player.PlayAnimation(player.AnimID_Die);

        player.SetVelocityX(0);
        player.SetVelocityY(0);

        player.StartCoroutine(ReloadSceneRoutine());

        Debug.Log("GAME OVER");
    }

    private System.Collections.IEnumerator ReloadSceneRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneLoader.Instance.LoadScene(currentScene);
    }
}