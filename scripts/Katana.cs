using Godot;
using System;

public partial class Katana : CharacterBody2D
{

	const float katanaSpeed = 350f;
	// const float katanaGravity = 0.5f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		
		Vector2 velocity = Velocity;
	
		velocity.X = katanaSpeed;
			
		Velocity = velocity;

		MoveAndSlide();
		
	}
}
