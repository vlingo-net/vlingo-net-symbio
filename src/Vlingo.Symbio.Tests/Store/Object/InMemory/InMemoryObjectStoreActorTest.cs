// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Symbio.Store.Dispatch;
using Vlingo.Symbio.Store.Object;
using Vlingo.Symbio.Tests.Store.Dispatch;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Symbio.Tests.Store.Object.InMemory
{
    public class InMemoryObjectStoreActorTest
    {
        private MockPersistResultInterest _persistInterest;
        private MockQueryResultInterest _queryResultInterest;
        private IObjectStore _objectStore;
        private World _world;
        private MockDispatcher<Test1Source, string> _dispatcher;

        [Fact]
        public void TestThatObjectPersistsQueries()
        {
            _dispatcher.AfterCompleting(1);
            var persistAccess = _persistInterest.AfterCompleting(1);
            var person = new Person("Tom Jones", 85);
            var source = new Test1Source();
            _objectStore.Persist(person, new List<Test1Source> { source }, _persistInterest);
            var persistSize = persistAccess.ReadFrom<int>("size");
            Assert.Equal(1, persistSize);
            Assert.Equal(person, persistAccess.ReadFrom<int ,object>("object", 0));
            
            var query = MapQueryExpression.Using<Person>("find", MapQueryExpression.Map("id", person.PersistenceId));

            var queryAccess = _queryResultInterest.AfterCompleting(1);
            _objectStore.QueryObject(query, _queryResultInterest, null);
            var querySize = queryAccess.ReadFrom<int>("size");
            Assert.Equal(1, querySize);
            Assert.Equal(person, queryAccess.ReadFrom<int ,object>("object", 0));

            Assert.Equal(1, _dispatcher.DispatchedCount());
            var dispatched = _dispatcher.GetDispatched()[0];
            ValidateDispatchedState(person, dispatched);

            var dispatchedEntries = dispatched.Entries;
            Assert.Single(dispatchedEntries);
            var entry = dispatchedEntries[0];
            Assert.NotNull(entry.Id);
            Assert.Equal(source.GetType().AssemblyQualifiedName, entry.TypeName);
            Assert.Equal(Metadata.NullMetadata(), entry.Metadata);
        }
        
        public InMemoryObjectStoreActorTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            _persistInterest = new MockPersistResultInterest();
            _queryResultInterest = new MockQueryResultInterest();
            _world = World.StartWithDefaults("test-object-store");
            var entryAdapterProvider = new EntryAdapterProvider(_world);
            entryAdapterProvider.RegisterAdapter(new Test1SourceAdapter());
    
            _dispatcher = new MockDispatcher<Test1Source, string>(new MockConfirmDispatchedResultInterest());
            _objectStore = _world.ActorFor<IObjectStore>(typeof(Vlingo.Symbio.Store.Object.InMemory.InMemoryObjectStoreActor<Test1Source, string>), _dispatcher);
        }
        
        private void ValidateDispatchedState(Person persistedObject, Dispatchable<Test1Source, string> dispatched)
        {
            Assert.NotNull(dispatched);
            Assert.NotNull(dispatched.Id);

            Assert.NotNull(dispatched.State);
            var state = dispatched.TypedState<string>();
            Assert.Equal(persistedObject.PersistenceId.ToString(), state.Id);
            Assert.Equal(persistedObject.GetType().AssemblyQualifiedName, state.Type);
            Assert.Equal(Metadata.NullMetadata(), state.Metadata);
        }
    }
    
    public class Test1Source : Source<string>
    {
        private int _one = 1;

        public int One()
        {
            return _one;
        }
    }
    
    public class Test1SourceAdapter : EntryAdapter<string, Test1Source>
    {
        public override Source<string> FromEntry(IEntry<Test1Source> entry) => entry.EntryData;

        public override IEntry<Test1Source> ToEntry(Source<string> source, Metadata metadata) =>
            new ObjectEntry<Test1Source>(typeof(Test1Source), 1, (Test1Source) source, metadata);

        public override IEntry<Test1Source> ToEntry(Source<string> source, string id, Metadata metadata)=>
            new ObjectEntry<Test1Source>(id, typeof(Test1Source), 1, (Test1Source) source, metadata);
    }
}