using UnityEngine;

#region Callbacks
internal delegate void SimpleCallback();

internal delegate void UnitCallback(Unit a_unit);

internal delegate void OrderCallback(AOrder a_order);
internal delegate void OrderPositionCallback(AOrder a_order, Vector3 a_position);
internal delegate void OrderUnitCallback(AOrder a_order, Unit a_target);

internal delegate void Vector3Callback(Vector3 a_position);

internal delegate void AttackConfCallback(AttackConf a_attackConf);
internal delegate void AttackWrapperCallback(AttackWrapper a_attackWrapper);
internal delegate void AttackCallback(Unit a_unit, AttackWrapper a_attackWrapper);

internal delegate void DamageReportCallback(DamageReport a_report);
internal delegate void UnitDamageCallback(Unit a_unit, DamageReport a_report);
#endregion