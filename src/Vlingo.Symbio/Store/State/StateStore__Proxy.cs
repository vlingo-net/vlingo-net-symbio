using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.State
{
    public class StateStore__Proxy<TState, TEntry> : Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>
    {
        private const string EntryReaderRepresentation1 = "EntryReader(string)";
        private const string ReadRepresentation2 = "Read(string, Vlingo.Symbio.Store.State.IReadResultInterest)";

        private const string ReadRepresentation3 =
            "Read(string, Vlingo.Symbio.Store.State.IReadResultInterest, object)";

        private const string WriteRepresentation4 =
            "Write<TSource>(string, int, Vlingo.Symbio.Store.State.IWriteResultInterest)";

        private const string WriteRepresentation5 =
            "Write<TSource>(string, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Store.State.IWriteResultInterest)";

        private const string WriteRepresentation6 =
            "Write<TSource>(string, int, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.State.IWriteResultInterest)";

        private const string WriteRepresentation7 =
            "Write<TSource>(string, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.State.IWriteResultInterest)";

        private const string WriteRepresentation8 =
            "Write<TSource>(string, int, Vlingo.Symbio.Store.State.IWriteResultInterest, object)";

        private const string WriteRepresentation9 =
            "Write<TSource>(string, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Store.State.IWriteResultInterest, object)";

        private const string WriteRepresentation10 =
            "Write<TSource>(string, int, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.State.IWriteResultInterest, object)";

        private const string WriteRepresentation11 =
            "Write<TSource>(string, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.State.IWriteResultInterest, object)";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public StateStore__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public ICompletes<IStateStoreEntryReader<IEntry<TEntry>>> EntryReader(string name)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ => __.EntryReader(name);
                var completes = new BasicCompletes<IStateStoreEntryReader<IEntry<TEntry>>>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, EntryReaderRepresentation1);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            completes, EntryReaderRepresentation1));
                }

                return completes;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, EntryReaderRepresentation1));
            }

            return null!;
        }

        public void Read(string id, Vlingo.Symbio.Store.State.IReadResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ => __.Read(id, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, ReadRepresentation2);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            ReadRepresentation2));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ReadRepresentation2));
            }
        }

        public void Read(string id, Vlingo.Symbio.Store.State.IReadResultInterest interest, object? @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Read(id, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, ReadRepresentation3);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            ReadRepresentation3));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ReadRepresentation3));
            }
        }

        public void Write(string id, TState state, int stateVersion,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Write(id, state, stateVersion, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation4);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            WriteRepresentation4));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation4));
            }
        }

        public void Write<TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Write<TSource>(id, state, stateVersion, sources, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation5);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            WriteRepresentation5));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation5));
            }
        }

        public void Write(string id, TState state, int stateVersion, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Write(id, state, stateVersion, metadata, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation6);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            WriteRepresentation6));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation6));
            }
        }

        public void Write<TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources,
            Vlingo.Symbio.Metadata metadata, Vlingo.Symbio.Store.State.IWriteResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Write<TSource>(id, state, stateVersion, sources, metadata, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation7);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            WriteRepresentation7));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation7));
            }
        }

        public void Write(string id, TState state, int stateVersion,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Write(id, state, stateVersion, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation8);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            WriteRepresentation8));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation8));
            }
        }

        public void Write<TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Write<TSource>(id, state, stateVersion, sources, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation9);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            WriteRepresentation9));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation9));
            }
        }

        public void Write(string id, TState state, int stateVersion, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Write(id, state, stateVersion, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation10);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            WriteRepresentation10));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation10));
            }
        }

        public void Write<TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources,
            Vlingo.Symbio.Metadata metadata, Vlingo.Symbio.Store.State.IWriteResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>> cons128873 = __ =>
                    __.Write<TSource>(id, state, stateVersion, sources, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation11);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TState, TEntry>>(this.actor, cons128873,
                            WriteRepresentation11));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation11));
            }
        }
    }
}