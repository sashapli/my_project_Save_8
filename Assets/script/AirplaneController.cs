using UnityEngine;
using UnityEngine.SceneManagement;
public class AirplaneController : MonoBehaviour
{
    // Сохраняем все ваши оригинальные переменные
    public float maxMotorTorque = 100f;
    public float maxSteeringAngle = 30f;
    public WheelCollider[] rearWheel;
    public Rigidbody rb;

    public float maxEnginePower = 150f;
    private float currentThrottle = 0f;
    public float throttleSpeed = 0.5f;

    [Header("Input Settings")]
    public float throttle;
    public float brake;
    private float pitch;
    private float roll;
    private float yaw;

    [Header("Physics Settings")]
    public float enginePower = 100f;
    public float maxSteerAngle = 30f;
    public float brakePower = 100f;
    public float pitchSpeed = 20f;
    public float rollSpeed = 20f;
    public float yawSpeed = 10f;

    [Header("Aerodynamics")]
    public float pitchTorque = 50f; 
    public float pitchRight = 50f;
    public float pitchUp = 50f;
    public float pitchforward = 50f;
    public float rollTorque = 40f;
    public float yawTorque = 30f;
    public float airDrag = 0.01f;

    [Header("Gravity Settings")]
    public float baseGravity = 9.81f;
    public float DinamicGravity = 9.81f;
    public float maxSpeed = 15f;
    public float minGravityFactor = 0.1f;
    public float gravityTransitionSpeed = 2f;
    private float currentGravityFactor = 1f;
    public float GravicsRoteit;

    [Header("Your Custom Forces")]
    public float vint1;
    public float speedVint;
    public Transform vint;
    public float SpeedKrenGrivics;

    private float totalHorizontalSpeed;
    private Vector3 Derection;
    public float RightRotationGrounded;

    public float maxAngle = -90f;
    public float minOutputValue = 1f;
    public float maxValue = 100f;
    public float divider = 10f;

    [SerializeField] private float currentValue;
    public float EngenEnergi;
    public float EngenEnergied;
    public float testGravics;
    [Range(0, 50)] public float energi;
    public float SpeedSval;
    public float ForceUp;
    public bool IsGame;
    public GameObject pauzMin;

