using Godot;
using System;

public partial class Player : XROrigin3D
{
	public XRController3D _leftHand;
	public XRController3D _rightHand;
	private bool jumping = false;
	private int Speed = 14;
	private Vector3 _targetVelocity = Vector3.Zero;
	private Vector3 direction = Vector3.Zero;
	private CharacterBody3D _player;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_leftHand = GetNode<XRController3D>("leftHand");
		_rightHand = GetNode<XRController3D>("rightHand");
		_player = GetNode<CharacterBody3D>("CharacterBody3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float moveforward = _leftHand.GetFloat("Thumbstick");
		//var _leftHand = GetNode<XRController3D>("leftHand");
		if (_rightHand.IsButtonPressed("A Click")) { jumping = true; }
		if (_leftHand.IsButtonPressed("primary")) {
			direction.X += 1f;
		}
		if (moveforward > .2) {
			direction.X += 1f;
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		_player.Velocity = _targetVelocity;
		_player.MoveAndSlide();
	}
}
