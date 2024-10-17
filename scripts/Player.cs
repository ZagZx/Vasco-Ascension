using System;
using System.Diagnostics;
using Godot;

// Precisa desacelerar o player se soltar o shift enquanto corre
// ex: andou, segurou shift, soltou shift e continua andando

// Tô sentindo umas travadas no movimento de forma geral


public partial class Player : CharacterBody2D
{
	[Export]
	private float Speed = 300f;
	
	[Export] 
	private float JumpSpeed = -400f;

	[Export]
	public float Gravity = 1200f;
	
	// Variável que diminui a velocidade máxima de ANDAR
	[Export]
	private float Multiplier = 2f; // 1 para o valor padrão

	// Variável que aumenta a velocidade de CORRER
	[Export]
	private float Accelerator = 1; // 1 para o valor padrão


	private const float AccelerationTime = 0.5f;
	
	private float acceleration;

	private float shiftPressedTime;
	private float shiftUnpressedTime;
	private bool running = false;


    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		float fDelta = (float)delta;

		if (!IsOnFloor())
		{
			velocity.Y += Gravity * fDelta; //GAMBIARRA, depois vai ter uma função pro movimento inteiro
			Velocity = velocity;
		}
		Velocity = Movimento(velocity, fDelta);
		// Debug.WriteLine(IsOnWall());
		


		MoveAndSlide();
		
		
	}

	public Vector2 Movimento(Vector2 velocity, float fDelta){
		
		
		if (acceleration >= 0)
			velocity = MoveRight(velocity, fDelta);
		// if (acceleration <= 0)
		// 	velocity = MoveLeft(velocity, fDelta);
		// if (acceleration != 0)
		// 	velocity = Run(velocity, fDelta);
		velocity = Jump(velocity, fDelta);
		// velocity = FixColision(velocity);
		

		return velocity;
	}

	// public Vector2 FixColision(Vector2 velocity)
	// {

	// 	if (IsOnWall())
	// 	{
	// 		if (velocity.X < 0)
	// 		{
	// 			velocity.X += 1;
	// 		}
	// 		else if (velocity.X > 0)
	// 			velocity.X -= 1;
	// 	}
	// 	return velocity;
	// }
	public float Decelerate(Vector2 velocity)
	{
		if (acceleration != 0)
			velocity.X -= velocity.X * (acceleration * 0.3f);
		else{
			velocity.X = 0;
		}
		Debug.WriteLine(velocity.X);
		Debug.WriteLine((acceleration, "acc"));
			

		return velocity.X;
	}
	public Vector2 MoveRight(Vector2 velocity, float fDelta){
			
		if (Input.IsActionPressed("right"))
		{	
			if (acceleration < AccelerationTime)
			{
				acceleration += fDelta;
				velocity.X = Speed * (acceleration * Multiplier);
			}
			
			else if (acceleration >= AccelerationTime)
			{
				velocity.X = Speed * (AccelerationTime * Multiplier);
			}
			
		}
		
		else if (velocity.X > 0)
		{	
			velocity.X = Decelerate(velocity);
			if (acceleration != 0 )
			{
				acceleration -= fDelta;
				if (acceleration < 0.01f)
				{
					acceleration = 0;
				}
			}
			
		
		}
		// Debug.WriteLine(acceleration);
		// Debug.WriteLine(Velocity.X);
		return velocity;
	}

	public Vector2 MoveLeft(Vector2 velocity, float fDelta)
	{
		if (Input.IsActionPressed("left"))
		{	
			if (acceleration > -AccelerationTime)
				acceleration -= fDelta;
			
			if (acceleration < -AccelerationTime)
			{	
				
				velocity.X = Speed * (acceleration * Multiplier);
			}
			
			else if (acceleration >= AccelerationTime)
			{
				velocity.X = Speed * -(AccelerationTime * Multiplier);
			}
			
			
		}
		
		else if (velocity.X < 0)
		{	
			
			acceleration += fDelta;

			velocity.X = Decelerate(velocity);
		}
		return velocity;
	}

	// public Vector2 Run(Vector2 velocity, float fDelta){
	// 	if (Input.IsActionPressed("shift"))
	// 	{
	// 		shiftPressedTime += fDelta;
	// 		shiftUnpressedTime = 0;
			
	// 		running = true;
	// 		if (shiftPressedTime < AccelerationTime)
	// 			velocity.X += velocity.X * shiftPressedTime;
	// 		else if (shiftPressedTime >= AccelerationTime)
	// 			velocity.X += velocity.X * AccelerationTime;
	// 	}
	// 	else if (running){
	// 		shiftPressedTime = 0;
	// 		shiftUnpressedTime += fDelta;

	// 		if (shiftUnpressedTime < AccelerationTime)
	// 		{	
				
	// 			velocity.X -= Speed * (shiftUnpressedTime * 0.5f);
				
	// 		}
	// 		else if (shiftUnpressedTime >= AccelerationTime)
	// 		{
	// 			velocity.X -= Speed * (AccelerationTime);
				
	// 			running = false;
	// 		}
	// 	}
	// 	return velocity;
		
	//}
	public Vector2 Jump(Vector2 velocity, float fDelta){
		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("jump"))
			{
				velocity.Y = JumpSpeed;
			}
		}
		return  velocity;
	}
}