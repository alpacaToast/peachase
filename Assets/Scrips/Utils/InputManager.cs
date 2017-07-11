using UnityEngine;

public struct ProcessedInput {
    public int tapCount;
    public Vector2 touchLocation;
	public TouchPhase phase;
}

public enum TouchLocation { UP, LEFT, DOWN, RIGHT}

public static class InputManager
{
    public static ProcessedInput ProcessTapInput(Touch touch)
    {
        var processedInput = new ProcessedInput();
        processedInput.tapCount = touch.tapCount;
		processedInput.phase = touch.phase;

        Vector2 touchLoc = touch.position;
        touchLoc.x /= Camera.main.pixelWidth;
        touchLoc.y /= Camera.main.pixelHeight;
        processedInput.touchLocation = touchLoc;

        return processedInput;
    }

	public static ProcessedInput ProcessSlideInput(Touch touch)
	{
		var processedInput = new ProcessedInput();
		processedInput.tapCount = touch.tapCount;
		processedInput.phase = touch.phase;

		Vector2 touchLoc = touch.deltaPosition;
		return processedInput;
			
	}
}