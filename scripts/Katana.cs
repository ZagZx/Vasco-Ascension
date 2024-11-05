using Godot;
using System;

public partial class Katana : CharacterBody2D
{

	const float katanaSpeed = 15f;
	// const float katanaGravity = 0.5f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		
		Vector2 velocity = Velocity;
		// if(!IsOnWall() && !IsOnFloor()){
			velocity.X += katanaSpeed * (float)delta;
			// velocity.Y += 1 * (float)delta;
		// }
		Velocity = velocity;

		MoveAndCollide(Velocity);
		// MoveAndSlide();
	}
}
