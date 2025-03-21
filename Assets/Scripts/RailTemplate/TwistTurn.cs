using UnityEngine;

public class TwistTurn : MonoBehaviour
{
    [SerializeField] GameObject Rail;
    [SerializeField] int degree;
    [SerializeField] float length;
    [SerializeField] bool clockwise;

    private GameObject[] transforms;

    private void Awake()
    {
        int count = 360 / degree + 1;
        float len = length / count;
        if (clockwise) degree = -degree;

        transforms = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            transforms[i] = Instantiate(Rail, transform);

            transforms[i].GetComponent<Rail>().prohibitJump = true;

            transforms[i].transform.localScale = new(1, 1, len);
            transforms[i].transform.localPosition = new(0, 0, len * i);
            transforms[i].transform.localRotation = Quaternion.Euler(0, 0, degree * i);
        }
    }
}
