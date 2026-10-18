using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class SimpleTest : MonoBehaviour
{
    public Animator animator;
    public int StctAnimator;
    public bool ISPlay;

    public Transform target; // Целевой Transform, к которому двигаемся
    public Transform camera;
    public float moveSpeed = 5f; // Скорость перемещения
    public float rotationSpeed = 5f; // Скорость поворота
    public GameObject CamerFree;
    public GameObject plein;
    public GameObject Kykyr;
    public GameObject Istril;
    public GameObject Canvas;
    public GameObject CanvasPlay;
    public CinemachineFreeLook cinemachine;

    public Scrollbar scrollbar;
    public Scrollbar scrollbar2;
    public Vector3 positionCam;
    public bool isCam;
    public bool isplayer;
    public GameObject CAm2;
    public GameObject CAm3;
    public GameObject CAm4;
    public AirplaneController KYKYR;
    public AirplaneController Plane;
    public AirplaneController ISTRIL;
    public TMP_Text traction;


    void Update()
    {
        if (ISPlay == true) {
            // Плавное перемещение к позиции цели
            camera.position = Vector3.Lerp(
                camera.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            // Плавный поворот к ориентации цели
            camera.rotation = Quaternion.Slerp(
                camera.rotation,
                target.rotation,
                rotationSpeed * Time.deltaTime
            ); }
        //    float _tra = ISAnimator ? Plane.throttle * 100 : KYKYR.throttle * 100;
        float _tra = 0;
        switch (StctAnimator)
        {
            case 0:_tra = KYKYR.throttle * 100; break;
            case 1:_tra = Plane.throttle * 100; break;
            case 2: _tra = ISTRIL.throttle * 100; ; break;
        }

        traction.text = $"traction: {(int)_tra}";

       if(camera.position == target.position)
        {
            switch (StctAnimator)
            {
                case 0:
                    cinemachine.LookAt = Kykyr.transform;
                    cinemachine.Follow = Kykyr.transform; break;
                case 1:
                    cinemachine.LookAt = plein.transform;
                    cinemachine.Follow = plein.transform; break;
                case 2:
                    cinemachine.LookAt = Istril.transform;
                    cinemachine.Follow = Istril.transform; break;
            }

            CamerFree.SetActive(true);
        }

        switch (StctAnimator)
        {
            case 0:
                scrollbar.size = Mathf.MoveTowards(scrollbar.size, 0.5f, 5 * Time.deltaTime);
                scrollbar2.size = Mathf.MoveTowards(scrollbar2.size, 0.019f, 5 * Time.deltaTime); break;
            case 1:
                scrollbar.size = Mathf.MoveTowards(scrollbar.size, 0.75f, 5 * Time.deltaTime);
                scrollbar2.size = Mathf.MoveTowards(scrollbar2.size, 0.25f, 5 * Time.deltaTime); break;
            case 2:
                scrollbar.size = Mathf.MoveTowards(scrollbar.size, 1f, 5 * Time.deltaTime);
                scrollbar2.size = Mathf.MoveTowards(scrollbar2.size, 0.5f, 5 * Time.deltaTime); break;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCam = !isCam;
        }

        if (isCam)
        {
            cinemachine.gameObject.SetActive(false);
            switch (StctAnimator)
            {
                case 0: GameObJtrue(CAm2); break;
                case 1: GameObJtrue(CAm3); break;
                case 2: GameObJtrue(CAm4); break;
            }
            camera.gameObject.SetActive(false);
        }
        else
        {
            if (isplayer) {
                cinemachine.gameObject.SetActive(true);
            }
            switch (StctAnimator)
            {
                case 0: GameObJfoals(CAm2); break;
                case 1: GameObJfoals(CAm3); break;
                case 2: GameObJfoals(CAm4); break;
            }
            camera.gameObject.SetActive(true);
        }
    }

    public void PLAY()
    {
        ISPlay = true;
    }
    public void PLAY2()
    {
       Canvas.SetActive(false);
        CanvasPlay.SetActive(true);

        switch (StctAnimator) { 
            case 0: GameObJtrue(Kykyr);break; 
            case 1: GameObJtrue(plein); break; 
            case 2: GameObJtrue(Istril); break; }

        Invoke("CAM1", 1.5f);
    }

    void CAM1()
    {
        CamerFree.SetActive(true);
        isplayer = true;
    }

    public void clikleft()
    {  
        if (StctAnimator == 2)
        {
            animator.SetTrigger("SVIPS");
            StctAnimator--;
        }
        if (StctAnimator == 1 )
        {
            animator.SetTrigger("SVIP");
            StctAnimator--;
        }

    }

    public void clikRight()
    {
        if (StctAnimator == 1)
        {
            animator.SetTrigger("PRIVl");
            StctAnimator++;
        }
        if (StctAnimator == 0) {
            animator.SetTrigger("PRIV");
            StctAnimator++;
        }

    }

    public void GameObJtrue(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    public void GameObJfoals(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}