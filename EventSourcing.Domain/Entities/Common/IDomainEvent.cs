﻿namespace EventSourcing.Domain.Entities.Common;

/// Ref: Covariance in C#
/// <summary>
/// Interface for Domain Event
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IDomainEvent<out TKey>
{
    public long AggregateVersion { get; }
    TKey AggregateId { get; }
    DateTime TimeStamp { get; }
}