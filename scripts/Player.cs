
using Godot;

//Tentando fazer o sistema de movimento do mario, que contém aceleração.


public partial class Player : CharacterBody2D
{
	[Export]
	private int Speed = 300;
	
	[Export] 
	private int Jump = -400;

	[Export]
	public int Gravity = 500;
	
	private const float AccelerationTime = 0.5f;

	private float shiftPressedTime;
	private bool running = false;

	private float moveUnpressedTime;
	private float movePressedTime;

	
    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Velocity = Movimento(velocity, delta);

		MoveAndSlide();
		
	}

	private Vector2 Movimento(Vector2 velocity, double delta)
	{
		
		velocity.X -= velocity.X * 0;

		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;
		
		if (!Input.IsActionPressed("shift"))
			shiftPressedTime = 0;

		if (Input.IsAnythingPressed())
		{
			if (Input.IsActionJustPressed("jump") && IsOnFloor())
				velocity.Y = Jump;

			if (Input.IsActionPressed("left"))
				velocity.X -= Speed;

			if (Input.IsActionPressed("right"))
				
				velocity.X += Speed;

			if (Input.IsActionPressed("shift") && IsOnFloor())
			{
				shiftPressedTime += (float)delta;
				running = true;
				if (shiftPressedTime < AccelerationTime && shiftPressedTime > 0.1)
				{
					
					velocity.X += velocity.X * shiftPressedTime;
					
					
				}
					
				else if (shiftPressedTime >= AccelerationTime)
				{
					
					velocity.X += velocity.X * AccelerationTime;
					
					
				}
					
			}
		}

		return velocity;
	}
	// private float Correr(Vector2 velocity, double delta)
	// {
	// 	if (Input.IsActionPressed("shift") && IsOnFloor())
	// 		{
	// 			pressedTime += (float)delta;
	// 			running = true;
	// 			if (pressedTime < AccelerationTime && pressedTime > 0.1)
	// 			{
					
	// 				velocity.X += velocity.X * pressedTime;
					
					
	// 			}
					
	// 			else if (pressedTime >= AccelerationTime)
	// 			{
					
	// 				velocity.X += velocity.X * AccelerationTime;
					
					
	// 			}
					
	// 		}
	// 	return velocity.X;
	// }
	
}
