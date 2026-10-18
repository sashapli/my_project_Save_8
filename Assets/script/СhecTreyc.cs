using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class СhecTreyc : MonoBehaviour
{
    private float totalRotationX, totalRotationZ = 0f;
    private float previousAngleZ;
    private float previousAngleX;
    public float deltaZ;
    public float deltaX;
    public float currentAngleZ;
    public float currentAngleX;
    public Animator piryetAnim;
    public TMP_Text TrycsText;

    void Start()
    {
        previousAngleZ = transform.eulerAngles.z;
        previousAngleX = transform.eulerAngles.x;
    }

    void Update()
    {
        PiryetUpdater();
        SaltoUpdater();
    }
    private void PiryetUpdater()
    {
        currentAngleZ = transform.eulerAngles.z;
        deltaZ = Mathf.DeltaAngle(previousAngleZ, currentAngleZ);
        totalRotationZ += Mathf.Abs(deltaZ);

        if (totalRotationZ >= 360f)
        {
            Piruet("тангаж");
            totalRotationZ = 0f;
        }

        previousAngleZ = currentAngleZ;
    }
    private void SaltoUpdater()
    {
        currentAngleX = transform.eulerAngles.x;
        deltaX = Mathf.DeltaAngle(previousAngleX, currentAngleX);
        totalRotationX += Mathf.Abs(deltaX);

        if (totalRotationX >= 360f)
        {
            Piruet("сальто");
            totalRotationX = 0f;
        }

        previousAngleX = currentAngleX;
    }

    public void Piruet(string text)
    {
        TrycsText.text = text;
       // Debug.Log($"Полный оборот по оси x/z ! Накоплено: {totalRotationX }°");
        piryetAnim.SetTrigger("piryet");
    }
}
