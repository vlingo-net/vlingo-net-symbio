// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Symbio
{
    /// <summary>
    /// The abstract base class of all journal entry types.
    /// </summary>
    /// <seealso cref="BinaryEntry"/>
    /// <seealso cref="ObjectEntry{T}"/>
    /// <seealso cref="TextEntry"/>
    /// <seealso cref="NullEntry"/>
    /// <typeparam name="T">The concrete type of <see cref="IEntry{T}"/> stored and read, which maybe be <c>string</c>, <c>byte[]</c>, or <c>object</c></typeparam>
    public abstract class BaseEntry<T> : IEntry<T>
    {
        protected static readonly byte[] EmptyBytesData = new byte[0];
        protected static readonly T EmptyObjectData = default!;
        protected static string EmptyTextData = string.Empty;
        protected static readonly string UnknownId = string.Empty;
        
        /// <summary>
        /// My <c>string</c> id that is unique within the <see cref="IJournal{T}"/> where persisted,
        /// and is (generally) assigned by the journal.
        /// </summary>
        private string _id;
        
        /// <summary>
        /// My data representation of the entry, generally serialized as string, byte[], or object.
        /// </summary>
        private readonly T _entryData;
        
        /// <summary>
        /// My associated <see cref="Metadata"/> if any.
        /// </summary>
        private readonly Metadata _metadata;

        /// <summary>
        /// My string type that is the fully-qualified class name of the original entry type.
        /// </summary>
        private readonly string _type;
        
        /// <summary>
        /// My int type version, which may be a semantic version or sequential version of my type.
        /// </summary>
        private readonly int _typeVersion;
        
        protected BaseEntry(string id, Type type, int typeVersion, T entryData): this(id, type, typeVersion, entryData, Metadata.NullMetadata())
        {
        }

        public BaseEntry(string id, Type type, int typeVersion, T entryData, Metadata metadata)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), "Entry id must not be null.");
            if (type == null) throw new ArgumentNullException(nameof(id), "Entry type must not be null.");
            if (typeVersion <= 0) throw new ArgumentOutOfRangeException(nameof(typeVersion), "Entry typeVersion must be greater than 0.");
            if (entryData == null) throw new ArgumentNullException(nameof(entryData), "Entry entryData must not be null.");
            if (metadata == null) throw new ArgumentNullException(nameof(metadata), "Entry metadata must not be null.");
            
            _id = id;
            _type = type.FullName;
            _typeVersion = typeVersion;
            _entryData = entryData;
            _metadata = metadata;
        }

        public BaseEntry(Type type, int typeVersion, T entryData, Metadata metadata) : this(UnknownId, type, typeVersion, entryData, metadata)
        {
        }

        /// <inheritdoc/>
        public string Id => _id;
        
        /// <inheritdoc/>
        public T EntryData => _entryData;
        
        /// <inheritdoc/>
        public Metadata Metadata => _metadata;
        
        /// <inheritdoc/>
        public string TypeName => _type;
        
        /// <inheritdoc/>
        public int TypeVersion => _typeVersion;
        
        public BinaryEntry? AsBinaryEntry() => this as BinaryEntry;
        
        public ObjectEntry<T> AsObjectEntry() => (ObjectEntry<T>) this;

        //public TextEntry AsTextEntry() => (TextEntry) this;

        /// <inheritdoc/>
        public bool HasMetadata => !_metadata.IsEmpty;

        /// <summary>
        /// Returns <c>true</c> if I am an instance of <see cref="BinaryEntry"/>. The default is to answer false.
        /// </summary>
        public virtual bool IsBinary => false;
        
        /// <summary>
        /// Returns <c>true</c> if I am an instance of <see cref="ObjectEntry{T}"/>. The default is to answer false.
        /// </summary>
        public virtual bool IsObject => false;
        
        /// <summary>
        /// Returns <c>true</c> if I am an instance of <see cref="TextEntry"/>. The default is to answer false.
        /// </summary>
        public virtual bool IsText => false;

        /// <inheritdoc/>
        public virtual bool IsEmpty => false;

        /// <inheritdoc/>
        public virtual bool IsNull => false;

        /// <inheritdoc/>
        public Type Typed => IEntry<T>.TypedFrom(_type);

        /// <summary>
        /// My string type that is the fully-qualified class name of the original entry type.
        /// </summary>
        public string Type => _type;

        public abstract IEntry<T> WithId(string id);

        public int CompareTo(IEntry<T> other)
        {
            var that = (BaseEntry<T>) other;
            var dataDiff = CompareData(this, that);
            if (dataDiff != 0) return dataDiff;

            var result = string.Compare(Id, that.Id, StringComparison.InvariantCulture);
            if (result == 0)
            {
                result = string.Compare(Type, that.Type, StringComparison.InvariantCulture);
            }
            
            if (result == 0)
            {
                result = TypeVersion.CompareTo(that.TypeVersion);
            }
            
            if (result == 0)
            {
                result = Metadata.CompareTo(that.Metadata);
            }

            return result;
        }

        public override int GetHashCode() => 31 * _id.GetHashCode();


        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType()) {
                return false;
            }
            return _id.Equals(((BaseEntry<T>) obj)._id);
        }

        public override string ToString()
        {
            return $"{GetType().Name}[id={_id} type={_type} typeVersion={_typeVersion} " +
                $"entryData={(IsText || IsObject ? _entryData!.ToString() : "(binary)")} metadata={_metadata}]";
        }

        private int CompareData(BaseEntry<T> state1, BaseEntry<T> state2)
        {
            if (state1.IsText && state2.IsText)
            {
                return string.Compare((string)(object)state1.EntryData!, (string)(object)state2.EntryData!, StringComparison.InvariantCulture);
            }
            
            if (state1.IsBinary && state2.IsBinary)
            {
                var data1 = (byte[])(object)state1.EntryData!;
                var data2 = (byte[])(object)state2.EntryData!;
                if (data1.Length == data2.Length)
                {
                    for (int idx = 0; idx < data1.Length; ++idx)
                    {
                        if (data1[idx] != data2[idx])
                        {
                            return 1;
                        }
                    }
                    return 0;
                }
                return 1;
            }
            return 1;
        }

        internal void __internal__setId(string id) => _id = id;
    }

    /// <summary>
    /// The byte[] form of <see cref="IEntry{T}"/>.
    /// </summary>
    public sealed class BinaryEntry : BaseEntry<byte[]>
    {
        public BinaryEntry(string id, Type type, int typeVersion, byte[] entryData, Metadata metadata) : base(id, type, typeVersion, entryData, metadata)
        {
        }
        
        public BinaryEntry(string id, Type type, int typeVersion, byte[] entryData) : base(id, type, typeVersion, entryData)
        {
        }
        
        public BinaryEntry(Type type, int typeVersion, byte[] entryData, Metadata metadata) : base(UnknownId, type, typeVersion, entryData, metadata)
        {
        }

        public BinaryEntry() : base(UnknownId, typeof(object), 1, EmptyBytesData, Metadata.NullMetadata())
        {
        }

        public override bool IsBinary => true;

        public override bool IsEmpty => EntryData.Length == 0;

        public override IEntry<byte[]> WithId(string id) => new BinaryEntry(id, Typed, TypeVersion, EntryData);
    }
    
    /// <summary>
    /// The object <typeparamref name="T"/> form of <see cref="IEntry{T}"/>.
    /// </summary>
    public sealed class ObjectEntry<T> : BaseEntry<T>
    {
        public ObjectEntry(string id, Type type, int typeVersion, T entryData, Metadata metadata) : base(id, type, typeVersion, entryData, metadata)
        {
        }
        
        public ObjectEntry(string id, Type type, int typeVersion, T entryData, int dataVersion) : base(id, type, typeVersion, entryData)
        {
        }
        
        public ObjectEntry(Type type, int typeVersion, T entryData, Metadata metadata) : base(UnknownId, type, typeVersion, entryData, metadata)
        {
        }

        public ObjectEntry() : base(UnknownId, typeof(T), 1, EmptyObjectData, Metadata.NullMetadata())
        {
        }

        public override bool IsObject => true;

        public override bool IsEmpty => EntryData!.Equals(EmptyObjectData);

        public override IEntry<T> WithId(string id) => new ObjectEntry<T>(id, Typed, TypeVersion, EntryData, 1);
    }
    
    /// <summary>
    /// The text string form of <see cref="IEntry{T}"/>.
    /// </summary>
    public sealed class TextEntry : BaseEntry<string>
    {
        public TextEntry(string id, Type type, int typeVersion, string entryData, Metadata metadata) : base(id, type, typeVersion, entryData, metadata)
        {
        }
        
        public TextEntry(string id, Type type, int typeVersion, string entryData) : base(id, type, typeVersion, entryData)
        {
        }
        
        public TextEntry(Type type, int typeVersion, string entryData, Metadata metadata) : base(UnknownId, type, typeVersion, entryData, metadata)
        {
        }

        public TextEntry() : base(UnknownId, typeof(string), 1, EmptyTextData, Metadata.NullMetadata())
        {
        }

        public override bool IsText => true;

        public override bool IsEmpty => string.IsNullOrEmpty(EntryData);

        public override IEntry<string> WithId(string id) => new TextEntry(id, Typed, TypeVersion, EntryData);
    }
    
    /// <summary>
    /// The object <typeparamref name="T"/> form of <see cref="IEntry{T}"/>.
    /// </summary>
    public sealed class NullEntry<T> : BaseEntry<T>
    {
        public NullEntry(T entryData) : base(UnknownId, typeof(T), 1, entryData, Metadata.NullMetadata())
        {
        }

        public override bool IsNull => true;

        public override bool IsEmpty => true;

        public override IEntry<T> WithId(string id) => this;
    }
}