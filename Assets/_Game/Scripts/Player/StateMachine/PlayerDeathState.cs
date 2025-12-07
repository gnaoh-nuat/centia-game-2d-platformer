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

        // Tắt vật lý để không bị rơi hoặc bị đẩy nữa (tùy game design)
        player.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        // Vô hiệu hóa Collider để không bị quái đánh thêm
        player.GetComponent<Collider2D>().enabled = false;

        Debug.Log("GAME OVER");
    }
}