    [SerializeField] private float stopDuration = 1f; // Длительность остановки в секундах
    public bool isStopping = false;
    public float stopTimer = 0f;
    private Vector3 initialVelocity;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb.useGravity = true;
    }

    void Update()
    {
        isStopping = Input.GetKey(KeyCode.Space);
        // Сохраняем ваш оригинальный ввод
        throttle = Input.GetAxis("Engen");
        if (throttle < 0) { throttle = 0; }
        brake = Input.GetKey(KeyCode.Space) ? 1f : 0f;
        roll = Input.GetAxis("Horizontal");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");
        vint1 = Input.GetAxis("vint");
        if (vint1 < 0) { vint1 = 0; }

        // Вращение винта (сохраняем вашу логику)
        if (vint != null)
        {
            vint.Rotate(Vector3.up * vint1 * speedVint);
        }
        // Расчет скорости (как у вас было)
        totalHorizontalSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
        Derection = rb.velocity.normalized;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsGame = !IsGame;
        }
        pauzMin.SetActive(IsGame);
        rb.isKinematic = IsGame;
        Cursor.lockState = !IsGame ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void FixedUpdate()
    {
        // Обновляем гравитацию
        UpdateGravity();

        if (IsGrounded())
        {
            GroundMovement();
            // Ваши оригинальные силы на земл  
            rb.AddForce(Vector3.down * DinamicGravity * MirrorNumber(throttle), ForceMode.Force);
           rb.AddForce(transform.forward * pitchforward * throttle*  pitch);
            rb.AddForce(transform.up * pitchUp * throttle * pitch);
            rb.AddTorque(transform.right * totalHorizontalSpeed * pitchRight *  pitch, ForceMode.Acceleration);
            rb.drag = 0.1f; // Минимальное сопротивление на земле
        }
        else
        {
            AirMovement();
            // Calculate engine power with inertia
            float angleX = transform.eulerAngles.x;
            if (angleX > 180f) angleX -= 360f;
            angleX = Mathf.Max(angleX, maxAngle);
            float normalized = Mathf.InverseLerp(0f, maxAngle, angleX);
            float rawValue = Mathf.Lerp(minOutputValue, maxValue, normalized);
            currentValue = Mathf.Max(rawValue / divider, minOutputValue);

            rb.AddForce(Vector3.down * (baseGravity *  MirrorNumber(throttle) + (currentValue * SpeedSval)), ForceMode.Force);
            CalculateAerodynamicDrag(); // Ваш оригинальный метод
        }

      
    }
    public float MirrorNumber(float input)
    {
        // Ограничиваем входное значение в диапазоне 0-1
        float clamped = Mathf.Clamp01(input);
        // "Зеркально" отражаем число
        return 1f - clamped;
    }
    void UpdateGravity()
    {
       /* // Плавное изменение гравитации в зависимости от скорости
        float targetGravityFactor = Mathf.Lerp(1f, minGravityFactor, Mathf.Clamp01(totalHorizontalSpeed / maxSpeed));
        currentGravityFactor = Mathf.Lerp(currentGravityFactor, targetGravityFactor, gravityTransitionSpeed * Time.fixedDeltaTime);

        // Применяем модифицированную гравитацию
        rb.AddForce(Physics.gravity * (currentGravityFactor - 1f), ForceMode.Acceleration);

        // Отключаем стандартную гравитацию на высокой скорости
        rb.useGravity = totalHorizontalSpeed < maxSpeed * 0.8f;*/
    }

    void GroundMovement()
    {


        if (brake == 1)
        {
            foreach (var wheel in rearWheel)
            {
                wheel.brakeTorque = maxMotorTorque * 5;
            }
            //rearWheel[2].motorTorque = 0;

            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * stopTimer);
        }
        else
        {
            foreach (var wheel in rearWheel)
            {
                wheel.brakeTorque = 0;
            } 
            // Ваша оригинальная логика движения по земле
        float motorInput = -Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");

        rearWheel[0].steerAngle = maxSteeringAngle * steeringInput;
        rearWheel[1].steerAngle = maxSteeringAngle * steeringInput;
        rearWheel[2].motorTorque = maxMotorTorque * motorInput;
            

            if (isStopping)
            {
            rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, Time.fixedDeltaTime * stopTimer);
            }
        }
    }


    void AirMovement()
    {
        currentThrottle = Mathf.Lerp(currentThrottle, throttle, throttleSpeed * Time.fixedDeltaTime);


        // Apply forces with inertia
        Vector3 engineForce = transform.forward * maxEnginePower * currentThrottle;

        rb.AddForce(engineForce, ForceMode.Force);

        // Управление в воздухе с учетом ваших параметров
        float controlFactor = Mathf.Clamp01(rb.velocity.magnitude / 20f);

        rb.AddRelativeTorque(
            Vector3.right * pitch * pitchTorque * controlFactor * Time.fixedDeltaTime,
            ForceMode.Acceleration);

        rb.AddRelativeTorque(
            Vector3.forward * roll * rollTorque * controlFactor * Time.fixedDeltaTime,
            ForceMode.Acceleration);

        rb.AddTorque(
            Vector3.up * yaw * yawTorque * controlFactor * 0.5f * Time.fixedDeltaTime,
            ForceMode.Acceleration);
    }

    void CalculateAerodynamicDrag()
    {
        // Улучшенная версия вашего оригинального метода
        if (rb.velocity.magnitude < 0.1f)
        {
            rb.drag = 0f;
            return;
        }

        // Базовое сопротивление, увеличивающееся со скоростью
        float speedFactor = rb.velocity.magnitude / 50f;
        float baseDragValue = Mathf.Lerp(0.1f, 1f, speedFactor);

        // Учет угла атаки (как у вас было)
        float angle = Vector3.Angle(transform.forward, rb.velocity.normalized);
        float angleFactor = Mathf.Clamp01(angle / 90f);

        // Плавное изменение сопротивления
        rb.drag = Mathf.Lerp(rb.drag, Mathf.Lerp(baseDragValue, 5f, angleFactor), Time.fixedDeltaTime * 5f);
    }

    bool IsGrounded()
    {
        foreach (var wheel in rearWheel)
        {
            if (wheel.isGrounded)
                return true;
        }
        return false;
    }

    public void back()
    {
        SceneManager.LoadScene("plane");
    }
    public void resume()
    {
        IsGame = !IsGame;
    }
}