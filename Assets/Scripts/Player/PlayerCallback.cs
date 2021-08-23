using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class PlayerCallback : EntityEventListener<IPlayerState>
{
    private PlayerMotor _playerMotor;
    private PlayerWeapons _playerWeapons;
    private void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
        _playerWeapons = GetComponent<PlayerWeapons>();
    }

    public override void Attached()
    {
        state.AddCallback("LifePoints", UpdatePlayerLife);      // when lifepoints is changed, call update player life.
        state.AddCallback("Pitch", _playerMotor.SetPitch);      // when client changes pitch, run this function.
        if (entity.IsOwner)
            state.LifePoints = _playerMotor.TotalLife;

    }

    public void FireEffect(float precision, int seed)
    {
        ShootEffectEvent evnt = ShootEffectEvent.Create(entity, EntityTargets.EveryoneExceptOwnerAndController);
        evnt.Precision = precision;
        evnt.Seed = seed;
        evnt.Send();
    }

    public override void OnEvent(ShootEffectEvent evnt)
    {
        _playerWeapons.FireEffect(evnt.Seed, evnt.Precision);
    }
    private void UpdatePlayerLife()
    {
        if(entity.HasControl)       // only update UI on the entity that has control.
            GUI_Controller.current.UpdateLife(state.LifePoints, _playerMotor.TotalLife);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            state.LifePoints += 10;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            state.LifePoints -= 10;
    }
}
