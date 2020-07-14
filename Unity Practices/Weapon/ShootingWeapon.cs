using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extentions;

public class ShootingWeapon : Weapon
{
    [Space]
    [Header("shooting weapon settings")]
    [SerializeField] protected int _maxMagCapacity;
    [SerializeField] protected int _currentMagCapacity;
    [SerializeField] protected int _ammoLeft;
    [SerializeField] protected float _reloadTime;
    protected float _timeBetweenReloads;

    [SerializeField] protected GameObject _shotBurst;
    [SerializeField] protected float _burstLength;

    [Header("Bullet entity")]
    [SerializeField] protected float _damageOfEachFraction = 0;
    [SerializeField] protected float _powerOfEachFraction = 0;
    protected int _fractionCount = 1;

    [Header("shot direction")]
    protected Vector3 _difference;
    [SerializeField] protected Transform _shotPoint;
    [SerializeField] protected float _scatterValue;
    [SerializeField] protected float _shotRadius;
    protected float _rotZ;

    [Header("Rotate settings")]
    [SerializeField] protected float _deathZoneAngle = 0;
    [SerializeField] protected Transform _flipPoint;
    [SerializeField] protected float _maxShootDistance;
    protected float _currentRadius;

    //rotating variables
    public float _direction = -1;
    public float _magnitude;
    protected float _lastMagnitude;
    
    protected Coroutine _shootingCR = null;
    protected bool _shooting = false;

    //cache 
    protected Animator _anim;
    protected Vector3 _mousePosition;
    protected Camera _mainCamera;

    public bool NotInDeathZone { get; set; }
    
    private bool LookRight => _rotZ > (-90 + _deathZoneAngle) && _rotZ < (90 - _deathZoneAngle);
    private bool LookLeft => ((_rotZ < (-90 - _deathZoneAngle) && _rotZ > -180) || (_rotZ < 180 && _rotZ > (90 + _deathZoneAngle)));

    protected override void Awake()
    {
        _anim = GetComponent<Animator>();
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (_timeBetweenReloads > 0)
            _timeBetweenReloads -= Time.deltaTime;
    }
    
    public override void StartShoot()
    {
        if (!_shooting)
        {
            _shootingCR = StartCoroutine(Shooting());
            _anim.SetBool("_isShooting", _shooting);
        }
    }

    protected IEnumerator Shooting()
    {
        _shooting = true;
        while (_shooting)
        {
            Shoot();
            yield return new WaitForSeconds(_shootCooldown);
        }
    }

    protected void Shoot()
    {
        if (_currentMagCapacity > 0)
        {
            if (_timeBetweenShots <= 0.0f && _timeBetweenReloads <= 0.0f && NotInDeathZone)
            {
                _currentMagCapacity--;
                var _newShot = Instantiate(_shotBurst);
                _newShot.transform.rotation = _shotPoint.rotation;
                _newShot.transform.position = _shotPoint.position;

                _newShot.HandleComponent<Animator>(
                        (Component) =>
                        {
                            Component.StopPlayback();
                            Component.SetBool("shot", true);
                            StartCoroutine(PlayAnimationOnce(_burstLength, Component));
                        }
                    );

                for (int _fraction = 0; _fraction < _fractionCount; _fraction++)
                {
                    WeaponRaycastHandler.PerformShoot(_shotPoint.position, GetShotDirection(), _maxShootDistance, this, _damageOfEachFraction, _powerOfEachFraction);
                }
                
                _timeBetweenShots = _shootCooldown;
            }
            else
            {
                return;
            }
        }
        else
        {
            //TODO: play empty sound
        }
    }

    protected IEnumerator PlayAnimationOnce(float seconds, Animator animator)
    {
        yield return new WaitForSeconds(seconds);
        animator.SetBool("shot", false);
    }

    protected Vector3 GetShotDirection()
    {
        return (_mousePosition + Random.insideUnitSphere * _scatterValue) - _shotPoint.position;
    }

    public override void StopShoot()
    {
        StopCoroutine(_shootingCR);
        _shooting = false;
        _anim.SetBool("_isShooting", _shooting);
    }

    public override void Aim()
    {
        if (_flipPoint != null)
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _difference = _mousePosition - _flipPoint.transform.position;
            _rotZ = Mathf.Atan2(_difference.y, _difference.x) * Mathf.Rad2Deg;
            SetBlend3();

            _currentRadius = new Vector2(_difference.x, _difference.y).magnitude;
        }
        else
        {
            throw new System.Exception("missing flip point");
        }
    }
    
    protected void SetBlend3()
    {
        if (LookRight)
        {
            if (_player._direction == Direction.left)
            {
                _player.Flip();
            }
            else if (_direction != 1)
            {
                _direction = 1;
            }

            NotInDeathZone = true;

            _magnitude = _rotZ / (90 - _deathZoneAngle);
        }
        else if (LookLeft)
        {
            if (_player._direction == Direction.right)
            {
                _player.Flip();
            }
            else if (_direction != 1)
            {
                _direction = 1;
            }


            NotInDeathZone = true;

            if (_rotZ > 0)
                _magnitude = 1 - ((_rotZ - 90 - _deathZoneAngle) / (90 - _deathZoneAngle));
            else
                _magnitude = ((_rotZ + 90 + _deathZoneAngle) / (-90 + _deathZoneAngle)) - 1;
        }
        else
        {
            NotInDeathZone = false;
        }

        _anim.SetFloat("Magnitude", _magnitude);
        _anim.SetFloat("Direction", _direction);
    }

    public override void Reload()
    {
        if (_timeBetweenReloads <= 0.0f && _ammoLeft > 0)
        {
            //TODO: play reload sound
            _timeBetweenReloads = _reloadTime;

            if (_ammoLeft < _maxMagCapacity)
            {
                _currentMagCapacity = _ammoLeft;
                _ammoLeft = 0;
            }
            else
            {
                _ammoLeft += _currentMagCapacity;
                _currentMagCapacity = 0;
                _currentMagCapacity = _maxMagCapacity;
                _ammoLeft -= _maxMagCapacity;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_flipPoint == null)
            return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_flipPoint.transform.position, _shotRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_flipPoint.transform.position, _maxShootDistance);
    }
}

