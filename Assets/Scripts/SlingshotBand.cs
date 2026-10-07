using UnityEngine;

public class SlingshotBand : MonoBehaviour
{
    public LineRenderer backBand;
    public LineRenderer frontBand;
    public Transform leftFork;
    public Transform rightFork;
    public Transform bird;

    public void UpdateBand()
    {
        if (bird == null)
            return;
        backBand.SetPosition(0, rightFork.position);
        backBand.SetPosition(1, bird.position);
        frontBand.SetPosition(0, bird.position);
        frontBand.SetPosition(1, leftFork.position);
        
        backBand.enabled = true;
        frontBand.enabled = true;
    }

    public void HideBand()
    {
        backBand.enabled = false;
        frontBand.enabled = false;
    }
}
