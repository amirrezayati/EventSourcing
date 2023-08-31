﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Domain.Entities.Common
{
    /// Ref: Covariance in C#
    /// <summary>
    /// Interface for Base Entity
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseEntity<out TKey>
    {
        TKey Id { get; }
    }

    public abstract class BaseAggregateRoot<TA, TKey> : BaseEntity<TKey>, IAggregateRoot<TKey> where TA : IAggregateRoot<TKey>
    {

        // Queuing all events
        private readonly Queue<IDomainEvent<TKey>> _events = new();

        protected BaseAggregateRoot() { }
        protected BaseAggregateRoot(TKey id) : base(id)
        {

        }

        protected void AddEvent(IDomainEvent<TKey> @event)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            _events.Enqueue(@event);
            Apply(@event);
            Version++;
        }

        /// <summary>
        /// This method is implemented in the derived class
        /// Apply this method to implement different events
        /// </summary>
        /// <param name="event"></param>

        protected abstract void Apply(IDomainEvent<TKey> @event);

        // Implementation of IAggregateRoot
        // Aggregate version
        public long Version { get; private set; }

        // Implementation of IAggregateRoot
        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        // Implementation of IAggregateRoot
        public void ClearEvents()
        {
            _events.Clear();
        }

        #region Factory

        private static readonly ConstructorInfo? CTor;

        static BaseAggregateRoot()
        {
            var aggregateType = typeof(TA);
            CTor = aggregateType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null, Type.EmptyTypes, Array.Empty<ParameterModifier>());
            if (CTor is null)
                throw new InvalidOperationException($"Unable to find required private parameter less constructor for Aggregate of type '{aggregateType.Name}'");
        }


        /// <summary>
        /// Create Base Aggregate root when Rehydrate all Events
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TA Create(IEnumerable<IDomainEvent<TKey>> events)
        {
            var domainEvents = events as IDomainEvent<TKey>[] ?? events.ToArray();
            if (events is null || !domainEvents.Any())
                throw new ArgumentNullException(nameof(events));
            var result = (TA)CTor?.Invoke(Array.Empty<object>())!;

            if (result is BaseAggregateRoot<TA, TKey> baseAggregate)
                foreach (var @event in domainEvents)
                    baseAggregate.AddEvent(@event);

            result.ClearEvents();

            return result;
        }

        #endregion  
    }
}
