using Godot;

public partial class Root : Node3D
{
	[Export]
	public int MaximumRefreshRate = 120;
	private OpenXRInterface _xrInterface;
	[Signal]
	public delegate void PoseRecenteredEventHandler();

	public override void _Ready()
	{
		_xrInterface = (OpenXRInterface)XRServer.FindInterface("OpenXR");
		if (_xrInterface != null && _xrInterface.IsInitialized())
		{
			var viewport = GetViewport();
			viewport.UseXR = true;
			GD.Print("OpenXR initialized successfully");


			if (RenderingServer.GetRenderingDevice() != null)
			{
				viewport.VrsMode = Viewport.VrsModeEnum.XR;
			}

			// Turn off v-sync!
			DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);

			// Change our main viewport to output to the HMD
			GetViewport().UseXR = true;
			//_xrInterface.SessionBegun += OnOpenXRSessionBegun;
			_xrInterface.PoseRecentered += OnOpenXRPoseRecentered;
		}
		else
		{
			GD.Print("OpenXR not initialized, please check if your headset is connected");
			GetTree().Quit();
		}
	}

	 private void OnOpenXRSessionBegun()
	{
		var currentRefreshRate = _xrInterface.DisplayRefreshRate;
		GD.Print(currentRefreshRate > 0f);

		var newRefreshRate = currentRefreshRate;
		var availableRefreshRates = _xrInterface.GetAvailableDisplayRefreshRates();
		if (availableRefreshRates.Count == 1)
		{
			newRefreshRate = (float)availableRefreshRates[0];
		}
		else
		{
			GD.Print("OpenXR: Available refresh rates: ", availableRefreshRates);
			foreach (float rate in availableRefreshRates)
			{
				if (rate > newRefreshRate && rate <= MaximumRefreshRate)
				{
					newRefreshRate = rate;
				}
			}
		}

		if (currentRefreshRate != newRefreshRate)
		{
			GD.Print("Setting refresh rate to {newRefreshRate}");
			_xrInterface.DisplayRefreshRate = newRefreshRate;
			currentRefreshRate = newRefreshRate;
			Engine.PhysicsTicksPerSecond = (int)currentRefreshRate;
		}

	}
	private void OnOpenXRPoseRecentered()
	{
		//EmitSignal(SignalName.PoseCentered);
	}
}
