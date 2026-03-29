namespace ObjectsDataStructures.PaymentSystem.Domain;

public sealed record PaymentReceipt(
    bool WasPaid,
    decimal Amount,
    decimal BalanceBefore,
    decimal BalanceAfter
);