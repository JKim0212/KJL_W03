using UnityEngine;

public class TwistTurn : MonoBehaviour
{
    [SerializeField] GameObject rail;
    [SerializeField] Material material;
    [SerializeField] float railSpeedRatePercent;
    [SerializeField] int degree;
    [SerializeField] float length;

    private void Awake()
    {
        if (degree == 0) degree = 1;

        int count = 360 / (degree > 0 ? degree : -degree) + 1;
        float len = length / count;

        for (int i = 0; i < count; i++)
        {
            GameObject newRail = Instantiate(rail, transform);
            newRail.GetComponent<Renderer>().material = material;
            newRail.GetComponent<Rail>().Init(railSpeedRatePercent, true);
            newRail.transform.localScale = new(1, 1, len);
            newRail.transform.localPosition = new(0, 0, len * i);
            newRail.transform.localRotation = Quaternion.Euler(0, 0, degree * i);
        }
    }
}
