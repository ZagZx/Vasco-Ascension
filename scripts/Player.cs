using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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

	private const float AccelerationTime = 0.5f;
	
	private float acceleration;


	public Marker2D katanaPosition;

	PackedScene katanaScene = GD.Load<PackedScene>("scenes/KatanaThrow.tscn");

	
	Node2D attackInstance;
	PackedScene katanaAttack = GD.Load<PackedScene>("scenes/KatanaAttack.tscn");
	AnimatedSprite2D attackAnimation;

	bool tocando = false;
	
	

    public override void _Ready()
    {
		katanaPosition = GetNode<Marker2D>("katanaPosition");
    }
    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		float fDelta = (float)delta;

		
		Velocity = Movimento(velocity, fDelta);
		
		AtirarFaca();
		Atacar();

		// Debug.WriteLine(IsOnWall());
		
		

		MoveAndSlide();
		// MoveAndCollide(Velocity*fDelta);

		
		
		
	}
	public void AtirarFaca()
	{	
		if (Input.IsActionJustPressed("f"))
		{
			Node2D katanaInstance = (Node2D)katanaScene.Instantiate();
			AddSibling(katanaInstance);
			
			katanaInstance.GlobalPosition = katanaPosition.GlobalPosition;
		}
	}
	public void Atacar()
	{
		

		
		if (Input.IsActionJustPressed("throw") && !tocando)
		{
			attackInstance = (Node2D)katanaAttack.Instantiate();
			AddChild(attackInstance);
			attackInstance.GlobalPosition =	katanaPosition.GlobalPosition;

			attackAnimation = attackInstance.GetNode<AnimatedSprite2D>("AreaAttack/Animation");
			attackAnimation.Play();
			tocando = true;
		}
		
		//GAMBIARRA
		try{
			if (!attackAnimation.IsPlaying())
			{   
				tocando = false;
				attackInstance.QueueFree();
			}
		}
		catch{

		}
		
	
		

		
	}

	public Vector2 Movimento(Vector2 velocity, float fDelta)
	{	
		Debug.WriteLine(IsOnWall());
		if (IsOnFloor())
		{
			velocity.Y = 0;
			Velocity = velocity;
		}

		else if (!IsOnFloor())
		{
			velocity.Y += Gravity * fDelta; //GAMBIARRA, depois vai ter uma função pro movimento inteiro
			Velocity = velocity;
		}



		if (acceleration >= 0)
		{
			velocity = MoveRight(velocity, fDelta);
			
		}
			
		if (acceleration <= 0)
		{
			velocity = MoveLeft(velocity, fDelta);
		}
	
		// if (acceleration != 0) 
		// 	velocity = Run(velocity, fDelta);
		velocity = Jump(velocity, fDelta);
		
		return velocity;
	}

	public float Decelerate(Vector2 velocity)
	{
		if (acceleration != 0)
		{	
			if (velocity.X > 0)
			{
				velocity.X -= velocity.X * (acceleration * 0.3f);
			}
				
			else
			{
				velocity.X += velocity.X * (acceleration * 0.3f);
			}
			// Debug.WriteLine(velocity.X);
			// Debug.WriteLine((acceleration, "acc"));
		}
		else
		{
			velocity.X = 0;
		}
		
		return velocity.X;
	}
	public Vector2 MoveRight(Vector2 velocity, float fDelta)
	{	
	
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

		if (IsOnWall())
		{
			acceleration = 0;
			Position = Position with{X = Position.X -1};
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
			{	
				acceleration -= fDelta;
				velocity.X = Speed * (acceleration * Multiplier);
			}
			
			else if (acceleration <= -AccelerationTime)
			{
				velocity.X = Speed * -(AccelerationTime * Multiplier);
			}
			
			
		}
		
		else if (velocity.X < 0)
		{	
			if (IsOnWall()){
				acceleration = 0;
			}
			else
			{
				velocity.X = Decelerate(velocity);

				if (acceleration != 0 )
				{
					acceleration += fDelta;
					if (acceleration > -0.01f)
					{
						acceleration = 0;
					}
				}
			}
		}

		if (IsOnWall())
		{
			acceleration = 0;
			Position = Position with{X = Position.X +1};
		}
		
		return velocity;
	}

	public Vector2 Run(Vector2 velocity, float fDelta)
	{
	
		return velocity;
	}
	public Vector2 Jump(Vector2 velocity, float fDelta)
	{
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
